using System;
using MvvmCross.Logging;

namespace EnterpriseTracker.Core.Utility
{
    public class CustomLogger : IMvxLog
    {
        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            Console.WriteLine(logLevel.ToString() + " : " + messageFunc);
            if(exception != null)
            {
                Console.WriteLine("Exception message : " + exception.Message);
                Console.WriteLine("Exception stacktrace : " + exception.StackTrace);
            }
            return true;
        }

        public bool IsLogLevelEnabled(MvxLogLevel logLevel)
        {
            return true;
        }
    }
}