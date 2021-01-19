namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для транформируемых объектов в пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IMatrix"/> и представляющий тип матрицы для трансформирования объекта.</typeparam>
    /// <typeparam name="TResult">Входной параметр, представляющий тип возращаемого объекта после трансформирования.</typeparam>
    public interface ITransformable<T, TResult> where T : struct, IMatrix
    {
        /// <summary>
        /// Трансформирует данный экземпляр <typeparamref name="TResult"/>, умножая его на <paramref name = "matrix" />.
        /// </summary>
        /// <param name="matrix">Матрица для трансформирования.</param>
        /// <returns>Трансформированный экземпляр <typeparamref name="TResult"/>.</returns>
        public TResult TransformBy(in T matrix);
    }
}
