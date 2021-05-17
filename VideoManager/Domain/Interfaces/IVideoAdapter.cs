using Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VideoManager.Domain.Interfaces
{
    public interface IVideoAdapter
    {
        Task AddCommentAsync(VideoMetadataModel videoMetadata, CancellationToken cancellationToken);
        Task<bool> AddOrUpdateCaptionAsync(string videoID, string language, Stream captionStream, CancellationToken cancellationToken);
        Task AddVideoAsync(VideoModel videoModel, CancellationToken cancellationToken);
        Task<VideoMetadataModel> GetUpcomingLiveAsync(CancellationToken cancellationToken);
        #region Captions
        #endregion

        #region Video

        #endregion
        Task<IList<string>> ListCaptionsAsync(string videoID, CancellationToken cancellationToken);
        Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadataModel, string liveChatMessage, CancellationToken cancellationToken);
    }
}
