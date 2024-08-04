

namespace Common.Domain.Exceptions
{
    public class InvalidDomainArgumentException : BaseDomainException
    {
        public InvalidDomainArgumentException(string message) : base(message)
        {
        }
    }
}
