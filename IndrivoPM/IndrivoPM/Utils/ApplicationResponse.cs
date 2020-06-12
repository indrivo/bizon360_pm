using System;

namespace Bizon360.Utils
{
    public class ApplicationResponse<T>
    {
        public T Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal ApplicationResponse(T result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }
    }

    public class ApplicationResponse : ApplicationResponse<string>
    {
        protected ApplicationResponse(string errorMessage)
            : base(null, errorMessage)
        {
        }

        public static ApplicationResponse<T> Ok<T>(T result)
        {
            return new ApplicationResponse<T>(result, null);
        }

        public static ApplicationResponse Ok()
        {
            return new ApplicationResponse(null);
        }

        public static ApplicationResponse Error(string errorMessage)
        {
            return new ApplicationResponse(errorMessage);
        }
    }
}
