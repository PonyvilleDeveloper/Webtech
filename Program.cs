using System;
using System.IO;
using System.Text.Json;

namespace Webtech {
    class Program {
        public static void Main() {
            ApiImpl.logger = new Logger{LogFile = $"F:/Documents/Programming/C#/Webtech/logs/{DateTime.Today.Day}{DateTime.Today.Month}{DateTime.Today.Year}.log"};
            var api = new REST_API("F:/Documents/Programming/C#/Webtech/config.json", typeof(ApiImpl));
            Server? backend = new(api);
            backend.isWorking = true;
        }
    }
}