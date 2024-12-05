using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using static System.Web.HttpUtility;

namespace Webtech;
public static class UsefulMethods {
    public static void ResponseText(this HttpListenerResponse res, string content) {
        var buffer = Encoding.UTF8.GetBytes(content);
        res.ContentLength64 = buffer.Length;
        res.ContentType = "text/plain";
        res.StatusCode = 200;
        using (Stream output = res.OutputStream) {
            output.Write(buffer);
            output.Flush();
        }
        res.Close();
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
            res.Close();
        } else {
            res.ResponseError(404, "Not found");
        }
    }
    public static void ResponseError(this HttpListenerResponse res, int code, string mes) {
        res.StatusCode = code;
        res.StatusDescription = mes;
        using (Stream output = res.OutputStream) {
            output.Write(Encoding.UTF8.GetBytes($"{code} {mes}"));
            output.Flush();
        }
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
    public static Dictionary<string, string> ParseForm(this HttpListenerRequest req) {
        var result = new Dictionary<string, string>();
        string[] form;
        using(var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            form = reader.ReadToEnd().Split('&');
        foreach(var kv in form)
            result.Add(UrlDecode(kv.Split('=')[0]), UrlDecode(kv.Split('=')[1]));
        return result;
    }
    public static Dictionary<string, string> ParseQuery(this HttpListenerRequest req) {
        var result = new Dictionary<string, string>();
		result.AddRange(
			
		);
		return result;
    }
}