using CatenaryCAD.Geometry;
using System;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Класс, описывающий модель опоры контактной сети.
    /// </summary>
    [Serializable]
    public abstract class Mast : Model, IMast
    {
        /// <inheritdoc/>
        /// <summary>
        /// Типы допустимых моделей <see cref="IFoundation"/>, на которые может быть установлена данная модель <see cref="IMast"/>.
        /// При наличии в массиве абстрактного класса, наследуемого от <see cref="IFoundation"/>, то допустимыми 
        /// фундаментами являются все наследуемые от указанного абстрактного класса модели. 
        /// При наличии класса определенной модели, наследуемого от <see cref="IFoundation"/>, то допустимым фундаментом 
        /// являеться указанная модель и все наследуемые от указанного класса модели. 
        /// </summary>
        /// <remarks>По умолчанию возвращает все модели, наследуемые от <seealso cref="IFoundation"/>.</remarks>
        public virtual Type[] AllowableFoundations => new Type[] { typeof(IFoundation) };
    }
}
