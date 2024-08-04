

namespace Common.Domain.Exceptions
{
    public class DomainEntityNotFoundException : BaseDomainException
    {
        public DomainEntityNotFoundException(string message) : base(message)
        {
        }
    }
}
