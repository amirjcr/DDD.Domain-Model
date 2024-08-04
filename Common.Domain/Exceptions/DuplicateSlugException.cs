

namespace Common.Domain.Exceptions
{
    public class DuplicateSlugException : BaseDomainException
    {
        public DuplicateSlugException(string message)
            : base(message)
        {
        }
    }
}
