using System;
using System.Net;

namespace Webtech;

#nullable disable

public class Token<T>(T r) where T: Enum {
    public Cookie Bellhop { get; private set; } = new();
    public T UserRole { get; init; } = r;
    public DateTime Expires {
        get => Bellhop.Expires;
        set => Bellhop.Expires = value;
    }
    public string Value {
        get => Bellhop.Value;
        set => Bellhop.Value = value;
    }
}