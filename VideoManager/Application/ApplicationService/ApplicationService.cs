using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoManager.Infrastructure.Interfaces;

namespace VideoManager.Application.ApplicationService
{
    public class ApplicationService : IApplicationService
    {
        private readonly ILogger<ApplicationService> _logger;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IDataRepositoryAdapter _dataRepositoryAdapter;

        public ApplicationService(ILogger<ApplicationService> logger, IVideoAdapter videoAdapter, IDataRepositoryAdapter dataRepositoryAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _videoAdapter = videoAdapter ?? throw new ArgumentNullException(nameof(videoAdapter));
            _dataRepositoryAdapter = dataRepositoryAdapter ?? throw new ArgumentNullException(nameof(dataRepositoryAdapter));
        }

        public async Task UploadCaptionToVideoAsync(string videoId, string language, Stream captionStream, CancellationToken cancellationToken = default)
        {
            await _videoAdapter.AddOrUpdateCaptionAsync(videoId, language, captionStream, cancellationToken);
        }

        public async Task UploadVideoAsync(Stream videoStream, CancellationToken cancellationToken = default)
        {
            VideoModel videoModel = new VideoModel()
            {
                VideoStream = videoStream
            };
            
            await _videoAdapter.AddVideoAsync(videoModel, cancellationToken);
        }

        public async Task<IDictionary<string, string>> GetVideoCategories(CancellationToken cancellationToken = default)
        {
            IQueryable<VideoCategoryModel> res = await _videoAdapter.GetCategoriesAsync(cancellationToken);
            return res.ToDictionary(k => k.Id, v => v.Name);
        }

        public async Task<IReadOnlyList<RecordModel>> GetRecords()
        {
            await _dataRepositoryAdapter.GetRecords(10);
        }
    }
}
