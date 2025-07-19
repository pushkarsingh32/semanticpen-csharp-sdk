using System;

namespace SemanticPen.SDK.Exceptions
{
    public class SemanticPenException : Exception
    {
        public string ErrorCode { get; }
        public int? HttpStatusCode { get; }

        public SemanticPenException(string message) : base(message)
        {
        }

        public SemanticPenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SemanticPenException(string message, string errorCode, int? httpStatusCode = null) : base(message)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static SemanticPenException NetworkError(string message, Exception innerException = null)
        {
            return new SemanticPenException($"Network error: {message}", innerException)
            {
                ErrorCode = "NETWORK_ERROR"
            };
        }

        public static SemanticPenException ValidationError(string message)
        {
            return new SemanticPenException($"Validation error: {message}")
            {
                ErrorCode = "VALIDATION_ERROR"
            };
        }

        public static SemanticPenException ApiError(string message, int statusCode)
        {
            return new SemanticPenException($"API error: {message}")
            {
                ErrorCode = "API_ERROR",
                HttpStatusCode = statusCode
            };
        }

        public static SemanticPenException AuthenticationError(string message)
        {
            return new SemanticPenException($"Authentication error: {message}")
            {
                ErrorCode = "AUTHENTICATION_ERROR",
                HttpStatusCode = 401
            };
        }

        public static SemanticPenException NotFoundError(string message)
        {
            return new SemanticPenException($"Not found: {message}")
            {
                ErrorCode = "NOT_FOUND_ERROR",
                HttpStatusCode = 404
            };
        }
    }
}