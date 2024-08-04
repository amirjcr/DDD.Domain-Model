using Common.Domain.DomainEvents;
using Common.Domain.Enums;
using Common.Domain.Exceptions;
using Common.Domain.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common.Domain
{
    public abstract class BaseEntity<TId> : IBaseEntity
    {

        #region Private Feilds
        private bool _isChanged = false;
        protected bool _isRoot = false;

        protected List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        private Dictionary<string, object?> _previousStage = new Dictionary<string, object?>(); // act as previous stage for this entity
        private Dictionary<string, object?> _backfeilds = new Dictionary<string, object?>(); // act as back feilds for properties 
        #endregion


        #region Constractors
        protected BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
        protected BaseEntity(TId id)
            : this()
        {
            Id = id;
        }

        protected BaseEntity(bool generateAutoId)
            : this()
        {
            if (generateAutoId)
                Id = Generator<TId>.GenerateId();

            CreationDate = DateTime.Now;

        }

        #endregion

        #region Props
        public DateTime CreationDate { get; protected set; }
        public DateTime? ModificationDate { get; protected set; }
        public TId? Id { get; protected set; }

        public bool IsChagned => _isChanged;

        #endregion

        #region Tracking Methods
        public virtual void ChangeEntityState(EntityState entityState) => EntityState = entityState;
        public virtual bool HasChanged() => _isChanged;
        public bool HasColumnChanged(string columnName) => _previousStage.ContainsKey(columnName);
        public T GetPreviousValue<T>(string columnName)
        {
            if (!_previousStage.ContainsKey(columnName)) return default(T)!;

            return (T)_previousStage[columnName]!;
        }
        #endregion

        protected T GetValue<T>([CallerMemberName] string? propertyName = null)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) && _backfeilds.ContainsKey(propertyName))
                return (T)_backfeilds[propertyName]!;

            return default(T)!;
        }
        protected void SetValue<T>(T data, [CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new InvalidDomainArgumentException("پارامتر ارسالی برای مقدار دهی پراپرتی خالی است و یا مقدار خاصی ندارد");

            if (!_backfeilds.ContainsKey(propertyName))
            {
                _backfeilds.Add(propertyName, (object)data!);
                return;
            }

            if (_backfeilds[propertyName] == null && !_previousStage.ContainsKey(propertyName))
            {
                _backfeilds[propertyName] = data;
                return;
            }
            else
            {
                if (_previousStage.ContainsKey(propertyName))
                {
                    if (_previousStage[propertyName] != (object)data!)
                    {
                        _previousStage[propertyName] = _backfeilds[propertyName];
                        _isChanged = true;
                        EntityState = EntityState.Modified;
                    }
                }
                else
                    _previousStage.Add(propertyName, (object)data!);

                _backfeilds[propertyName] = (object)data!;
            }

        }


        public virtual void SetModificationDate()
             => this.ModificationDate = DateTime.Now;

        public virtual void AccpetChanges()
        {
            _isChanged = true;
        }
    }


    public interface IBaseEntity
    {
        public bool IsChagned { get; }
        void AccpetChanges();
    }

}

