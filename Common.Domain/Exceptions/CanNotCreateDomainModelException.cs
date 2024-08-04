
using System;

namespace Common.Domain.Exceptions
{
    public sealed class CanNotCreateDomainModelException : BaseDomainException
    {
        public CanNotCreateDomainModelException(string message)
            : base(message)
        { }

        public CanNotCreateDomainModelException(Type domainType)
        : base($"the model typeof : {domainType.FullName} can not be created !")
        { }
    }


    public sealed class InvalidDomainDataException : BaseDomainException
    {
        public InvalidDomainDataException(string fieldName)
            : base($"the data passw with worng data Data Name : {fieldName}")
        {
        }
    }
}
