using Catenary.Attributes;
using System;
using System.Linq;

namespace Catenary.Models
{
    /// <summary>
    /// Класс, описывающий модель фиксатора.
    /// </summary>
    [Serializable]
    public abstract class Bracket : Model, IBracket
    {
        /// <inheritdoc/>
        /// <summary>
        /// Типы допустимых моделей <see cref="IMast"/>, на которые может быть установлена данная модель <see cref="IBracket"/>.
        /// При наличии в массиве абстрактного класса, наследуемого от <see cref="IMast"/>, то допустимыми 
        /// моделями являются все наследуемые от указанного абстрактного класса модели. 
        /// При наличии класса определенной модели, наследуемого от <see cref="IMast"/>, то допустимой моделью 
        /// являеться указанная модель и все наследуемые от указанного класса модели. 
        /// </summary>
        /// <remarks>По умолчанию возвращает все модели, наследуемые от <seealso cref="IMast"/>.</remarks>
        public virtual Type[] AllowableMasts => new Type[] { typeof(IMast) };


        internal event Action<Type[]> AvailableBracketsUpdated;
        internal void SetAvailableBrackets(Type[] anchorties)
        {
            //получаем все прокэшированные модели производные от IAnchorTie
            var allties = Main.GetCatenaryObjects(typeof(IAnchorTie));

            var availablefoundations = anchorties
                .SelectMany((all) => allties
                    .Where((sub) => sub.IsSubclassOf(all))
                    .Union(anchorties)
                .Where((abs) => !abs.IsAbstract))//отсеиваем все абстрактые объекты
                .Where((non) => Attribute.GetCustomAttribute(non, typeof(ModelNonBrowsableAttribute), false) is null) //отсеиваем объекты которые помечены как недоступные
                .ToArray();

            AvailableBracketsUpdated?.Invoke(availablefoundations);
        }
    }
}
