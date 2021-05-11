﻿using AutoMapper;
using Domain.Models;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VideoManager.Domain.Interfaces;

namespace VideoManager.Infrastructure.YouTube
{
    public class YoutubeAdapter : IVideoAdapter
    {
        private readonly ILogger<YoutubeAdapter> _logger;
        private readonly YoutubeServiceProvider _ytServiceProvider;
        private readonly IMapper _mapper;

        private static readonly Regex _ytUrlIdParserRegex = new Regex(@"^.*(?:(?:youtu.be\/)|(?:v\/)|(?:\/u\/\w\/)|(?:embed\/)|(?:watch\?))\??v?=?([a-z0-9_-]*).*", RegexOptions.IgnoreCase);

        private const string SNIPPET_PART_PARAM = "snippet";

        public YoutubeAdapter(ILogger<YoutubeAdapter> logger, YoutubeServiceProvider ytServiceProvider, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ytServiceProvider = ytServiceProvider ?? throw new ArgumentNullException(nameof(ytServiceProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IList<string>> ListCaptionsAsync(string videoID, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Begin {nameof(ListCaptionsAsync)} for video {videoID}");
            IList<Caption> res = await ListCaptionsInternalAsync(videoID, cancellationToken);
            return res.Select(c => c.Snippet.Language).ToList();
        }

        public async Task<bool> AddOrUpdateCaptionAsync(string videoID, string language, Stream captionStream, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddOrUpdateCaptionAsync)} for video {videoID}");
                bool res = false;

                //Checks if a caption already exists for this video
                IList<Caption> list = await ListCaptionsInternalAsync(videoID, cancellationToken);
                Caption caption = list.FirstOrDefault(c => c.Snippet.Language == language);

                if(caption != null)
                {
                    caption.Snippet.IsDraft = false;
                    caption.Snippet.IsAutoSynced = false;
                    res = await UpdateCaptionInternalAsync(caption, captionStream, cancellationToken);
                }
                else
                {
                    caption = new Caption
                    {
                        Snippet = new CaptionSnippet
                        {
                            VideoId = videoID,
                            IsDraft = false,
                            IsAutoSynced = false
                        }
                    };

                    res = await AddCaptionInternalAsync(caption, captionStream, cancellationToken);
                }

                return res;
            }
            catch(Exception e)
            {
                _logger.LogError("An error occured while uploading caption", e);
                return false;
            }
        }

        public async Task<IQueryable<VideoCategoryModel>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(GetCategoriesAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                VideoCategoriesResource.ListRequest req = ytService.VideoCategories.List(SNIPPET_PART_PARAM);
                req.Hl = "fr_FR";
                req.RegionCode = "fr";
                VideoCategoryListResponse list = await req.ExecuteAsync(cancellationToken);

                return _mapper.ProjectTo<VideoCategoryModel>(list.Items.Where(cat => cat.Snippet.Assignable.GetValueOrDefault(false)).AsQueryable());
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving video categories", e);
                throw;
            }
        }

