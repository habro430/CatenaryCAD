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
        /// <value>По умолчанию возвращает все модели, наследуемые от <seealso cref="IFoundation"/>.</value>
        Dictionary<string, Type> AllowableModels { get; set; }
    }
}
