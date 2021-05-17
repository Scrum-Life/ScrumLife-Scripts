using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Video
{
    public interface IVideoService
    {
        Task<VideoMetadataModel> GetUpcomingLiveAsync(CancellationToken cancellationToken);
        Task UpdateVideoMetadata(VideoMetadataModel videoMetadata, CancellationToken cancellationToken);
        Task UploadVideoAsync(VideoModel videoModel, CancellationToken cancellationToken);
    }
}
