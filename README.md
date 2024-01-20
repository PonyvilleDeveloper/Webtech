# Webtech
.net8 DLL with simple HTTP-Serve rrunning in the background in a separate thread
# Usage
```C#
using Webtech;

namespace Foo {
    class Program {
        public static void Main() {
            var api = new REST_API($"{ApiImpl.WorkDir}apiconfig.json", typeof(ApiImpl));
            Server? backend = new(api);
            backend.Work = true;

            ApiImpl.logger.Info("MAIN", "Server started...");
            Console.ReadKey();
        }
    }
}
```
