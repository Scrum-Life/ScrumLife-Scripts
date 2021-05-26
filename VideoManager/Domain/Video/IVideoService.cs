using Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Video
{
    public interface IVideoService
    {
        Task CommentVideoAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken);
        Task<VideoMetadataModel> GetUpcomingLiveAsync(CancellationToken cancellationToken);
        Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken);
        Task UploadVideoAsync(VideoModel videoModel, IProgress<UploadStatusModel> progress, CancellationToken cancellationToken);
    }
}
