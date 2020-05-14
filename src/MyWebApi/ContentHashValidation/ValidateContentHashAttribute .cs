using System;

namespace MyWebApi.ContentHashValidation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateContentHashAttribute : Attribute, IContentHashValidationMetadata { }
}
