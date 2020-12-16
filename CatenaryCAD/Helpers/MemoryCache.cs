using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace CatenaryCAD.Helpers
{
    /// <summary>
    /// Представляет класс, реализующий кэш, расположенный в оперативной памяти.
    /// </summary>
    /// <typeparam name="T">Тип элементов хранящихся в кэше.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class MemoryCache<T> where T : class
    {
        /// <summary>
        /// Значение времени по умолчанию (в минутах), по истечению которого, объект удаляется из кэша, 
        /// если к нему за это вермя не было обращений.
        /// </summary>
        public const double DefaultSlidingMinutes = 20;

        /// <summary>
        /// Значение времени по умолчанию  (в минутах), по истечению которого, объект удаляется из кэша, 
        /// независимо от скользящего таймера.
        /// </summary>
        public const double DefaultAbsoluteMinutes = 60;

        public readonly double SlidingMinutes;
        public readonly double AbsoluteMinutes;

        private ObjectCache cache;
        private CacheItemPolicy policy;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MemoryCache{T}"/>.
        /// </summary>
        /// <param name="sliding"> Значение времени (в минутах), по истечению которого, объект удаляется из кэша, 
        /// если к нему за это вермя не было обращений.</param>
        /// <param name="absolute">Значение времени (в минутах), по истечению которого, объект удаляется из кэша, 
        /// независимо от <paramref name="sliding"/>.</param>
        public MemoryCache(double sliding, double absolute)
        {
            SlidingMinutes = sliding;
            AbsoluteMinutes = absolute;

            policy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromMinutes(sliding),
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(absolute)
            };

            cache = new MemoryCache(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MemoryCache{T}"/>, с параметрами по умолчанию.
        /// </summary>
        public MemoryCache() : 
            this(DefaultSlidingMinutes, DefaultAbsoluteMinutes)
        {
        }

        /// <summary>
        /// Возвращает объект из кэша используя уникальный идентификатор <paramref name="key"/>, 
        /// в случаях когда объект отсутствует то, создается новый объект в кэше, сам объект
        /// инициализируется через вызов метода <paramref name="create"/>.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <param name="create">Делегат, инкапсулирующий метод создания объекта, 
        /// в случаях если объект отсутствует в кэше.</param>
        /// <returns>Объект <typeparamref name="T"/> представляющий значение записи из кэша.</returns>
        public T GetOrCreate(string key, Func<T> create)
        {
            if (!cache.Contains(key))
                cache.Set(key, create(), policy);

            return cache.Get(key) as T;
        }

        /// <summary>
        /// Возвращает количество элементов, содержащихся в <see
        /// cref="MemoryCache{T}"/>.
        /// </summary>
        /// <value>Количество элементов, содержащихся в <see
        /// cref="MemoryCache{T}"/>.</value>
        /// 
        public int Count => (int)cache.GetCount();

        /// <summary>
        ///  Проверяет, существует ли запись <paramref name="key"/> в кэше.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <returns><see langword="true"/> если в <see cref="MemoryCache{T}"/> существует
        /// запись с уникальным идентификатором <paramref name="key"/>, в противном 
        /// случае - <see langword="false"/>.</returns>
        public bool Contains(string key) => cache.Contains(key);

        /// <summary>
        /// Получает объект <typeparamref name="T"/> с уникальным идентификатором
        /// <paramref name="key"/> из кэша.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <returns>Объект <typeparamref name="T"/> представляющий значение записи из кэша.</returns>
        public T Get(string key) => cache.Get(key) as T;

        /// <summary>
        /// Удаляет объект <typeparamref name="T"/> с уникальным идентификатором
        /// <paramref name="key"/> из кэша.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <returns><see langword="true"/> если запись с уникальным идентификатором 
        /// <paramref name="key"/> удалена из <see cref="MemoryCache{T}"/>, в противном 
        /// случае - <see langword="false"/>.</returns>
        public bool Remove(string key) => cache.Remove(key) != null ? true : false;

        /// <summary>
        /// Добавляет объект <typeparamref name="T"/> с уникальным идентификатором
        /// <paramref name="key"/> и значением <paramref name="value"/> в кэш.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <param name="value">Объект <typeparamref name="T"/> для добавления в кэш.</param>
        /// <returns><see langword="true"/> если запись с уникальным идентификатором 
        /// <paramref name="key"/> и значением <paramref name="value"/> добавлена в
        /// <see cref="MemoryCache{T}"/>, в противном случае - <see langword="false"/>.</returns>
        public bool Add(string key, T value) => cache.Add(key, value, policy);
        
    }
}
