using System;

namespace Catenary.Models.Events
{
    [Serializable]
    public class EventOutput
    {
        public readonly object Value;
        public EventOutput(object value) => Value = value;
    }
}
