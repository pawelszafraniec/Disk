﻿using System;
using System.Runtime.Serialization;

namespace Disk.Search.Query
{
    [Serializable]
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException()
        {
        }

        public InvalidQueryException(string? message) : base(message)
        {
        }

        public InvalidQueryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}