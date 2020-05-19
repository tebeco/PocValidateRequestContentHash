using System;

namespace ContentHashValidation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateContentHashAttribute : Attribute, IContentHashValidationMetadata { }
}
