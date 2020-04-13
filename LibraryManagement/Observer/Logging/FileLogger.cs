using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibraryManagement.Observer.Logger
{
    public class FileLogger : ILoggingService
    {
        public FileInfo OutputFile { get; set; }
        public bool IgnoreCallerInfo { get; set; } = false; 

        public FileLogger(FileInfo outputFile)
        {
            OutputFile = outputFile;
        }
        public void Log(Status status, string message, DateTime timeStamp, string callerName, string sourceFile, int sourceLineNumber)
        {

            if (IgnoreCallerInfo)
            {
                File.AppendAllText(OutputFile.FullName, $"[{status.ToString()}] [{timeStamp.ToString("hh:mm:ss.fff")}] {message} \n");
            }
            else
            {
                File.AppendAllText(OutputFile.FullName, $"[{status.ToString()}] [{timeStamp.ToString("hh:mm:ss.fff")}] {message} ({callerName}, {sourceFile}, {sourceLineNumber}) \n");
            }
        }
    }
}
