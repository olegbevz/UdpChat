namespace UdpChat.Server
{
    using System;
    using System.Diagnostics;
    using System.Security;

    public class EventLogging : ILogging
    {
        private string _eventSource;

        private string _entryLog;

        public EventLogging(string eventSource, string entryLog)
        {
            if (string.IsNullOrEmpty(eventSource))
            {
                throw new ArgumentNullException("eventSource");
            }

            if (string.IsNullOrEmpty(entryLog))
            {
                throw new ArgumentNullException("entryLog");
            }

            _eventSource = eventSource;

            _entryLog = entryLog;
        }

        public void WriteLog(string log)
        {
            try
            {
                if (!EventLog.SourceExists(_eventSource))
                {
                    EventLog.CreateEventSource(_eventSource, _entryLog);
                }

                EventLog.WriteEntry(_eventSource, log, EventLogEntryType.Information);
            }
            catch (SecurityException)
            {
            }
        }
    }
}