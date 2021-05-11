using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoManager.Domain.Interfaces
{
    public interface IDataRepositoryAdapter
    {
        Task<IReadOnlyList<RecordModel>> GetRecords(int limit);
    }
}
