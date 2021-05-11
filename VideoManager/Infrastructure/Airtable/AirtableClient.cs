using AirtableApiClient;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoManager.Domain.Interfaces;

namespace VideoManager.Infrastructure.Airtable
{
    public class AirtableClient : IDataRepositoryAdapter
    {
        private readonly ILogger<IDataRepositoryAdapter> _logger;
        private readonly IMapper _mapper;
        private readonly AirtableConfiguration _config;

        public AirtableClient(ILogger<IDataRepositoryAdapter> logger, IMapper mapper, AirtableConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IReadOnlyList<RecordModel>> GetRecords(int limit)
        {
            string errorMessage = "";
            string offset;
            List<RecordModel> results = new List<RecordModel>();

            using (AirtableBase airtableBase = new AirtableBase(_config.ApiKey, _config.DatabaseId))
            {
                do
                {
                    AirtableListRecordsResponse res = await airtableBase.ListRecords(_config.TableName, maxRecords: limit);

                    if (res.Success)
                    {
                        results.AddRange(_mapper.ProjectTo<RecordModel>(res.Records.AsQueryable()));
                        offset = res.Offset;
                    }
                    else if (res.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = res.AirtableApiError.ErrorMessage;
                        break;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                        break;
                    }
                } while (offset != null);
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _logger.LogError(errorMessage);
            }

            return results;
        }
    }
}
