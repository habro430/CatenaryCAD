using Catenary.Geometry;

namespace Catenary.Models
{
    /// <summary>
    /// Интерфейс, описывающий контракты для модели фундамента опоры контактной сети.
    /// </summary>
    public interface IFoundation : IModel
    {
        /// <summary>
        /// Возвращает точку присоединения <see cref="Point3D"/> опоры <see cref="IMast"/> к фундаменту <see cref="IFoundation"/> .
        /// </summary>
        /// <returns>Точка присодинения <see cref="Point3D"/> опоры <see cref="IMast"/> к фундаменту <see cref="IFoundation"/>.</returns>
        Point3D GetDockingPointForMast();
    }
}
