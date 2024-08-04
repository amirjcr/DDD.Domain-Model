using Common.Domain.DomainEvents;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Common.Domain
{

    public abstract class AggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot
        where TId : struct
    {
        protected AggregateRoot(bool generateAutoId)
            : base(generateAutoId)
        {
            _isRoot = true;
        }

        #region Methods
        public void AddDomainEvent(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void RemoveEvent(IDomainEvent domainEvent)
            => _domainEvents.Remove(domainEvent);
        #endregion
    }


    public interface IAggregateRoot { }

}


