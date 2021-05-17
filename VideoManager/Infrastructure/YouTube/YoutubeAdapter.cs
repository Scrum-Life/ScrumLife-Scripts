using AutoMapper;
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
        private readonly YoutubeConfiguration _configuration;
        private readonly IMapper _mapper;

        private static readonly Regex _ytUrlIdParserRegex = new Regex(@"^.*(?:(?:youtu.be\/)|(?:v\/)|(?:\/u\/\w\/)|(?:embed\/)|(?:watch\?))\??v?=?([a-z0-9_-]*).*", RegexOptions.IgnoreCase);

        private const string SNIPPET_PART_PARAM = "snippet";

        public YoutubeAdapter(ILogger<YoutubeAdapter> logger, YoutubeServiceProvider ytServiceProvider, YoutubeConfiguration configuration, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ytServiceProvider = ytServiceProvider ?? throw new ArgumentNullException(nameof(ytServiceProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Caption methods
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
        #endregion

        #region Comments
        public async Task AddCommentAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                CommentThread commentThread = new CommentThread()
                {
                    Snippet = new CommentThreadSnippet
                    {
                        ChannelId = _configuration.ChannelId,
                        VideoId = GetVideoIdFromUrl(videoMetadata.VideoUrl),
                        TopLevelComment = new Comment
                        {
                            Snippet = new CommentSnippet
                            {
                                TextOriginal = videoMetadata.PinnedComment
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
        #endregion

        #region Add/update video
        public async Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadataModel, string chatMessage, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);
                
                string videoId = GetVideoIdFromUrl(videoMetadataModel.VideoUrl);
                if (!string.IsNullOrEmpty(videoId))
                {
                    IEnumerable<VideoCategory> categories = await GetCategoriesAsync(cancellationToken);
                    VideoCategory category = categories.FirstOrDefault(c => c.Snippet.Title == videoMetadataModel.Category);

                    Video video = await GetVideoMetadataInternalAsync(videoId, cancellationToken);
                    video.HydrateFromVideoModel(videoMetadataModel);
                    video.Snippet.CategoryId = category.Id;

                    VideosResource.UpdateRequest req = ytService.Videos.Update(video, new string[] { "snippet" , "status" });
                    await req.ExecuteAsync(cancellationToken);

                    if (!string.IsNullOrEmpty(chatMessage))
                    {
                        LiveChatMessage msg = new LiveChatMessage { Snippet = new LiveChatMessageSnippet {
                            LiveChatId = video.LiveStreamingDetails.ActiveLiveChatId,
                            Type = "textMessageEvent",
                            TextMessageDetails = new LiveChatTextMessageDetails { MessageText = chatMessage }
                        } };
                        ytService.LiveChatMessages.Insert(msg, SNIPPET_PART_PARAM);
                    }
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

                IEnumerable<VideoCategory> categories = await GetCategoriesAsync(cancellationToken);
                VideoCategory category = categories.FirstOrDefault(c => c.Snippet.Title == videoModel.Metadata.Category);

                Video video = new Video();
                video.HydrateFromVideoModel(videoModel.Metadata);
                video.Snippet.CategoryId = category.Id;

                VideosResource.InsertMediaUpload req = ytService.Videos.Insert(
                    video,
                    new string[] { "snippet", "status" },
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

                IUploadProgress res = await req.UploadAsync(cancellationToken);
                videoModel.VideoStream.Close();
            }
            catch(Exception e)
            {
                _logger.LogError("An error occurred while uploading video", e);
                videoModel.VideoStream.Close();
                throw;
            }
        }
        #endregion

        public async Task<VideoMetadataModel> GetUpcomingLiveAsync(CancellationToken cancellationToken)
        {
            SearchResult res = await GetUpcomingLiveInternalAsync(cancellationToken);
            VideoMetadataModel metadata = _mapper.Map<VideoMetadataModel>(res);
            metadata.VideoUrl = BuildVideoUrl(res.Id.VideoId);

            return metadata;
        }

        #region Utilities
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

        public static string BuildVideoUrl(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return $"https://www.youtube.com/watch?v={id}";
            }

            return null;
        }
        #endregion

        #region Private methods
        private async Task<IEnumerable<VideoCategory>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(GetCategoriesAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                VideoCategoriesResource.ListRequest req = ytService.VideoCategories.List(SNIPPET_PART_PARAM);
                req.Hl = "fr_FR";
                req.RegionCode = "fr";
                VideoCategoryListResponse list = await req.ExecuteAsync(cancellationToken);

                return list.Items.Where(cat => cat.Snippet.Assignable.GetValueOrDefault(false));
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving video categories", e);
                throw;
            }
        }
        private async Task<SearchResult> GetUpcomingLiveInternalAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace($"Begin {nameof(AddVideoAsync)}");
                YouTubeService ytService = await _ytServiceProvider.CreateServiceAsync(cancellationToken);

                SearchResource.ListRequest req = ytService.Search.List(SNIPPET_PART_PARAM);
                req.ChannelId = _configuration.ChannelId;
                req.EventType = SearchResource.ListRequest.EventTypeEnum.Upcoming;
                req.Type = "video";

                SearchListResponse res = await req.ExecuteAsync(cancellationToken);

                return res?.Items?.FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred while retrieving video metadata", e);
                throw;
            }
        }

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
