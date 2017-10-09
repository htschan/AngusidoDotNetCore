using System;

namespace Server.Helpers
{
    public class RepositoryException : Exception
    {
        public int StatusCode { get; set; }

        public RepositoryException() { }
        public RepositoryException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
        public RepositoryException(int statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}