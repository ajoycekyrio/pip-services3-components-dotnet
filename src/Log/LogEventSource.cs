﻿using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace PipServices3.Components.Log
{
    [EventSource(Name = "log")]
    public class LogEventSource : EventSource
    {
        public static readonly LogEventSource Current = new LogEventSource();

        static LogEventSource()
        {
            // A workaround for the problem where ETW activities do not get tracked until Tasks infrastructure is initialized.
            // This problem will be fixed in .NET Framework 4.6.2.
            Task.Run(() => { });
        }

        // Instance constructor is private to enforce singleton semantics
        private LogEventSource() : base() { }

        public class Keywords
        {
            public const EventKeywords Fatal = (EventKeywords)0x01;
            public const EventKeywords Error = (EventKeywords)0x02;
            public const EventKeywords Warning = (EventKeywords)0x04;
            public const EventKeywords Informational = (EventKeywords)0x08;
            public const EventKeywords Debug = (EventKeywords)0x10;
            public const EventKeywords Trace = (EventKeywords)0x20;
        }

        // For very high-frequency events it might be advantageous to raise events using WriteEventCore API.
        // This results in more efficient parameter handling, but requires explicit allocation of EventData structure and unsafe code.
        // To enable this code path, define UNSAFE conditional compilation symbol and turn on unsafe code support in project properties.
        private
#if UNSAFE
        unsafe
#endif
        void PerformWriteEvent(int eventId, string correlationId, string message)
        {
#if !UNSAFE
            WriteEvent(eventId, correlationId, message);
#else
            const int numArgs = 2;
            fixed (pCorrelationId = correlationId, pMessage = message)
            {
                EventData* eventData = stackalloc EventData[numArgs];
                eventData[0] = new EventData { DataPointer = (IntPtr) pCorrelationId, Size = SizeInBytes(correlationId) };
                eventData[1] = new EventData { DataPointer = (IntPtr) pMessage, Size = SizeInBytes(message) };

                WriteEventCore(eventId, numArgs, eventData);
            }
#endif
        }

        private const int FatalEventId = 1000;
        [Event(FatalEventId, Message = "{0} : {1}", Level = EventLevel.Critical, Keywords = Keywords.Fatal)]
        public void Fatal(string correlationId, string message)
        {
            PerformWriteEvent(FatalEventId, correlationId ?? "---", message ?? "");
        }

        private const int ErrorEventId = 1001;
        [Event(ErrorEventId, Message = "{0} : {1}", Level = EventLevel.Error, Keywords = Keywords.Error)]
        public void Error(string correlationId, string message)
        {
            PerformWriteEvent(ErrorEventId, correlationId ?? "---", message ?? "");
        }

        private const int WarnEventId = 1002;
        [Event(WarnEventId, Message = "{0} : {1}", Level = EventLevel.Warning, Keywords = Keywords.Warning)]
        public void Warn(string correlationId, string message)
        {
            PerformWriteEvent(WarnEventId, correlationId ?? "---", message ?? "");
        }

        private const int InfoEventId = 1003;
        [Event(InfoEventId, Message = "{0} : {1}", Level = EventLevel.Informational, Keywords = Keywords.Informational)]
        public void Info(string correlationId, string message)
        {
            PerformWriteEvent(InfoEventId, correlationId ?? "---", message ?? "");
        }

        private const int DebugEventId = 1004;
        [Event(DebugEventId, Message = "{0} : {1}", Level = EventLevel.Verbose, Keywords = Keywords.Debug)]
        public void Debug(string correlationId, string message)
        {
            PerformWriteEvent(DebugEventId, correlationId ?? "---", message ?? "");
        }

        private const int TraceEventId = 1005;
        [Event(TraceEventId, Message = "{0} : {1}", Level = EventLevel.Verbose, Keywords = Keywords.Trace)]
        public void Trace(string correlationId, string message)
        {
            PerformWriteEvent(TraceEventId, correlationId ?? "---", message ?? "");
        }

    }
}
