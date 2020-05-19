using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ContentHashValidation
{
    public sealed class FixedLengthWithLockArrayPool<T> : ArrayPool<T>
    {
        private readonly Stack<T[]> _arrays = new Stack<T[]>();
        private SpinLock _lock = new SpinLock();
        private readonly int _size;

        public FixedLengthWithLockArrayPool(int size, int preAllocatedBuffer = 0)
        {
            Debug.Assert(size > 0);
            Debug.Assert(preAllocatedBuffer >= 0);

            _size = size;

            for (int i = 0; i < preAllocatedBuffer; i++)
            {
                _arrays.Push(new T[_size]);
            }
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
