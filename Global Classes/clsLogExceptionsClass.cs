using System.Diagnostics;

namespace ZakaBank_24.Global_Classes
{
    public class clsLogExceptionsClass
    {
        private static string _SourceName = "ZakaBank";

        /// <summary>
        ///   This Method For Loging Try Catch Exception From Data Access For This Project
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="type"></param>
        public static void LogExseptionsToLogerViewr(string Message, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(_SourceName))
            {
                EventLog.CreateEventSource(_SourceName, "Application");
            }


            EventLog.WriteEntry(_SourceName, Message, type);
        }
    }
}
