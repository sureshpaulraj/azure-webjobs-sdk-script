﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Azure.WebJobs.Script.WebHost.Diagnostics
{
    internal class SystemEventGenerator : ISystemEventGenerator
    {
        public void LogEvent(TraceLevel level, string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary, Exception exception = null)
        {
            switch (level)
            {
                case TraceLevel.Verbose:
                    SystemEvents.Log.RaiseFunctionsEventVerbose(subscriptionId, appName, functionName, eventName, source, details, summary);
                    break;
                case TraceLevel.Info:
                    SystemEvents.Log.RaiseFunctionsEventInfo(subscriptionId, appName, functionName, eventName, source, details, summary);
                    break;
                case TraceLevel.Warning:
                    SystemEvents.Log.RaiseFunctionsEventWarning(subscriptionId, appName, functionName, eventName, source, details, summary);
                    break;
                case TraceLevel.Error:
                    if (string.IsNullOrEmpty(details) && exception != null)
                    {
                        details = exception.ToString();
                    }
                    SystemEvents.Log.RaiseFunctionsEventError(subscriptionId, appName, functionName, eventName, source, details, summary);
                    break;
            }
        }

        public void LogMetric(string subscriptionId, string appName, string eventName, long average, long minimum, long maximum, long count)
        {
            SystemEvents.Log.RaiseFunctionsMetrics(subscriptionId, appName, eventName, average, minimum, maximum, count);
        }

        [EventSource(Guid = "08D0D743-5C24-43F9-9723-98277CEA5F9B")]
        public sealed class SystemEvents : EventSource
        {
            internal static readonly SystemEvents Log = new SystemEvents();

            [Event(65520, Level = EventLevel.Verbose, Channel = EventChannel.Operational)]
            public void RaiseFunctionsEventVerbose(string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary)
            {
                if (IsEnabled())
                {
                    WriteEvent(65520, subscriptionId, appName, functionName, eventName, source, details, summary);
                }
            }
            [Event(65521, Level = EventLevel.Informational, Channel = EventChannel.Operational)]
            public void RaiseFunctionsEventInfo(string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary)
            {
                if (IsEnabled())
                {
                    WriteEvent(65521, subscriptionId, appName, functionName, eventName, source, details, summary);
                }
            }
            [Event(65522, Level = EventLevel.Warning, Channel = EventChannel.Operational)]
            public void RaiseFunctionsEventWarning(string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary)
            {
                if (IsEnabled())
                {
                    WriteEvent(65522, subscriptionId, appName, functionName, eventName, source, details, summary);
                }
            }
            [Event(65523, Level = EventLevel.Error, Channel = EventChannel.Operational)]
            public void RaiseFunctionsEventError(string subscriptionId, string appName, string functionName, string eventName, string source, string details, string summary)
            {
                if (IsEnabled())
                {
                    WriteEvent(65523, subscriptionId, appName, functionName, eventName, source, details, summary);
                }
            }
            [Event(65524, Level = EventLevel.Informational, Channel = EventChannel.Operational)]
            public void RaiseFunctionsMetrics(string subscriptionId, string appName, string eventName, long average, long minimum, long maximum, long count)
            {
                if (IsEnabled())
                {
                    WriteEvent(65524, subscriptionId, appName, eventName, average, minimum, maximum, count);
                }
            }
        }
    }
}
