namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для позиционируемых объектов пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IPoint"/> и представляющий тип координаты расположения объекта.</typeparam>
    public interface IPositionable<T> where T: struct, IPoint
    {
        /// <summary>
        /// Получает или задает координаты расположение объекта.
        /// </summary>
        /// <value>
        /// Координаты расположения.
        /// </value>
        public T Position { get; set; }
    }
}
