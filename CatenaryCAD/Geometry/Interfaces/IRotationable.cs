namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для вращаемых объектов в пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IVector"/>, представляющий тип координаты вращения объекта в пространстве.</typeparam>
    public interface IRotationable<T> where T: struct, IVector
    {
        /// <summary>
        /// Получает или задает координаты вращения объекта.
        /// </summary>        
        /// <value>
        /// Координаты вращения.
        /// </value>
        public T Rotation { get; set; }
    }
}
