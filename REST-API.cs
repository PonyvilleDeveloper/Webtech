using System;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

#nullable disable

namespace Webtech;
public delegate bool AuthFunc(HttpListenerRequest req);
public class REST_API {                                                                           //Класс API
    CRUD[] cruds;                                                                                 //Массив методов
    readonly AuthFunc Authentificate;
    readonly RoleCheck CheckingRole;
    private void ProcessCrud(int idx, HttpListenerContext ctx) {
        var crud = cruds[idx];
        if(crud.AuthRequired)
            if(Authentificate(ctx.Request))
                if(CheckingRole(ctx.Request, crud.role_attr))
                    crud.act(ctx.Request, ctx.Response);
                else
                    ctx.Response.ResponseError(403, "Your role does not have sufficient rights to call this method.");
            else
                ctx.Response.ResponseError(401, "Auth required for calling this method.");
        else
            crud.act(ctx.Request, ctx.Response);
    }
    public void FindAndRealize(string method, string url, HttpListenerContext ctx) {
        try {
            var idx = 0;
            for(var c = cruds[idx]; c.Method != method && !Regex.Match(url, c.RegUrl).Success; idx++)
                continue;
            ProcessCrud(idx, ctx);
        } catch (InvalidOperationException) {
            ctx.Response.ResponseError(501, "You trying call a non-exist method.");
        }
    }
    public REST_API(string path_cfg, Type ApiImpl) {                                              //Конструктор
        using (FileStream cfg = new(path_cfg, FileMode.Open))                                     //Используя поток, подключённый к файлу конфига
            cruds = JsonSerializer.Deserialize<CRUD[]>(cfg)!;                                     //Преобразуем его как json в массив методов

        for(var i = 0; i < cruds.Length; i++) {
            var realzn = ApiImpl.GetMethod(cruds[i].Name);
            cruds[i].role_attr = realzn.GetCustomAttributes(false).First();
            if(realzn != null)
                cruds[i].act = realzn.CreateDelegate<Action>();
            else
                throw new Exception($"Class {nameof(ApiImpl)} don't have implementation for CRUD \"{cruds[i].Name}\"");
        }
        
        Authentificate = delegate { return true; };
        CheckingRole = delegate { return true; };
    }
    public REST_API(string path_cfg, Type ApiImpl, AuthFunc auth_func) : this(path_cfg, ApiImpl) {
        Authentificate = auth_func;
    }
    public REST_API(string path_cfg, Type ApiImpl, AuthFunc auth_func, RoleCheck RoleChecker) : this(path_cfg, ApiImpl) {
        Authentificate = auth_func;
        CheckingRole = RoleChecker;
    }
    public override string ToString()                                                             //Преобразование в строку
        => JsonSerializer.Serialize(cruds, new JsonSerializerOptions { WriteIndented = true });   //Возвращаем json API
}