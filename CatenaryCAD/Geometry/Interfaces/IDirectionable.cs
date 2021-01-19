namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для направляемых объектов в пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IVector"/>, представляющий тип координаты направление объекта в пространстве.</typeparam>
    public interface IDirectionable<T> where T: struct, IVector
    {
        /// <summary>
        /// Получает или задает координаты направление объекта.
        /// </summary>        
        /// <value>
        /// Координаты направления.
        /// </value>
        public T Direction { get; set; }
    }
}
