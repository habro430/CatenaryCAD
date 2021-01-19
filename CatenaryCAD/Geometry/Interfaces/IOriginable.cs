namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для объектов имеющих исходную точку в пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IPoint"/> и представляющий тип исходной точки в пространств.</typeparam>
    public interface IOriginable<T> where T: struct, IPoint
    {
        /// <summary>
        /// Получает или задает координаты исходной точки объекта.
        /// </summary>        
        /// <value>
        /// Координаты исходной точки.
        /// </value>
        public T Origin { get; set; }
    }
}
