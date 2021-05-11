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

        public DataAccessorService(ILogger<DataAccessorService> logger, IDataRepositoryAdapter repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IReadOnlyList<RecordModel>> GetRecords(int limit)
        {
            return await _repository.GetRecords(limit);
        }
    }
}
