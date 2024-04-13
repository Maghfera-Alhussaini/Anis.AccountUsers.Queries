﻿using Anis.AccountUsers.Queries.Domain.Exceptions;
using Grpc.Core;

namespace Anis.AccountUsers.Queries.Grpc.Extensions
{
    public static class EnumExtensions
    {
        public static StatusCode ToRpcStatuseCode(this ExceptionStatusCode exceptionStatusCode)
            => exceptionStatusCode switch
            {
                ExceptionStatusCode.OK => StatusCode.OK,
                ExceptionStatusCode.Cancelled => StatusCode.Cancelled,
                ExceptionStatusCode.Unknown => StatusCode.Unknown,
                ExceptionStatusCode.InvalidArgument => StatusCode.InvalidArgument,
                ExceptionStatusCode.DeadlineExceeded => StatusCode.DeadlineExceeded,
                ExceptionStatusCode.NotFound => StatusCode.NotFound,
                ExceptionStatusCode.AlreadyExists => StatusCode.AlreadyExists,
                ExceptionStatusCode.PermissionDenied => StatusCode.PermissionDenied,
                ExceptionStatusCode.ResourceExhausted => StatusCode.ResourceExhausted,
                ExceptionStatusCode.FailedPrecondition => StatusCode.FailedPrecondition,
                ExceptionStatusCode.Aborted => StatusCode.Aborted,
                ExceptionStatusCode.OutOfRange => StatusCode.OutOfRange,
                ExceptionStatusCode.Unimplemented => StatusCode.Unimplemented,
                ExceptionStatusCode.Internal => StatusCode.Internal,
                ExceptionStatusCode.Unavailable => StatusCode.Unavailable,
                ExceptionStatusCode.DataLoss => StatusCode.DataLoss,
                ExceptionStatusCode.Unauthenticated => StatusCode.Unauthenticated,
                _ => throw new NotImplementedException("StatusCode not implemented ."),
            };
    }
}