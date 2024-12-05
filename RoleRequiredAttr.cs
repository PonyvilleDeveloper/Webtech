using System;

namespace Webtech;

[AttributeUsage(AttributeTargets.Method)]
public class AccessConstraint<T>(T r) : Attribute where T : Enum {
    public T RequiredRole { get; init; } = r;
}