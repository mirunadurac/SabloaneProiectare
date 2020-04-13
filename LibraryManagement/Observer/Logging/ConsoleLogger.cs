using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Observer.Logger
{
    public class ConsoleLogger : ILoggingService
    {

        public bool IgnoreCallerInfo { get; set; } = false;

        public void Log(Status status, string message, DateTime timeStamp, string callerName, string sourceFile, int sourceLineNumber)
        {
            if (IgnoreCallerInfo)
            {
                Console.WriteLine($"[{status.ToString()}] [{timeStamp.ToString("hh:mm:ss.fff")}] {message}");
            }
            else
            {
                Console.WriteLine($"[{status.ToString()}] [{timeStamp.ToString("hh:mm:ss.fff")}] {message} ({callerName}, {sourceFile}, {sourceLineNumber})");
            }
        }
    }
}
