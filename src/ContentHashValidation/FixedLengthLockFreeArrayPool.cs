using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MyWebApi.ContentHashValidation
{
    public sealed class FixedLengthLockFreeArrayPool<T> : ArrayPool<T>
    {
        private readonly ConcurrentStack<T[]> _arrays = new ConcurrentStack<T[]>();
        private readonly int _size;

        public FixedLengthLockFreeArrayPool(int size)
        {
            if (size < 0)
            {
                //ThrowHelper.ThrowArgumentOutOfRangeException(nameof(size), size);
            }

            _size = size;
        }

        public override T[] Rent(int minimumLength)
        {
            if (_arrays.TryPop(out T[] value))
            {
                return value;
            }
            return new T[_size];
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            Debug.Assert(array.Length == _size);

            if (clearArray)
            {
                array.AsSpan().Clear();
            }

            _arrays.Push(array);
        }
    }
}
