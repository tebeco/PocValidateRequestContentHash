namespace MyWebApi.ContentHashValidation
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
