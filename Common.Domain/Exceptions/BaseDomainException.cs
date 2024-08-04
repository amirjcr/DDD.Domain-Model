
using System;

namespace Common.Domain.Exceptions
{
    public abstract class BaseDomainException : Exception
    {
        protected BaseDomainException(string message)
            : base(message)
        {

        }

        protected BaseDomainException(string message, BaseDomainException innerException)
            : base(message, innerException)
        { }
    }
}
