using System;
using System.Net;
using static System.IO.File;

namespace Webtech;

#nullable disable

public class BaseApi {
    public static Logger logger;
    public static string WorkDir;

    static BaseApi() {
        WorkDir = Environment.CurrentDirectory;
        logger = new() {LogFile = $"{WorkDir}/logs/{DateTime.Today.Day}{DateTime.Today.Month}{DateTime.Today.Year}.log"};
    }
    public static void Resource(HttpListenerRequest req, HttpListenerResponse res) {
        if (!Exists($"{WorkDir}{req.RawUrl}")) {
            logger.Error("API-Resource", $"Non-existed or not founded resource requested {req.RawUrl}");
            res.ResponseError(404, "Non-existed or not founded resource requested");
            return;
        }
        logger.Info("API-Resource", "Resource requested " + req.RawUrl);
        res.ResponseFile($"{WorkDir}/{req.RawUrl}");
    }
    public static void Page(HttpListenerRequest req, HttpListenerResponse res) {
        var page = req.Url.LocalPath;
        if (!Exists($"{WorkDir}resources/pages{page}.html")) {
            logger.Error("API-Page", $"Non-existed or not founded page requested {page}");
            res.ResponseError(404, "Non-existed or not founded page requested");
            return;
        }
        logger.Info("API-Page", "Page requested " + req.RawUrl);
        res.ResponseFile($"{WorkDir}resources/pages{page}.html");
    }
}