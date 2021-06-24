using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imago.IO.Classes
{
    public class ApiErrorMessage
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ApiErrorResponse
    {
        [JsonProperty("errors")]
        public ApiErrorMessage[] Errors { get; set; } = Array.Empty<ApiErrorMessage>();
    }
}
