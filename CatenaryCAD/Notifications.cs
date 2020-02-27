using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatenaryCAD
{
    [Serializable, Flags]
    public enum NotificationFlags
    {
        Erased = 1,
        Placed = 2,
        Updated = 4,
        Transformed = 8,
    }
}
