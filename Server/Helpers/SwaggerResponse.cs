
using System.Collections.Generic;

namespace Server.Helpers
{
    public class SwaggerResponse
    {
        public int StatusCode { get; }

        public Dictionary<string, IEnumerable<string>> Headers { get; }

        public SwaggerResponse(int statusCode, Dictionary<string, IEnumerable<string>> headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
    }

    public class SwaggerResponse<TResult> : SwaggerResponse
    {
        public TResult Result { get; }
        public string Message { get; }

        public SwaggerResponse(int statusCode, Dictionary<string, IEnumerable<string>> headers, TResult result)
            : this(statusCode, headers, result, "")
        {
        }

        public SwaggerResponse(int statusCode, Dictionary<string, IEnumerable<string>> headers, TResult result, string message)
            : base(statusCode, headers)
        {
            Result = result;
            Message = message;
        }
    }
}