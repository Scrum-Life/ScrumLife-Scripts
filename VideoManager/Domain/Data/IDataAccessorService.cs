using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Data
{
    public interface IDataAccessorService
    {
        Task<IReadOnlyList<PublicationModel>> GetRecords(int limit);
        Task<bool> UpdateLiveVideoRecord(string recordId, VideoMetadataModel videoMetadata);
    }
}
