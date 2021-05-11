using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Data
{
    public interface IDataAccessorService
    {
        Task<IReadOnlyList<RecordModel>> GetRecords(int limit);
    }
}
