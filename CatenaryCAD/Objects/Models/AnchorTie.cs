using CatenaryCAD.Attributes;
using System;
using System.Linq;

namespace CatenaryCAD.Models
{
    /// <summary>
    /// Класс, описывающий модель анкерной оттяжки.
    /// </summary>
    [Serializable]
    public abstract class AnchorTie : Model, IAnchorTie
    {
        internal event Action<Type[]> AvailableAnchorTiesUpdated;
        internal void SetAvailableAnchorTies(Type[] anchorties)
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

            AvailableAnchorTiesUpdated?.Invoke(availablefoundations);
        }
    }
}
