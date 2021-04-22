using CatenaryCAD.Geometry;
using CatenaryCAD.Geometry.Meshes;
using CatenaryCAD.Geometry.Shapes;
using System;

namespace CatenaryCAD.Models
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
