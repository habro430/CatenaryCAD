using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace Catenary.Helpers
{
    /// <summary>
    /// Представляет класс, реализующий кэш, расположенный в оперативной памяти.
    /// </summary>
    /// <typeparam name="T">Тип элементов хранящихся в кэше.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class MemoryCache<T> where T : class
    {
        private static readonly double DefaultSlidingMinutes = 20;
        private static readonly double DefaultAbsoluteMinutes = 60;

        /// <summary>
        /// Время, по истечению которого, объект удаляется из данного экземпляра 
        /// <see cref="MemoryCache{T}"/>, если к нему за это время не было обращений.
        /// </summary>
        /// <value>Значение времени (в минутах)</value>
        public readonly double SlidingMinutes;

        /// <summary>
        /// Время, по истечению которого, объект удаляется из данного экземпляра 
        /// <see cref="MemoryCache{T}"/>, независимо от скользящего таймера..
        /// </summary>
        /// <value>Значение времени (в минутах)</value>
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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="sliding"/> меньше чем 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="absolute"/> меньше чем 0.</exception>
        /// <remarks>Если требуеться указать что записи в кэше не должны иметь скользящего и/или абсолютного срока дейсвия, 
        /// то аргументам <paramref name="sliding"/> и <paramref name="absolute"/> необходимо присвоить 0.</remarks>
        public MemoryCache(double sliding, double absolute)
        {
            if (sliding < 0) throw new ArgumentOutOfRangeException(nameof(sliding));
            if (absolute < 0) throw new ArgumentOutOfRangeException(nameof(absolute));

            SlidingMinutes = sliding;
            AbsoluteMinutes = absolute;

            policy = new CacheItemPolicy()
            {
                
                SlidingExpiration = sliding == 0 ? MemoryCache.NoSlidingExpiration : 
                                                   TimeSpan.FromMinutes(sliding),
                AbsoluteExpiration = absolute == 0 ? MemoryCache.InfiniteAbsoluteExpiration :
                                                     DateTimeOffset.Now.AddMinutes(absolute)
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
        /// Возвращает количество элементов, содержащихся в <see cref="MemoryCache{T}"/>.
        /// </summary>
        /// <value>Количество элементов, содержащихся в <see cref="MemoryCache{T}"/>.</value>
        public int Count => (int)cache.GetCount();

        /// <summary>
        /// Индексатор обеспечивающий доступ к элементам, содержащихся в <see cref="MemoryCache{T}"/>.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <returns>Объект <typeparamref name="T"/> представляющий значение записи из кэша, 
        /// если запись с таким ключем имееться в кэше. В противном случае возвращает <see langword="null"/>.</returns>
        /// <example>
        /// </example>
        public T this[string key]
        {
            get
            {
                if (cache.Contains(key))
                    return cache.Get(key) as T;
                else return null;
            }
            set
            {
                if (value == null)
                {
                    cache.Remove(key);
                }
                else
                {
                    cache.Set(key, value, policy);
                }
            }
        }

        /// <summary>
        ///  Проверяет, существует ли запись <paramref name="key"/> в кэше.
        /// </summary>
        /// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        /// <returns><see langword="true"/> если в <see cref="MemoryCache{T}"/> существует
        /// запись с уникальным идентификатором <paramref name="key"/>, в противном 
        /// случае - <see langword="false"/>.</returns>
        public bool Contains(string key) => cache.Contains(key);

        ///// <summary>
        ///// Возвращает объект из кэша используя уникальный идентификатор <paramref name="key"/>, 
        ///// в случаях когда объект отсутствует то, создается новый объект в кэше с уникальным 
        ///// идентификатором <paramref name="key"/>, и со значением возвращаемым <paramref name="create"/>.
        ///// </summary>
        ///// <param name="key">Ключ, уникальный идентификатор объекта в кэше.</param>
        ///// <param name="create">Делегат, инкапсулирующий метод создания объекта, 
        ///// в случаях если объект отсутствует в кэше.</param>
        ///// <returns>Объект <typeparamref name="T"/> представляющий значение записи из кэша.</returns>
        //public T GetOrCreate(string key, Func<T> create)
        //{
        //     this["sad"] ??= new T();
        //    if (!cache.Contains(key))
        //        cache.Set(key, create(), policy);

        //    return cache.Get(key) as T;
        //}

    }
}
