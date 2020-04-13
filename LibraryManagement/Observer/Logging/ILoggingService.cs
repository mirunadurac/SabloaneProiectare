using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Observer.Logger
{
    public interface ILoggingService
    {
        void Log(Status status, string message, DateTime timeStamp, string callerName, string sourceFile, int sourceLineNumber);
    }
}
