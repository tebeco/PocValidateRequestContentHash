using System;

namespace InOutLogging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class InOutLoggingBaseAttribute : Attribute, IInOutLoggingMetadata
    {
        public virtual bool IgnoreContent { get; set; } = false;

        public virtual bool IsExcluded { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NoContentInOutLoggingAttribute : InOutLoggingBaseAttribute, IInOutLoggingMetadata
    {
        public override bool IgnoreContent { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreInOutLoggingAttribute : InOutLoggingBaseAttribute, IInOutLoggingMetadata
    {
        public override bool IsExcluded { get; set; } = true;
    }
}
