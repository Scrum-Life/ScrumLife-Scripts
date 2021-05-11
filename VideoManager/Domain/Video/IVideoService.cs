using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Video
{
    public interface IVideoService
    {
        Task UpdateVideoMetadata(VideoMetadataModel videoMetadata, CancellationToken cancellationToken);
    }
}
