﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.Shared
{
    public class ClearDataEvent : PubSubEvent<string>
    {
    }

    public class DebugModeRunnedEvent : PubSubEvent<bool> 
    {
    }

    public class DiagnosticEvent : PubSubEvent<DiagnosticModel>
    {
    }

    public class DiagnosticModel 
    {
        public string CategoryName { get; set; }

        public string EventName { get; set; }

        public string ActivityId { get; set; }
    }
}
