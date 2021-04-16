using System;

namespace CatenaryCAD.Models.Events
{
    [Serializable]
    public class EventOutput
    {
        public readonly object Value;
        public EventOutput(object value) => Value = value;
    }
}
