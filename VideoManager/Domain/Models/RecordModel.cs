using System.Collections.Generic;

namespace Domain.Models
{
    public sealed class RecordModel
    {
        public string Id { get; set; }

        public IDictionary<string, object> Fields { get; set; }
    }
}
