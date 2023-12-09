using System;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using static System.IO.File;

#nullable disable

namespace Webtech {
    internal class ApiImpl {
        public static Logger logger {get; set;}
        public static void Hello(HttpListenerRequest req, HttpListenerResponse res) {
            logger.Success("API", "Greetings requested");
            res.StatusCode = 200;
            res.ResponseText("Hello World!");
        }
        public static void TechInfo(HttpListenerRequest req, HttpListenerResponse res) {
            logger.Warning("API", "Tech info requested");
            var server = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            var result = $@"Assembly: {Assembly.GetExecutingAssembly().Location}
Author: {server.CompanyName}
Version: {server.FileVersion}
Comments: {server.Comments}

[Request]:
    User-Agent: {req.UserAgent}
    Method: {req.HttpMethod}
    Auth: {req.IsAuthenticated}
    HasData: {req.HasEntityBody}
    Cookies: {string.Join('\n', req.Cookies)}";
            res.StatusCode = 200;
            res.ResponseText(result);
        }
        public static void Page(HttpListenerRequest req, HttpListenerResponse res) {
            if (!Exists($"F:/Documents/Programming/C#/Webtech/resources/pages{req.RawUrl}.html")) {
                logger.Error("API", "Non-existed or not founded page requested");
                return;
            }
            logger.Info("API", "Page requested " + req.RawUrl);
            res.StatusCode = 200;
            res.ResponseFile($"F:/Documents/Programming/C#/Webtech/resources/pages{req.RawUrl}.html");
        }
    }
}