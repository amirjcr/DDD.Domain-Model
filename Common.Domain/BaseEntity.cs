using Common.Domain.DomainEvents;
using Common.Domain.Enums;
using Common.Domain.Exceptions;
using Common.Domain.Utilites;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Common.Domain
{
    public abstract class BaseEntity<TId> : IBaseEntity
    {

        #region Private Feilds
        private EntityState _state = EntityState.UnChanged;
        protected bool _isRoot = false;

        protected List<IDomainEvent> _domainEvents = new List<IDomainEvent>(); // for cases you will use domain event's
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
        public TId Id { get; protected set; }

        public bool IsChagned => _state == EntityState.Changed || HasAnyChildChanged();

        public EntityState State => throw new NotImplementedException();

        public bool IsRoot => throw new NotImplementedException();

        #endregion

        #region Tracking Methods
        public bool HasColumnChanged(string columnName) => _previousStage.ContainsKey(columnName);
        public T GetPreviousValue<T>(string columnName)
        {
            CheckPropertyNameIsNotNullOrEmpty(columnName);

            if (!_previousStage.ContainsKey(columnName)) return default(T)!;

            return (T)_previousStage[columnName]!;
        }
        public void RollBackValue(string columnName)
        {
            CheckPropertyNameIsNotNullOrEmpty(columnName);

            if (_previousStage.ContainsKey(columnName))
                _backfeilds[columnName] = _previousStage[columnName];
        }

        public Dictionary<string, object> GetEntityChanges()
        {
            var modifiedProperties = new Dictionary<string, object>();

            foreach (var item in _backfeilds)
                if (_previousStage.ContainsKey(item.Key) && item.Value != _previousStage[item.Key])
                    modifiedProperties.Add(item.Key, item.Value!);

            return modifiedProperties;
        }

        public abstract void AccpetChanges();

        #endregion


        #region Methods

        public bool HasAnyChildChanged()
        {
            if (_previousStage.Count() == 0) return false;

            foreach (var item in _previousStage)
            {
                if (item.Value is IBaseEntity child)
                    return child.State == EntityState.Changed;
                else if (item.Value is IEnumerable<IBaseEntity> childern)
                    return childern.Any(c => c.State == EntityState.Changed);
            }

            return false;
        }

        public IEnumerable<IBaseEntity> GetChildChanges()
        {
            if (_previousStage.Count() == 0) return null!;

            List<IBaseEntity>? changedList = new List<IBaseEntity>();

            foreach (var item in _previousStage.Values)
            {
                if (item is IBaseEntity child && child.State == EntityState.Changed)
                    changedList.Add(child);

                else if (item is IEnumerable<IBaseEntity> childern)
                    changedList.AddRange(childern.Where(c => c.State == EntityState.Changed));
            }

            return changedList;
        }

        public virtual void SetModificationDate()
             => this.ModificationDate = DateTime.Now;

        #endregion

        private void CheckPropertyNameIsNotNullOrEmpty(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new Exception();
        }

        protected T GetValue<T>([CallerMemberName] string? propertyName = null)
        {
            CheckPropertyNameIsNotNullOrEmpty(propertyName!);

            if (_backfeilds.ContainsKey(propertyName!))
                return (T)_backfeilds[propertyName!]!;

            return default(T)!;
        }
        protected void SetValue<T>(T data, [CallerMemberName] string? propertyName = null)
        {
            CheckPropertyNameIsNotNullOrEmpty(propertyName!);


            if (!_backfeilds.ContainsKey(propertyName!))
            {
                _backfeilds.Add(propertyName!, (object)data!);
                return;
            }

            if (_backfeilds[propertyName!] == null && !_previousStage.ContainsKey(propertyName!))
            {
                _backfeilds[propertyName!] = data;
                return;
            }
            else
            {
                if (_previousStage.ContainsKey(propertyName!))
                {
                    if (_previousStage[propertyName!] != (object)data!)
                    {
                        _previousStage[propertyName!] = _backfeilds[propertyName!];
                        _isChanged = true;
                    }
                }
                else
                    _previousStage.Add(propertyName!, (object)data!);

                _backfeilds[propertyName!] = (object)data!;
            }

        }
    }


    public interface IBaseEntity
    {
        /// <summary>
        /// TrackChanges All across Aggregate
        /// </summary>
        bool IsChagned { get; }

        /// <summary>
        /// Check if Entity is root or not
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Track Changes In Entity Level
        /// </summary>
        EntityState State { get; }


        /// <summary>
        /// Accept Changes and Rest the Tracker State
        /// </summary>
        void AccpetChanges();

        /// <summary>
        /// Check if an child entity is changed or not.
        /// </summary>
        /// <returns></returns>
        bool HasAnyChildChanged();


        /// <summary>
        /// GetAllChildChanges
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBaseEntity> GetChildChanges();
    }

}

