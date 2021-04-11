using System;
using System.Collections.Generic;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели фундамента опоры контактной сети.
    /// </summary>
    public interface IFoundation : IModel
    {
        /// <summary>
        /// Допустимые объекты <see cref="IFoundation"/> которые можно установить для данной модели.
        /// </summary>
        Type[] AvailableFoundations { get; set; }
    }
}
