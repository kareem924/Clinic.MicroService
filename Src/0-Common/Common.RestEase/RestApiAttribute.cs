using System;

namespace Common.RestEase
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class RestApiAttribute : Attribute
    {
        public string BaseUrl { get; private set; }

        public RestApiAttribute(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}
