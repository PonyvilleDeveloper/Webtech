using System;
using System.Net;
using System.Text;

#nullable disable

namespace Webtech {
    public class Server {
        HttpListener HTTP;
        REST_API API;
        Thread Listening;
        bool work;
        void Listen() {
            while(this.work) {
                var ctx = HTTP.GetContext();
                API[ctx.Request.HttpMethod, ctx.Request.RawUrl](ctx.Request, ctx.Response);
            }
        }
        public bool isWorking {
            get {
                return work;
            }
            set {
                work = value;
                if(value) {
                    HTTP.Start();
                    Listening.Start();
                } else {
                    Listening.Join();
                    HTTP.Stop();
                }
            }
        }
        public Server(REST_API api, string IP = "127.0.0.1", params int[] ports) {
            API = api;
            HTTP = new();
            foreach (var p in ports)
                HTTP.Prefixes.Add($"http://{IP}:{p}/");
            if (ports.Length == 0)
                HTTP.Prefixes.Add($"http://{IP}:8080/");
            Listening = new(Listen);
        }
        ~Server() {
            HTTP.Close();
        }
    }
}