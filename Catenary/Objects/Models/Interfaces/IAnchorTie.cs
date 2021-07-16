using System;

namespace Catenary.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели анкерной оттяжки.
    /// </summary>
    public interface IAnchorTie : IModel
    {
        /// <summary>
        /// Типы допустимых моделей <see cref="IMast"/>, на которые может быть установлена данная модель <see cref="IAnchorTie"/>.
        /// </summary>
        /// <value>Модели, наследуемые от <seealso cref="IMast"/>.</value>
        Type[] AllowableMasts{ get; }
    }
}
