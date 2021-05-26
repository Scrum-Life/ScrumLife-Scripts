using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoManager.Domain.Interfaces;

namespace Domain.Data
{
    public sealed class DataAccessorService : IDataAccessorService
    {
        private readonly ILogger<DataAccessorService> _logger;
        private readonly IDataRepositoryAdapter _repository;
        private readonly IMapper _mapper;
        private readonly IVideoAdapter _videoAdapter;

        public DataAccessorService(ILogger<DataAccessorService> logger, IDataRepositoryAdapter repository, IMapper mapper, IVideoAdapter videoAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _videoAdapter = videoAdapter ?? throw new ArgumentNullException(nameof(videoAdapter));

            _logger.LogTrace($"{GetType()} initialization");
        }

        public async Task<IReadOnlyList<PublicationModel>> GetRecords(int limit)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(GetRecords)}");

            IReadOnlyList<RecordModel> records = await _repository.GetRecords(limit);
            List<PublicationModel> pubList = new List<PublicationModel>(records.Count);

            foreach (RecordModel rec in records)
            {
                pubList.Add(new PublicationModel
                {
                    Id = rec.Id,
                    LiveVideo = new VideoMetadataModel
                    {
                        VideoTitle = rec.GetValueOrDefault<string>("[live bonus] Titre"),
                        VideoDescription = rec.GetValueOrDefault<string>("[LIVE BONUS] Description"),
                        PublicationDate = rec.GetValueOrDefault<DateTime>("Date de publication"),
                        Category = rec.GetValueOrDefault<string>("[live bonus] Catégorie"),
                        Tags = rec.GetListValues("[LIVE BONUS] Tags", ","),
                        VideoUrl = rec.GetValueOrDefault<string>("[live bonus] URL")
                    },
                    MainVideo = new VideoMetadataModel
                    {
                        VideoTitle = rec.GetValueOrDefault<string>("[youtube] Titre"),
                        VideoDescription = rec.GetValueOrDefault<string>("[youtube] Description"),
                        PinnedComment = rec.GetValueOrDefault<string>("[youtube] Commentaire à épingler"),
                        PublicationDate = rec.GetValueOrDefault<DateTime>("Date de publication"),
                        Category = rec.GetValueOrDefault<string>("[youtube] Catégorie"),
                        Tags = rec.GetListValues("[youtube] Tags", ","),
                        VideoUrl = rec.GetValueOrDefault<string>("[youtube] URL")
                    }
                });
            }

            return pubList;
        }

        public async Task<bool> UpdateLiveVideoRecord(string recordId, VideoMetadataModel videoMetadata)
        {
            _logger.LogTrace($"{GetType()} - BEGIN {nameof(UpdateLiveVideoRecord)}");

            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("[live bonus] Titre", videoMetadata.VideoTitle);
            values.Add("[LIVE BONUS] Description", videoMetadata.VideoDescription);
            values.Add("[live bonus] URL", videoMetadata.VideoUrl);

            return await _repository.UpdateRecord(recordId, values);
        }

        public async Task<bool> UpdateMainVideoRecord(string recordId, VideoMetadataModel videoMetadata)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            values.Add("[youtube] Titre", videoMetadata.VideoTitle);
            values.Add("[youtube] Description", videoMetadata.VideoDescription);
            values.Add("[youtube] URL", videoMetadata.VideoUrl);

            return await _repository.UpdateRecord(recordId, values);
        }
    }
}
