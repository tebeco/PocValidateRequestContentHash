using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ContentHashValidation
{
    public sealed class FixedLengthLockFreeArrayPool<T> : ArrayPool<T>
        where T: notnull
    {
        private readonly ConcurrentStack<T[]> _arrays = new ConcurrentStack<T[]>();
        private readonly int _size;

        public FixedLengthLockFreeArrayPool(int size, int preAllocatedBuffer = 0)
        {
            Debug.Assert(size > 0);
            Debug.Assert(preAllocatedBuffer >= 0);

            _size = size;

            for (int i = 0; i < preAllocatedBuffer; i++)
            {
                _arrays.Push(new T[_size]);
            }
        }

        public override T[] Rent(int minimumLength)
        {
            if (_arrays.TryPop(out var value))
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
