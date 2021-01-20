namespace CatenaryCAD.Geometry.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий контракты для транформируемых объектов в пространстве.
    /// </summary>
    /// <typeparam name="T">Входной параметр, унаследованный от <see cref="IMatrix"/> и представляющий тип матрицы для преобразования объекта.</typeparam>
    /// <typeparam name="TResult">Входной параметр, представляющий тип возращаемого объекта после трансформирования.</typeparam>
    public interface ITransformable<T, TResult> where T : struct, IMatrix
    {
        /// <summary>
        /// Преобразовывает объект с помощью заданной матрицы <paramref name="matrix"/> и возвращает результат.
        /// </summary>
        /// <param name="matrix">Матрица для преобразования объекта.</param>
        /// <returns>Результат преобразования.</returns>
        public TResult TransformBy(in T matrix);
    }
}
