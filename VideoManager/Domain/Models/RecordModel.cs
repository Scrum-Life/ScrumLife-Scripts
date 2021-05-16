using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public sealed class RecordModel
    {
        public string Id { get; set; }

        public IDictionary<string, object> Fields { get; set; }

        public T GetValueOrDefault<T>(string key, T defaultValue = null) where T : class
        {
            return Fields.TryGetValue(key, out object res) ? res as T : defaultValue;
        }

        public T? GetValueOrDefault<T>(string key, T? defaultValue = null) where T : struct
        {
            return Fields.TryGetValue(key, out object res) ? res as T? : defaultValue;
        }

        public IList<string> GetListValues(string key, string separator)
        {
            return Fields.TryGetValue(key, out object res) 
                ? res.ToString().Split(separator).ToList() 
                : new List<string>();
        }
    }
}