        public async Task AddCommentAsync(string comment, string videoId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                CommentThread commentThread = new CommentThread()
                {
                    Snippet = new CommentThreadSnippet
                    {
                        VideoId = videoId,
                        IsPublic = true,
                        TopLevelComment = new Comment
                        {
                            Snippet = new CommentSnippet
                            {
                                VideoId = videoId,
                                TextOriginal = comment
                            }
                        }
                    }
                };

                CommentThreadsResource.InsertRequest req = ytService.CommentThreads.Insert(commentThread, SNIPPET_PART_PARAM);
                CommentThread res = await req.ExecuteAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while adding comment to video", e);
                throw;
            }
        }

        public async Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadataModel, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                string videoId = GetVideoIdFromUrl(videoMetadataModel.VideoUrl);
                if (!string.IsNullOrEmpty(videoId))
                {
                    Video video = await GetVideoMetadataInternalAsync(videoId, cancellationToken);
                    video.Snippet.Title = videoMetadataModel.VideoTitle;
                    video.Snippet.Description = videoMetadataModel.VideoDescription;

                    VideosResource.UpdateRequest req = ytService.Videos.Update(video, "snippet");
                    await req.ExecuteAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while updating video", e);
                throw;
            }
        }

        public async Task AddVideoAsync(VideoModel videoModel, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);
                Video video = _mapper.Map<Video>(videoModel);

                video.Snippet = new VideoSnippet
                {
                    CategoryId = "27",
                    DefaultLanguage = "fr",
                    Description = "Ceci est une description",
                    DefaultAudioLanguage = "fr",
                    Title = "Ceci est un titre",
                    Tags = new List<string>() { "test", "toto", "tropbien" }
                };

                video.AgeGating = new VideoAgeGating { AlcoholContent = false };
                video.MonetizationDetails = new VideoMonetizationDetails { Access = new AccessPolicy { Allowed = false } };
                video.Status = new VideoStatus
                {
                    MadeForKids = false,
                    PrivacyStatus = "private",
                    PublishAt = DateTime.UtcNow.AddHours(6)
                };

                VideosResource.InsertMediaUpload req = ytService.Videos.Insert(
                    video,
                    new string[] { "snippet", "liveStreamingDetails", "status" },
                    videoModel.VideoStream,
                    "application/octet-stream"
                );

                req.NotifySubscribers = true;

                req.ProgressChanged += ((uProgress) =>
                {
                    _logger.LogInformation($"Upload of video : {uProgress.BytesSent} bytes sent");
                });

                req.ResponseReceived += ((video) =>
                {
                    _logger.LogInformation($"Upload of video is complete : {video.Id}");
                    if (!string.IsNullOrEmpty(video.ProcessingDetails?.ProcessingFailureReason))
                    {
                        _logger.LogError($"Error while processing video upload : {video.ProcessingDetails.ProcessingFailureReason}");
                        _logger.LogError($"{video.ProcessingDetails.ProcessingIssuesAvailability ?? "No reason"}");
                    }
                });

                await req.UploadAsync(cancellationToken);
                videoModel.VideoStream.Close();
            }
            catch(Exception e)
            {
                _logger.LogError("An error occurred while uploading video", e);
                videoModel.VideoStream.Close();
                throw;
            }
        }

        public static string GetVideoIdFromUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Match match = _ytUrlIdParserRegex.Match(url);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return null;
        }

        #region Private methods
        private async Task<IList<Caption>> ListCaptionsInternalAsync(string videoID, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Begin {nameof(ListCaptionsInternalAsync)} for video {videoID}");
            YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

            CaptionsResource.ListRequest captionsRequest = ytService.Captions.List(SNIPPET_PART_PARAM, videoID);

            CaptionListResponse res = await captionsRequest.ExecuteAsync(cancellationToken);

            return res.Items;
        }

        private async Task<bool> AddCaptionInternalAsync(Caption caption, Stream captionStream, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddCaptionInternalAsync)} for video {caption.Snippet.VideoId}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                //create the request now and insert our params...
                ResumableUpload captionRequest = ytService.Captions.Insert(caption, SNIPPET_PART_PARAM, captionStream, "application/atom+xml");

                return await UploadCaptionAsync(captionRequest, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while uploading caption", e);
                return false;
            }
        }

        private async Task<bool> UpdateCaptionInternalAsync(Caption caption, Stream captionStream, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(UpdateCaptionInternalAsync)} for video {caption.Snippet.VideoId}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                //using MemoryStream ms = new MemoryStream(captionBytes);
                //create the request now and insert our params...
                ResumableUpload captionRequest = ytService.Captions.Update(caption, SNIPPET_PART_PARAM, captionStream, "application/atom+xml");

                return await UploadCaptionAsync(captionRequest, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while uploading caption", e);
                return false;
            }
        }

        private async Task<bool> UploadCaptionAsync(ResumableUpload uploadRequest, CancellationToken cancellationToken)
        {
            //finally upload the request... and wait.
            //TODO Créer un taskmanager pour suivre l'avancement des taches lancées
            IUploadProgress res = await uploadRequest.UploadAsync(cancellationToken);
            UploadStatus s = res.Status;

            while (s != UploadStatus.Completed && s != UploadStatus.Failed)
            {
                _logger.LogInformation($"Caption upload in progress : {s}");
                await Task.Delay(500);
            }

            if (s == UploadStatus.Completed)
            {
                _logger.LogInformation($"Caption upload completed with success");
                return true;
            }
            else
            {
                _logger.LogError($"Caption upload failed : {res.Exception?.Message ?? "Unexpected error ¯\\(°_o)/¯"}");
                return false;
            }
        }

        private async Task<Video> GetVideoMetadataInternalAsync(string videoId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                VideosResource.ListRequest req = ytService.Videos.List(SNIPPET_PART_PARAM);
                req.Id = videoId;

                VideoListResponse res = await req.ExecuteAsync(cancellationToken);

                return res?.Items?.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving video metadata", e);
                throw;
            }
        }
        #endregion
    }
}
