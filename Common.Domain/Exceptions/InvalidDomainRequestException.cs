
namespace Common.Domain.Exceptions
{
    public sealed class InvalidDomainRequestException : BaseDomainException
    {
        public InvalidDomainRequestException(string message) : base(message)
        {
        }
    }
}
