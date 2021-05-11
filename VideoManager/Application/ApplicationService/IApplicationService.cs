using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VideoManager.Application.ApplicationService
{
    public interface IApplicationService
    {
        Task GetRecords();
        Task<IDictionary<string, string>> GetVideoCategories(CancellationToken cancellationToken = default);
        Task UploadCaptionToVideoAsync(string videoId, string language, Stream captionStream, CancellationToken cancellationToken = default);
        Task UploadVideoAsync(Stream videoStream, CancellationToken cancellationToken = default);
    }
}