using Catenary.Geometry;
using System;

namespace Catenary.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели фундамента опоры контактной сети.
    /// </summary>
    public interface IFoundation : IModel
    {
        /// <summary>
        /// Типы допустимых моделей <see cref="IMast"/>, которые могут быть установлены на данную модель <see cref="IFoundation"/>.
        /// </summary>
        /// <value>Модели, наследуемые от <seealso cref="IMast"/>.</value>
        Type[] AllowableMasts { get; }
    }
}
