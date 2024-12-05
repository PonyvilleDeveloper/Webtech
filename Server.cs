using System.Net;
using System.Threading;
using System.Threading.Tasks;

#nullable disable

namespace Webtech;
public class Server {
    readonly HttpListener HTTP;
    readonly REST_API API;
    readonly Thread Listening;
    bool doWork;
    async void Listen() {
        while(this.doWork) {
            var ctx = await HTTP.GetContextAsync();
            await Task.Run(() => {
                API.FindAndRealize(ctx.Request.HttpMethod, ctx.Request.RawUrl, ctx);
            });
        }
    }
    public bool Work {
        get => doWork;
        set {
            doWork = value;
            if(value) {
                HTTP.Start();
                Listening.Start();
            } else {
                Listening.Join();
                HTTP.Stop();
            }
        }
    }
    public Server(REST_API api, bool use_https = false, params IPEndPoint[] addresses) {
        API = api;                                                  //Установка API
        HTTP = new();                                               //Инициализация прослушивателя
        if(addresses.Length == 0)                                   //Если нет заданных адресов
            HTTP.Prefixes.Add($"http{(use_https ? "s" : "")}://127.0.0.1:8080/"); //Добавляется адрес по умолчанию
        else                                                        //Иначе
            foreach(var addr in addresses)
                HTTP.Prefixes.Add($"http{(use_https ? "s" : "")}://" + addr.ToString() + "/"); //Добавляется каждый
        Listening = new(Listen);
    }
    ~Server() {
        Listening.Join();
        HTTP.Close();
    }
}