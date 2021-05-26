using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VideoManager.Domain.Interfaces;

namespace Domain.Video
{
    public class VideoService : IVideoService
    {
        private readonly ILogger<VideoService> _logger;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IMapper _mapper;

        public VideoService(ILogger<VideoService> logger, IVideoAdapter videoAdapter, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _videoAdapter = videoAdapter ?? throw new ArgumentNullException(nameof(videoAdapter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _logger.LogTrace($"{GetType()} initialization");
        }

        public async Task UploadVideoAsync(VideoModel videoModel, IProgress<UploadStatusModel> progress, CancellationToken cancellationToken)
        {
            await _videoAdapter.AddVideoAsync(videoModel, progress, cancellationToken);
        }
        

        public async Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken) =>
            await _videoAdapter.UpdateVideoMetadataAsync(videoMetadata, null, cancellationToken);

        public async Task<VideoMetadataModel> GetUpcomingLiveAsync(CancellationToken cancellationToken) =>
            await _videoAdapter.GetUpcomingLiveAsync(cancellationToken);

        public async Task CommentVideoAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken) =>
            await _videoAdapter.AddCommentAsync(videoMetadata, cancellationToken);
    }
}
