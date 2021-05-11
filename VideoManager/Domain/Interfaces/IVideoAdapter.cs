using Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoManager.Domain.Interfaces
{
    public interface IVideoAdapter
    {
        Task AddCommentAsync(string comment, string videoId, CancellationToken cancellationToken);
        Task<bool> AddOrUpdateCaptionAsync(string videoID, string language, Stream captionStream, CancellationToken cancellationToken);
        Task AddVideoAsync(VideoModel videoModel, CancellationToken cancellationToken);
        Task<IQueryable<VideoCategoryModel>> GetCategoriesAsync(CancellationToken cancellationToken);
        #region Captions
        #endregion

        #region Video

        #endregion
        Task<IList<string>> ListCaptionsAsync(string videoID, CancellationToken cancellationToken);
        Task UpdateVideoMetadataAsync(VideoMetadataModel videoMetadataModel, CancellationToken cancellationToken);
    }
}
