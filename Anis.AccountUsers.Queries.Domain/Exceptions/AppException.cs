﻿namespace Anis.AccountUsers.Queries.Domain.Exceptions
{
    public class AppException : Exception
    {
        public ExceptionStatusCode StatusCode { get; set; }

        public AppException(ExceptionStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}