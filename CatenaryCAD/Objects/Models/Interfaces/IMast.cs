using Catenary.Geometry;
using Catenary.Geometry.Meshes;
using Catenary.Geometry.Shapes;
using System;

namespace Catenary.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели опоры контактной сети.
    /// </summary>
    public interface IMast : IModel
    {
        /// <summary>
        /// Типы допустимых моделей <see cref="IFoundation"/>, на которые может быть установлена данная модель <see cref="IMast"/>.
        /// </summary>
        /// <value>Модели, наследуемые от <seealso cref="IFoundation"/>.</value>
        Type[] AllowableFoundations { get; }
    }
}
