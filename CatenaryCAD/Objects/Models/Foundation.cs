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
