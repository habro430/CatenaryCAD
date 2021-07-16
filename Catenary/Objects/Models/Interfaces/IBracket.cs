using System;

namespace Catenary.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели фиксатора.
    /// </summary>
    public interface IBracket : IModel
    {
        /// <summary>
        /// Типы допустимых моделей <see cref="IMast"/>, на которые может быть установлена данная модель <see cref="IBracket"/>.
        /// </summary>
        /// <value>Модели, наследуемые от <seealso cref="IMast"/>.</value>
        Type[] AllowableMasts { get; }
    }
}
