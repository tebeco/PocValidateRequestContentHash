namespace ContentHashValidation
{
    public enum PoolKind
    {
        SharedArrayPool,
        DedicatedArrayPool,
        FixedLengthLockFree,
        FixedLengthWithLock,
        PreAllocatedFixedLengthLockFree,
        PreAllocatedFixedLengthWithLock
    }
}
