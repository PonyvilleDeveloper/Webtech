using System;
using System.Net;
using System.Text.Json.Serialization;

#nullable disable

namespace Webtech {
    public delegate void Action(HttpListenerRequest req, HttpListenerResponse res);
    internal struct CRUD {
        public string Name { get; set; }
        [JsonPropertyName("Url")]
        public string RegUrl { get; set; }
        public string Method { get; set; }
        [JsonIgnore]
        public Action act;
    }
}