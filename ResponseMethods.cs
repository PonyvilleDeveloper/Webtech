using System;
using System.Net;
using System.Text;

namespace Webtech {
    internal static class ResponseMethods {
        public static void ResponseText(this HttpListenerResponse res, string content) {
            var buffer = Encoding.UTF8.GetBytes(content);
            res.ContentLength64 = buffer.Length;
            res.ContentType = "text/plain";
            using (Stream output = res.OutputStream) {
                output.Write(buffer);
                output.Flush();
            }
        }
        public static void ResponseFile(this HttpListenerResponse res, string path) {
            var info = new FileInfo(path);
            if(info.Exists) {
                res.StatusCode = 200;
                var buffer = File.ReadAllBytes(path);
                res.ContentLength64 = buffer.Length;
                res.ContentType = info.GetMimeType();
                using (Stream output = res.OutputStream) {
                    output.Write(buffer);
                    output.Flush();
                }
            } else {
                res.ResponseError(404, "Not found");
            }
        }
        public static void ResponseError(this HttpListenerResponse res, int code, string mes) {
            res.StatusCode = code;
            res.StatusDescription = mes;
            res.Close();
        }
        public static string GetMimeType(this FileInfo file) {
            var ext = file.Extension.TrimStart('.');
            return ext switch {
                //Text
                "htm" or "html"
                or "css" or "cmd"
                or "csv " or "xml" => "text/" + ext,
                "js" => "text/javascript",
                //Image
                "jpg" or "jpeg"
                or "png" or "gif" => "image/" + ext,
                "svg" => "image/svg+xml",
                //Audio
                "mp3" => "audio/mpeg",
                //Video
                "mp4" or "ogg"
                or "webm" => "video/" + ext,
                _ => "application/" + ext,
            };
        }
    }
}