using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MyWebApi.ContentHashValidation
{
    public sealed class FixedLengthWithLockArrayPool<T> : ArrayPool<T>
    {
        private readonly Stack<T[]> _arrays = new Stack<T[]>();
        private SpinLock _lock = new SpinLock();
        private readonly int _size;

        public FixedLengthWithLockArrayPool(int size)
        {
            if (size < 0)
            {
                //ThrowHelper.ThrowArgumentOutOfRangeException(nameof(size), size);
            }

            _size = size;

        }

        private bool Enter()
        {
            var taken = false;
            _lock.Enter(ref taken);
            return taken;
        }

        private void Exit(bool taken)
        {
            if (taken)
            {
                _lock.Exit();
            }
        }

        public override T[] Rent(int minimumLength)
        {
            if (TryPop(out T[] value))
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

            Push(array);
        }

        private void Push(T[] value)
        {
            bool taken = Enter();
            _arrays.Push(value);
            Exit(taken);
        }

        private bool TryPop(out T[] value)
        {
            bool taken = Enter();
            bool b = _arrays.TryPop(out value!);
            Exit(taken);
            return b;
        }
    }
}
