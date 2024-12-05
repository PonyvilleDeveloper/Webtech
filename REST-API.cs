using System;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using AuthFunc = System.Func<System.Net.HttpListenerRequest, bool>;

#nullable disable

namespace Webtech;
public class REST_API {                                                                           //Класс API
    CRUD[] cruds;                                                                                 //Массив методов
    AuthFunc Authentificate;
    public void FindAndRealize(string method, string url, HttpListenerContext ctx) {
        try {
            var crud = (from C in cruds                                                           //Вернуть (для каждого crud в массиве методов
                        where C.Method == method && Regex.Match(url, C.RegUrl).Success            //где метод http совпадает с методом запроса, а url подходит регулярному выр-ю из crud
                        select C).First();
            if(crud.AuthRequired) {
                if(Authentificate(ctx.Request))
                    crud.act(ctx.Request, ctx.Response);
                else
                    ctx.Response.ResponseError(401, "Auth required for calling this method.");
            } else
                crud.act(ctx.Request, ctx.Response);
        } catch (InvalidOperationException) {
            ctx.Response.ResponseError(501, "You trying call a non-exist method.");
        }
    }
    public REST_API(string path_cfg, Type ApiImpl) {                                              //Конструктор
        using (FileStream cfg = new(path_cfg, FileMode.Open))                                     //Используя поток, подключённый к файлу конфига
            cruds = JsonSerializer.Deserialize<CRUD[]>(cfg)!;                                     //Преобразуем его как json в массив методов

        for(var i = 0; i < cruds.Length; i++) {
            var realzn = ApiImpl.GetMethod(cruds[i].Name);
            if(realzn != null)
                cruds[i].act = realzn.CreateDelegate<Action>();
            else
                throw new Exception($"Class {nameof(ApiImpl)} don't have implementation for CRUD \"{cruds[i].Name}\"");
        }
        
        Authentificate = delegate { return true; };
    }
    public REST_API(string path_cfg, Type ApiImpl, AuthFunc auth_func) : this(path_cfg, ApiImpl) {
        Authentificate = auth_func;
    }
    public override string ToString()                                                             //Преобразование в строку
        => JsonSerializer.Serialize(cruds, new JsonSerializerOptions { WriteIndented = true });   //Возвращаем json API
}