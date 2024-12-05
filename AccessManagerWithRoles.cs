using System;
using System.Net;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using static System.Convert;

namespace Webtech;

public class AccessManagerWithRoles {
    List<Cookie> TokenStorage;
    Timer TScleaner;
    Random rand;
    int token_life_duration;
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
        var t_s = !TokenStorage.Find(t => t.Value == ac_token.Value)?.Expired ?? false;
        return t_c && t_s;
    }
    public Cookie Authorize(HttpListenerResponse res) {
        Cookie ac_token = new() {
                Name = "ac_token",
                Value = ToBase64String(BitConverter.GetBytes(rand.NextInt64())),
                Expires = DateTime.Now.AddHours(token_life_duration)
            };
            TokenStorage.Add(ac_token);
            res.AddHeader("Authorization", ac_token.Value);
        return ac_token;
    }
    public AccessManagerWithRoles(int token_life_hours = 1) {
        token_life_duration = token_life_hours;
        TokenStorage = new();
        TScleaner = new(token_life_hours * 3600 * 1000);
        TScleaner.AutoReset = true;
        TScleaner.Elapsed += CleanStorage!;
        rand = new();
    }
}