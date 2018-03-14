using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TimeIsMoney.Common.ErrorHandler
{
    public sealed class ErrorCollection
    {
        [JsonProperty("errors")]
        private Dictionary<string, List<string>> _errors { get; set; } = new Dictionary<string, List<string>>();

        [JsonIgnore]
        public bool HasErrors => _errors.Any();

        public void AddError(string key, string error)
        {
            if (!_errors.TryGetValue(key, out List<string> errors))
            {
                errors = new List<string>();
                _errors[key] = errors;
            }
            errors.Add(error);
        }
    }
}