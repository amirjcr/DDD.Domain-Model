
namespace Common.Domain.Exceptions
{
    public class EmailValidationException : BaseDomainException
    {
        public EmailValidationException(string message)
            : base(message) { }
    }
}
