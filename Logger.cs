using System;
using System.Threading.Tasks;
using System.Linq;
using static System.Console;
using static System.IO.File;

namespace Webtech {
    public class Logger {
        public required string LogFile;
        public async void Info(string source, string message) {
            await Task.Run(() => {
                ForegroundColor = ConsoleColor.White;
                var log = $"{DateTime.Now} [INFO]: ({source}) - {message}\n";
                Write(log);
                AppendAllText(LogFile, log);
                ResetColor();
            });
        }
        public async void Error(string source, string message) {
            await Task.Run(() => {
                ForegroundColor = ConsoleColor.Red;
                var log = $"{DateTime.Now} [ERROR]: ({source}) - {message}\n";
                Write(log);
                AppendAllText(LogFile, log);
                ResetColor();
            });
        }
        public async void Warning(string source, string message) {
            await Task.Run(() => {
                ForegroundColor = ConsoleColor.Yellow;
                var log = $"{DateTime.Now} [WARN]: ({source}) - {message}\n";
                Write(log);
                AppendAllText(LogFile, log);
                ResetColor();
            });
        }
        public async void Success(string source, string message) {
            await Task.Run(() => {
                ForegroundColor = ConsoleColor.Green;
                var log = $"{DateTime.Now} [SUCCESS]: ({source}) - {message}\n";
                Write(log);
                AppendAllText(LogFile, log);
                ResetColor();
            });
        }
        public async void Debug(string source, params object[] content) {
            await Task.Run(() => {
                ForegroundColor = ConsoleColor.Magenta;
                var log = $"\t{DateTime.Now} [DEBUG]: ({source})\n\t{string.Join("\n\t", content.Select(o => o.ToString()))}\n";
                Write(log);
                AppendAllText(LogFile, log);
                ResetColor();
            });
        }
    }
}