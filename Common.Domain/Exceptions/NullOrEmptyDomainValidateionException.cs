
using System.Text;

namespace Common.Domain.Exceptions
{
    public sealed class NullOrEmptyDomainValidateionException : BaseDomainException
    {
        public NullOrEmptyDomainValidateionException(string nameOfField)
            : base($"{nameOfField} پارامتر اراسالی نال یا خالی از مقدار است") { }



        public NullOrEmptyDomainValidateionException(string[] nameOfFields)
            : base(CreateCustomMessage(nameOfFields))
        {
        }

        private static string CreateCustomMessage(string[] nameOfFields)
        {
            StringBuilder message = new StringBuilder();

            foreach (var field in nameOfFields)
                message.AppendLine($"{field} پارامتر ارسالی خالی یا نال می باشد");

            return message.ToString();
        }


        
    }
}
