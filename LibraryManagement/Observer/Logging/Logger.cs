using LibraryManagement.Observer.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LibraryManagement.Observer.Logging
{
    public class Logger
    {
        #region Singleton Related
        private static Logger instance;
        private static object padLock = new object();
        public static Logger Instance
        {
            get
            {
                if(instance == null)
                {
                    lock(padLock)
                    {
                        if(instance == null)
                        {
                            instance = new Logger();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion


        private IList<ILoggingService> observers;

        public void AddObservers(IList<ILoggingService> observers)
        {
            this.observers = observers;
        }

        public void Log(Status status, string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath]   string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            lock(padLock)
            {
                foreach(var observer in observers)
                {
                    observer.Log(status, message, DateTime.Now, memberName, sourceFilePath, sourceLineNumber);
                }
            }
        }

        private Logger()
        {
            observers = new List<ILoggingService>();
        }

        
    }
}
