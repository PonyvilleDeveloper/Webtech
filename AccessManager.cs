using System;
using System.Net;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Convert;

namespace Webtech;

public class AccessManager {
    List<Cookie> TokenStorage;
    System.Timers.Timer TScleaner;
    Random rand;
    void CleanStorage(object src, ElapsedEventArgs e) {
        lock(TokenStorage)
            TokenStorage = new(TokenStorage.Where(c => !c.Expired));
    }
    public bool Authenticate(HttpListenerRequest info) {
        var ac_token = info.Cookies["ac_token"];
        var auth_header = info.Headers["Authorization"]?.Split()[1];
        if(ac_token == null || auth_header == null)
            return false;
        var t_c = ac_token.Value == auth_header;
        var t_s = !TokenStorage.Find(t => t.Value == ac_token.Value)?.Expired;
        return t_c && t_s == true;
    }
    public Cookie Authorize(HttpListenerResponse res) {
        Cookie ac_token = new() {
                Name = "ac_token",
                Value = ToBase64String(BitConverter.GetBytes(rand.NextInt64())),
                Expires = DateTime.Now.AddHours(1)
            };
            TokenStorage.Add(ac_token);
            res.AddHeader("Authorization", ac_token.Value);
        return ac_token;
    }
    public AccessManager(int clean_period) {
        TokenStorage = new();
        TScleaner = new(clean_period * 1000);
        TScleaner.AutoReset = true;
        TScleaner.Elapsed += CleanStorage!;
        rand = new();
    }
}