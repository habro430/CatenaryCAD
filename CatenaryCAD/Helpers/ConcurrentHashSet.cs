// The contents of this file is taken from https://github.com/i3arnon/ConcurrentHashSet/blob/master/src/ConcurrentHashSet/ConcurrentHashSet.cs

//MIT License
//Copyright(c) 2019 Bar Arnon

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

//==============================================================================

//CoreFX (https://github.com/dotnet/corefx)
//The MIT License (MIT)
//Copyright (c) .NET Foundation and Contributors

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CatenaryCAD.Helpers
{
    /// <summary>
    /// Представляет класс, реализующий потокобезопасную уникальную хэш коллекцию.
    /// </summary>
    /// <typeparam name="T">Тип объектов в коллекции.</typeparam>
    /// <remarks>
    /// Все открытые члены <see cref = "ConcurrentHashSet {T}" /> являются потокобезопасными и могут использоваться 
    /// одновременно из нескольких потоков.
    /// </remarks>
    [Serializable, DebuggerDisplay("Count = {Count}")]
    public class ConcurrentHashSet<T> : IReadOnlyCollection<T>, ICollection<T>
    {
        private const int DefaultCapacity = 8;
        private const int MaxLockNumber = 1024;

        private readonly IEqualityComparer<T> _comparer;
        private readonly bool _growLockArray;

        private int _budget;
        private volatile Tables _tables;

        private static int DefaultConcurrencyLevel => PlatformHelper.ProcessorCount;

        /// <summary>
        /// Возвращает количество элементов, содержащихся в <see
        /// cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <value>Количество элементов, содержащихся в <see
        /// cref="ConcurrentHashSet{T}"/>.</value>
        public int Count
        {
            get
            {
                var count = 0;
                var acquiredLocks = 0;
                try
                {
                    AcquireAllLocks(ref acquiredLocks);

                    for (var i = 0; i < _tables.CountPerLock.Length; i++)
                    {
                        count += _tables.CountPerLock[i];
                    }
                }
                finally
                {
                    ReleaseLocks(0, acquiredLocks);
                }

                return count;
            }
        }

        /// <summary>
        /// Получает значение, указывающее, является ли<see cref = "ConcurrentHashSet {T}" /> пустым.
        /// </summary>
        /// <value><see langword="true"/> если в <see cref="ConcurrentHashSet{T}"/> отсутвуют объекты, в противном случае - <see langword="false"/>.</value>
        public bool IsEmpty
        {
            get
            {
                var acquiredLocks = 0;
                try
                {
                    AcquireAllLocks(ref acquiredLocks);

                    for (var i = 0; i < _tables.CountPerLock.Length; i++)
                    {
                        if (_tables.CountPerLock[i] != 0)
                        {
                            return false;
                        }
                    }
                }
                finally
                {
                    ReleaseLocks(0, acquiredLocks);
                }

                return true;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>, 
        /// имеющий уровень параллелизма по умолчанию, начальную емкость по умолчанию и
        /// использующий компаратор по умолчанию.
        /// </summary>
        public ConcurrentHashSet()
            : this(DefaultConcurrencyLevel, DefaultCapacity, true, null)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>,
        /// имеющий указанный уровень параллелизма, указанную начальную емкость и 
        /// использующий компаратор по умолчанию.
        /// </summary>
        /// <param name="concurrencyLevel">Предполагаемое количество потоков, которые могут обновить
        /// <see cref="ConcurrentHashSet{T}"/> одновременно.</param>
        /// <param name="capacity">Начальное количество элементов, которое может содержать <see
        /// cref="ConcurrentHashSet{T}"/>. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> меньше чем 1.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> меньше чем 0. </exception>
        public ConcurrentHashSet(int concurrencyLevel, int capacity)
            : this(concurrencyLevel, capacity, false, null)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>
        /// содержащий элементы, скопированные из указанного <see cref="T:System.Collections.IEnumerable{T}"/>,
        /// имеющий уровень параллелизма по умолчанию, начальную емкость по умолчанию и
        /// использующий компаратор по умолчанию.
        /// </summary>
        /// <param name="collection"><see cref="T:System.Collections.IEnumerable{T}"/>, 
        /// элементы которого копируются в новый экземпляр <see cref="ConcurrentHashSet{T}"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> имеет пустую ссылку.</exception>
        public ConcurrentHashSet(IEnumerable<T> collection)
            : this(collection, null)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>
        /// имеющий уровень параллелизма по умолчанию, начальную емкость по умолчанию и
        /// использующий указанный компаратор <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">Реализация <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/> 
        /// для использования при сравнении элементов.</param>
        public ConcurrentHashSet(IEqualityComparer<T> comparer)
            : this(DefaultConcurrencyLevel, DefaultCapacity, true, comparer)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>
        /// содержащий элементы, скопированные из указанного <see cref="T:System.Collections.IEnumerable"/>,
        /// имеющий уровень параллелизма по умолчанию, начальную емкость по умолчанию и
        /// использующий указанный компаратор <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="collection"><see cref="T:System.Collections.IEnumerable{T}"/>, 
        /// элементы которого копируются в новый экземпляр <see cref="ConcurrentHashSet{T}"/>.</param>
        /// <param name="comparer">Реализация <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/> 
        /// для использования при сравнении элементов.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> имеет пустую ссылку.
        /// </exception>
        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(comparer)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            InitializeFromCollection(collection);
        }


        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/> 
        /// содержащий элементы, скопированные из указанного <see cref="T:System.Collections.IEnumerable"/>, 
        /// имеющий указанный уровень параллелизма, начальную емкость по умолчанию и 
        /// использующий указанный компаратор <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="concurrencyLevel">Предполагаемое количество потоков, которые могут обновить <see cref="ConcurrentHashSet{T}"/> одновременно.</param>
        /// <param name="collection"><see cref="T:System.Collections.IEnumerable{T}"/>, 
        /// элементы которого копируются в новый экземпляр <see cref="ConcurrentHashSet{T}"/>.</param>
        /// <param name="comparer">Реализация <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/> 
        /// для использования при сравнении элементов.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> имеет пустую ссылку.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> меньше чем 1.</exception>
        public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(concurrencyLevel, DefaultCapacity, false, comparer)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            InitializeFromCollection(collection);
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="ConcurrentHashSet{T}"/>
        /// имеющий указанный уровень параллелизма, указанную начальную емкость и
        /// использующий указанный компаратор <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="concurrencyLevel">Предполагаемое количество потоков, которые могут обновить
        /// <see cref="ConcurrentHashSet{T}"/> одновременно.</param>
        /// <param name="capacity">Начальное количество элементов, которое может содержать <see
        /// cref="ConcurrentHashSet{T}"/>.</param>
        /// <param name="comparer">Реализация <see cref="T:System.Collections.Generic.IEqualityComparer{T}"/> 
        /// для использования при сравнении элементов.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> меньше чем 1.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> меньше чем 0. </exception>
        public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T> comparer)
            : this(concurrencyLevel, capacity, false, comparer)
        {
        }

        private ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T> comparer)
        {
            if (concurrencyLevel < 1) throw new ArgumentOutOfRangeException(nameof(concurrencyLevel));
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            // The capacity should be at least as large as the concurrency level. Otherwise, we would have locks that don't guard
            // any buckets.
            if (capacity < concurrencyLevel)
            {
                capacity = concurrencyLevel;
            }

            var locks = new object[concurrencyLevel];
            for (var i = 0; i < locks.Length; i++)
            {
                locks[i] = new object();
            }

            var countPerLock = new int[locks.Length];
            var buckets = new Node[capacity];
            _tables = new Tables(buckets, locks, countPerLock);

            _growLockArray = growLockArray;
            _budget = buckets.Length / locks.Length;
            _comparer = comparer ?? System.Collections.Generic.EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Добавляет указанный элемент в <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <param name="item">Добавляемый элемент.</param>
        /// <returns><see langword="true"/> если элемент был успешно добавлен в <see cref="ConcurrentHashSet{T}"/>,
        /// <see langword="false"/> если он был добавлен ранее и существует в коллекции.</returns>
        /// <exception cref="T:System.OverflowException"><see cref="ConcurrentHashSet{T}"/>
        /// содержит слишком много элементов.</exception>
        public bool Add(T item) =>
            AddInternal(item, _comparer.GetHashCode(item), true);

        /// <summary>
        /// Удаляет все элементы из <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        public void Clear()
        {
            var locksAcquired = 0;
            try
            {
                AcquireAllLocks(ref locksAcquired);

                var newTables = new Tables(new Node[DefaultCapacity], _tables.Locks, new int[_tables.CountPerLock.Length]);
                _tables = newTables;
                _budget = Math.Max(1, newTables.Buckets.Length / newTables.Locks.Length);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }

        /// <summary>
        /// Определяет, содержит ли <see cref="ConcurrentHashSet{T}"/> указанный элемент в коллекции.
        /// </summary>
        /// <param name="item">Проверяемый элемент <see cref="ConcurrentHashSet{T}"/>.</param>
        /// <returns><see langword="true"/> если элемент содержиться в коллекции <see cref="ConcurrentHashSet{T}"/>, 
        /// в противном случае - <see langword="false"/>.</returns>
        public bool Contains(T item)
        {
            var hashcode = _comparer.GetHashCode(item);

            // We must capture the _buckets field in a local variable. It is set to a new table on each table resize.
            var tables = _tables;

            var bucketNo = GetBucket(hashcode, tables.Buckets.Length);

            // We can get away w/out a lock here.
            // The Volatile.Read ensures that the load of the fields of 'n' doesn't move before the load from buckets[i].
            var current = Volatile.Read(ref tables.Buckets[bucketNo]);

            while (current != null)
            {
                if (hashcode == current.Hashcode && _comparer.Equals(current.Item, item))
                {
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Пытаеться удалить элемент из коллекции <see cref="ConcurrentHashSet{T}"/>.
        /// </summary>
        /// <param name="item">Удаляемый эдемент.</param>
        /// <returns><see langword="true"/> если элемент успешно удален из коллекции <see cref="ConcurrentHashSet{T}"/>, 
        /// в противном случае - <see langword="false"/>.</returns>
        public bool TryRemove(T item)
        {
            var hashcode = _comparer.GetHashCode(item);
            while (true)
            {
                var tables = _tables;

                GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

                lock (tables.Locks[lockNo])
                {
                    // If the table just got resized, we may not be holding the right lock, and must retry.
                    // This should be a rare occurrence.
                    if (tables != _tables)
                    {
                        continue;
                    }

                    Node previous = null;
                    for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
                    {
                        Debug.Assert((previous == null && current == tables.Buckets[bucketNo]) || previous.Next == current);

                        if (hashcode == current.Hashcode && _comparer.Equals(current.Item, item))
                        {
                            if (previous == null)
                            {
                                Volatile.Write(ref tables.Buckets[bucketNo], current.Next);
                            }
                            else
                            {
                                previous.Next = current.Next;
                            }

                            tables.CountPerLock[lockNo]--;
                            return true;
                        }
                        previous = current;
                    }
                }

                return false;
            }
        }

        /// <summary>Возвращает перечислитель, который выполняет итерацию по <see cref="ConcurrentHashSet{T}"/>.</summary>
        /// <returns>Перечислитель для <see cref="ConcurrentHashSet{T}"/>.</returns>
        /// <remarks>
        /// Перечислитель, возвращенный из коллекции, можно безопасно использовать одновременно с 
        /// чтением и записью в коллекцию, однако он не представляет собой моментальный снимок коллекции.
        /// Содержимое перечислителя, может содержать изменения внесеные в коллекцию после вызова 
        /// <see cref="GetEnumerator"/>.
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            var buckets = _tables.Buckets;

            for (var i = 0; i < buckets.Length; i++)
            {
                // The Volatile.Read ensures that the load of the fields of 'current' doesn't move before the load from buckets[i].
                var current = Volatile.Read(ref buckets[i]);

                while (current != null)
                {
                    yield return current.Item;
                    current = current.Next;
                }
            }
        }        
        
        /// <inheritdoc cref="GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="Add(T)"/>
        void ICollection<T>.Add(T item) => Add(item);

        /// <inheritdoc cref="TryRemove(T)"/>
        bool ICollection<T>.Remove(T item) => TryRemove(item);

        /// <summary>
        /// Получает значение, указывающее, является ли объект <see cref="ConcurrentHashSet{T}"/> доступным только для чтения.
        /// </summary>
        /// <returns> Значение всегда равно <see langword="false"/>.</returns>
        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Копирует элементы коллекции <see cref="ConcurrentHashSet{T}"/> в указанный массив,
        /// начиная с указанного индекса массива.
        /// </summary>
        /// <param name="array">Одномерный массив, в который копируются элементы из <see cref="ConcurrentHashSet{T}"/>.
        /// Массив должен иметь индексацию, начинающуюся с нуля.</param>
        /// <param name="arrayIndex">Отсчитываемый от нуля индекс в массиве <paramref name="array"/>, указывающий начало копирования.</param>
        /// <exception cref="ArgumentNullException">Свойство <paramref name="array"/> имеет значение <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Значение параметра <paramref name="arrayIndex"/> меньше 0.</exception>
        /// <exception cref="ArgumentException">Число элементов в исходной коллекции <see cref="ConcurrentHashSet{T}"/>
        /// больше доступного места от положения, заданного значением параметра <paramref name="arrayIndex"/>,
        /// до конца массива назначения <paramref name="array"/>.</exception>
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            var locksAcquired = 0;
            try
            {
                AcquireAllLocks(ref locksAcquired);

                var count = 0;

                for (var i = 0; i < _tables.Locks.Length && count >= 0; i++)
                {
                    count += _tables.CountPerLock[i];
                }

                if (array.Length - count < arrayIndex || count < 0) //"count" itself or "count + arrayIndex" can overflow
                {
                    throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space from index to the end of the destination array.");
                }

                CopyToItems(array, arrayIndex);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }


        private void InitializeFromCollection(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                AddInternal(item, _comparer.GetHashCode(item), false);
            }

            if (_budget == 0)
            {
                _budget = _tables.Buckets.Length / _tables.Locks.Length;
            }
        }

        private bool AddInternal(T item, int hashcode, bool acquireLock)
        {
            while (true)
            {
                var tables = _tables;

                GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

                var resizeDesired = false;
                var lockTaken = false;
                try
                {
                    if (acquireLock)
                        Monitor.Enter(tables.Locks[lockNo], ref lockTaken);

                    // If the table just got resized, we may not be holding the right lock, and must retry.
                    // This should be a rare occurrence.
                    if (tables != _tables)
                    {
                        continue;
                    }

                    // Try to find this item in the bucket
                    Node previous = null;
                    for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
                    {
                        Debug.Assert(previous == null && current == tables.Buckets[bucketNo] || previous.Next == current);
                        if (hashcode == current.Hashcode && _comparer.Equals(current.Item, item))
                        {
                            return false;
                        }
                        previous = current;
                    }

                    // The item was not found in the bucket. Insert the new item.
                    Volatile.Write(ref tables.Buckets[bucketNo], new Node(item, hashcode, tables.Buckets[bucketNo]));
                    checked
                    {
                        tables.CountPerLock[lockNo]++;
                    }

                    //
                    // If the number of elements guarded by this lock has exceeded the budget, resize the bucket table.
                    // It is also possible that GrowTable will increase the budget but won't resize the bucket table.
                    // That happens if the bucket table is found to be poorly utilized due to a bad hash function.
                    //
                    if (tables.CountPerLock[lockNo] > _budget)
                    {
                        resizeDesired = true;
                    }
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(tables.Locks[lockNo]);
                }

                //
                // The fact that we got here means that we just performed an insertion. If necessary, we will grow the table.
                //
                // Concurrency notes:
                // - Notice that we are not holding any locks at when calling GrowTable. This is necessary to prevent deadlocks.
                // - As a result, it is possible that GrowTable will be called unnecessarily. But, GrowTable will obtain lock 0
                //   and then verify that the table we passed to it as the argument is still the current table.
                //
                if (resizeDesired)
                {
                    GrowTable(tables);
                }

                return true;
            }
        }

        private static int GetBucket(int hashcode, int bucketCount)
        {
            var bucketNo = (hashcode & 0x7fffffff) % bucketCount;
            Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
            return bucketNo;
        }

        private static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
        {
            bucketNo = (hashcode & 0x7fffffff) % bucketCount;
            lockNo = bucketNo % lockCount;

            Debug.Assert(bucketNo >= 0 && bucketNo < bucketCount);
            Debug.Assert(lockNo >= 0 && lockNo < lockCount);
        }

        private void GrowTable(Tables tables)
        {
            const int maxArrayLength = 0X7FEFFFFF;
            var locksAcquired = 0;
            try
            {
                // The thread that first obtains _locks[0] will be the one doing the resize operation
                AcquireLocks(0, 1, ref locksAcquired);

                // Make sure nobody resized the table while we were waiting for lock 0:
                if (tables != _tables)
                {
                    // We assume that since the table reference is different, it was already resized (or the budget
                    // was adjusted). If we ever decide to do table shrinking, or replace the table for other reasons,
                    // we will have to revisit this logic.
                    return;
                }

                // Compute the (approx.) total size. Use an Int64 accumulation variable to avoid an overflow.
                long approxCount = 0;
                for (var i = 0; i < tables.CountPerLock.Length; i++)
                {
                    approxCount += tables.CountPerLock[i];
                }

                //
                // If the bucket array is too empty, double the budget instead of resizing the table
                //
                if (approxCount < tables.Buckets.Length / 4)
                {
                    _budget = 2 * _budget;
                    if (_budget < 0)
                    {
                        _budget = int.MaxValue;
                    }
                    return;
                }

                // Compute the new table size. We find the smallest integer larger than twice the previous table size, and not divisible by
                // 2,3,5 or 7. We can consider a different table-sizing policy in the future.
                var newLength = 0;
                var maximizeTableSize = false;
                try
                {
                    checked
                    {
                        // Double the size of the buckets table and add one, so that we have an odd integer.
                        newLength = tables.Buckets.Length * 2 + 1;

                        // Now, we only need to check odd integers, and find the first that is not divisible
                        // by 3, 5 or 7.
                        while (newLength % 3 == 0 || newLength % 5 == 0 || newLength % 7 == 0)
                        {
                            newLength += 2;
                        }

                        Debug.Assert(newLength % 2 != 0);

                        if (newLength > maxArrayLength)
                        {
                            maximizeTableSize = true;
                        }
                    }
                }
                catch (OverflowException)
                {
                    maximizeTableSize = true;
                }

                if (maximizeTableSize)
                {
                    newLength = maxArrayLength;

                    // We want to make sure that GrowTable will not be called again, since table is at the maximum size.
                    // To achieve that, we set the budget to int.MaxValue.
                    //
                    // (There is one special case that would allow GrowTable() to be called in the future: 
                    // calling Clear() on the ConcurrentHashSet will shrink the table and lower the budget.)
                    _budget = int.MaxValue;
                }

                // Now acquire all other locks for the table
                AcquireLocks(1, tables.Locks.Length, ref locksAcquired);

                var newLocks = tables.Locks;

                // Add more locks
                if (_growLockArray && tables.Locks.Length < MaxLockNumber)
                {
                    newLocks = new object[tables.Locks.Length * 2];
                    Array.Copy(tables.Locks, 0, newLocks, 0, tables.Locks.Length);
                    for (var i = tables.Locks.Length; i < newLocks.Length; i++)
                    {
                        newLocks[i] = new object();
                    }
                }

                var newBuckets = new Node[newLength];
                var newCountPerLock = new int[newLocks.Length];

                // Copy all data into a new table, creating new nodes for all elements
                for (var i = 0; i < tables.Buckets.Length; i++)
                {
                    var current = tables.Buckets[i];
                    while (current != null)
                    {
                        var next = current.Next;
                        GetBucketAndLockNo(current.Hashcode, out int newBucketNo, out int newLockNo, newBuckets.Length, newLocks.Length);

                        newBuckets[newBucketNo] = new Node(current.Item, current.Hashcode, newBuckets[newBucketNo]);

                        checked
                        {
                            newCountPerLock[newLockNo]++;
                        }

                        current = next;
                    }
                }

                // Adjust the budget
                _budget = Math.Max(1, newBuckets.Length / newLocks.Length);

                // Replace tables with the new versions
                _tables = new Tables(newBuckets, newLocks, newCountPerLock);
            }
            finally
            {
                // Release all locks that we took earlier
                ReleaseLocks(0, locksAcquired);
            }
        }

        private void AcquireAllLocks(ref int locksAcquired)
        {
            // First, acquire lock 0
            AcquireLocks(0, 1, ref locksAcquired);

            // Now that we have lock 0, the _locks array will not change (i.e., grow),
            // and so we can safely read _locks.Length.
            AcquireLocks(1, _tables.Locks.Length, ref locksAcquired);
            Debug.Assert(locksAcquired == _tables.Locks.Length);
        }

        private void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
        {
            Debug.Assert(fromInclusive <= toExclusive);
            var locks = _tables.Locks;

            for (var i = fromInclusive; i < toExclusive; i++)
            {
                var lockTaken = false;
                try
                {
                    Monitor.Enter(locks[i], ref lockTaken);
                }
                finally
                {
                    if (lockTaken)
                    {
                        locksAcquired++;
                    }
                }
            }
        }

        private void ReleaseLocks(int fromInclusive, int toExclusive)
        {
            Debug.Assert(fromInclusive <= toExclusive);

            for (var i = fromInclusive; i < toExclusive; i++)
            {
                Monitor.Exit(_tables.Locks[i]);
            }
        }

        private void CopyToItems(T[] array, int index)
        {
            var buckets = _tables.Buckets;
            for (var i = 0; i < buckets.Length; i++)
            {
                for (var current = buckets[i]; current != null; current = current.Next)
                {
                    array[index] = current.Item;
                    index++; //this should never flow, CopyToItems is only called when there's no overflow risk
                }
            }
        }

        internal static void ClearNull(ConcurrentHashSet<IIdentifier> sets)
        {
            var buckets = sets._tables.Buckets;
            for (int i = 0; i < buckets.Length; i++)
            {
                var current = Volatile.Read(ref buckets[i]);

                if (current == null) continue;
                if (current.Item?.GetGuid() == Guid.Empty) sets.TryRemoveByHashCode(current.Hashcode);

            }
        }
        internal bool TryRemoveByHashCode(int hashcode)
        {
            while (true)
            {
                var tables = _tables;

                GetBucketAndLockNo(hashcode, out int bucketNo, out int lockNo, tables.Buckets.Length, tables.Locks.Length);

                lock (tables.Locks[lockNo])
                {
                    // If the table just got resized, we may not be holding the right lock, and must retry.
                    // This should be a rare occurrence.
                    if (tables != _tables)
                    {
                        continue;
                    }

                    Node previous = null;
                    for (var current = tables.Buckets[bucketNo]; current != null; current = current.Next)
                    {
                        Debug.Assert((previous == null && current == tables.Buckets[bucketNo]) || previous.Next == current);

                        if (hashcode == current.Hashcode)
                        {
                            if (previous == null)
                            {
                                Volatile.Write(ref tables.Buckets[bucketNo], current.Next);
                            }
                            else
                            {
                                previous.Next = current.Next;
                            }

                            tables.CountPerLock[lockNo]--;
                            return true;
                        }
                        previous = current;
                    }
                }

                return false;
            }

        }

        [Serializable]
        private class Tables
        {
            public readonly Node[] Buckets;
            public readonly object[] Locks;

            public volatile int[] CountPerLock;

            public Tables(Node[] buckets, object[] locks, int[] countPerLock)
            {
                Buckets = buckets;
                Locks = locks;
                CountPerLock = countPerLock;
            }
        }

        [Serializable]
        private class Node
        {
            public readonly T Item;
            public readonly int Hashcode;

            public volatile Node Next;

            public Node(T item, int hashcode, Node next)
            {
                Item = item;
                Hashcode = hashcode;
                Next = next;
            }
        }
        private static class PlatformHelper
        {
            private const int ProcessorCountRefreshIntervalMs = 30000;

            private static volatile int _processorCount;
            private static volatile int _lastProcessorCountRefreshTicks;

            internal static int ProcessorCount
            {
                get
                {
                    var now = Environment.TickCount;
                    if (_processorCount == 0 || now - _lastProcessorCountRefreshTicks >= ProcessorCountRefreshIntervalMs)
                    {
                        _processorCount = Environment.ProcessorCount;
                        _lastProcessorCountRefreshTicks = now;
                    }

                    return _processorCount;
                }
            }
        }
    }
}