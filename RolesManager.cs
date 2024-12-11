using System;
using System.Collections.Generic;
using System.Net;

namespace Webtech;

public delegate bool RoleCheck(HttpListenerRequest req, object attr);

public class RolesManager<T> where T : Enum {
    Dictionary<string, T> roles = new();

    public void AssociateRole(string token_val, T role) => roles.Add(token_val, role);

    public bool CheckRole(HttpListenerRequest req, object attr) {
        var constraint = (attr as AccessConstraint<T>)!;
        var key = req.Cookies["ac_token"];
        return key != null && roles[key.Value].Equals(constraint.RequiredRole);
    }
}