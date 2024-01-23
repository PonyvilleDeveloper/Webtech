# Webtech
.net8 DLL with simple HTTP-Server running in the background in a separate thread
# Usage
```C#
using Webtech;

namespace Foo {
    class Program {
        public static void Main() {
            var api = new REST_API($"{ApiImpl.WorkDir}apiconfig.json", typeof(ApiImpl)); //Available on 127.0.0.1:8080
            Server? backend = new(api);
            backend.Work = true;

            ApiImpl.logger.Info("MAIN", "Server started...");
            Console.ReadKey();
        }
    }
}
```
or
```C#
using Webtech;

namespace Foo {
    class Program {
        public static void Main() {
            var api = new REST_API($"{ApiImpl.WorkDir}apiconfig.json", typeof(ApiImpl));
            Server? backend = new(api, new IPEndPoint(neew IPAdress(new byte[] {127, 0, 0, 1}), 8080));
            backend.Work = true;

            ApiImpl.logger.Info("MAIN", "Server started...");
            Console.ReadKey();
        }
    }
}
```
## API config json example
```JSON
[
    {
        "Name": "Resource",
        "Url": "resources/\\w+/\\w+\\.\\w+",
        "AuthRequired": false,
        "Method": "GET"
    },
    {
        "Name": "TechInfo",
        "Url": "tech",
        "AuthRequired": true,
        "Method": "GET"
    },
    {
        "Name": "Page",
        "Url": "\\w+",
        "AuthRequired": false,
        "Method": "GET"
    },
    {
        "Name": "Auth",
        "Url": "auth",
        "AuthRequired": false,
        "Method": "POST"
    }
]
```
