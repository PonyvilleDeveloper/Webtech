using System;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

#nullable disable

namespace Webtech {    
    public class REST_API {
        CRUD[] cruds;
        void UnknownAction(HttpListenerRequest req, HttpListenerResponse res) {
            res.StatusCode = 406;
            res.ResponseText("Non-existing api method.");
        }

        public Action this[string method, string url] {
            get {
                try {
                    return (from crud in cruds
                            where crud.Method == method && Regex.Match(url, crud.RegUrl).Success
                            select crud).First().act;
                } catch (InvalidOperationException) {
                    return UnknownAction;
                }
            }
        }
        public REST_API(string path_cfg, Type ApiImpl) {
            using (FileStream cfg = new(path_cfg, FileMode.Open)) {
                cruds = JsonSerializer.Deserialize<CRUD[]>(cfg);
            }
            for (var i = 0; i < cruds.Length; i++)
                cruds[i].act = ApiImpl.GetMethod(cruds[i].Name).CreateDelegate<Action>();
        }
        public override string ToString() {
            return JsonSerializer.Serialize(cruds, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}