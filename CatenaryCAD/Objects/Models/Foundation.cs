using Catenary.Geometry;
using Catenary.Attributes;
using System;
using System.Linq;

namespace Catenary.Models
{
    /// <summary>
    /// Класс, описывающий модель фундамента опоры контактной сети.
    /// </summary>
    [Serializable]
    public abstract class Foundation : Model, IFoundation
    {
        /// <inheritdoc/>
        /// <summary>
        /// Типы допустимых моделей <see cref="IMast"/>, которые могут быть установлены на данную модель <see cref="IFoundation"/>.
        /// При наличии в массиве абстрактного класса, наследуемого от <see cref="IMast"/>, то допустимыми 
        /// опорами являются все наследуемые от указанного абстрактного класса модели. 
        /// При наличии класса определенной модели, наследуемого от <see cref="IMast"/>, то допустимой опорой 
        /// являеться указанная модель и все наследуемые от указанного класса модели. 
        /// </summary>
        /// <remarks>По умолчанию возвращает все модели, наследуемые от <seealso cref="IMast"/>.</remarks>
        public virtual Type[] AllowableMasts => new Type[] { typeof(IMast) };

        /// <inheritdoc/>
        public virtual Point3D GetDockingPointForMast() => Point3D.Origin;


        internal event Action<Type[]> AvailableFoundationsUpdated;
        internal void SetAvailableFoundations(Type[] foundations)
        {
            //получаем все прокэшированные модели производные от IFoundation
            var allfoundations = Main.GetCatenaryObjects(typeof(IFoundation));

            var availablefoundations = foundations
                .SelectMany((all) => allfoundations
                    .Where((sub) => sub.IsSubclassOf(all))
                    .Union(foundations)
                .Where((abs) => !abs.IsAbstract))//отсеиваем все абстрактые объекты
                .Where((non) => Attribute.GetCustomAttribute(non, typeof(ModelNonBrowsableAttribute), false) is null) //отсеиваем объекты которые помечены как недоступные
                .ToArray();

            AvailableFoundationsUpdated?.Invoke(availablefoundations);
        }
    }
}
