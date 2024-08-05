
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Domain
{

    public interface IChangeLog<T> where T : IBaseEntity
    {
        int DataHashCode { get; }
        T Data { get; }
        DateTime LastModifiedDate { get; }
        EntityTrackState State { get; }

        void UpdateLog(T entity, EntityTrackState state);

    }

    public sealed class ChangeLog<T> : IChangeLog<T>
    where T : IBaseEntity
    {

        private int _dataHashCode;
        private T _data;
        private DateTime _lastModifiedDate;
        private EntityTrackState _state;

        public ChangeLog(int dataHashCode, T data, EntityTrackState state)
        {
            _dataHashCode = dataHashCode;
            _data = data;
            _lastModifiedDate = DateTime.Now;
            _state = state;
        }

        public int DataHashCode => _dataHashCode;
        public T Data => _data;
        public DateTime LastModifiedDate => _lastModifiedDate;
        public EntityTrackState State => _state;

        public void UpdateLog(T entity, EntityTrackState state)
        {
            _lastModifiedDate = DateTime.Now;
            _state = state;
            _data = entity;
        }
    }



    public interface IChangeTracker
    {
        void Track<T>(T entity, EntityTrackState state) where T : IBaseEntity;
        void Hold<T>(T entity) where T : IBaseEntity;

    }

    public sealed class ChangeTracker : IChangeTracker

    {
        private readonly HashSet<ChangeLog<IBaseEntity>> _changes = new HashSet<ChangeLog<IBaseEntity>>();
        private readonly HashSet<IBaseEntity> _entityHolder = new HashSet<IBaseEntity>();


        public IEnumerable<IBaseEntity> Getchanges()
        {
            return _changes.Where(c => c.Data.IsChagned && c.State != EntityTrackState.UnChanged)
                           .Select(c => c.Data);
        }

        public void Track<T>(T entity, EntityTrackState state) where T : IBaseEntity
        {

            if (!entity.IsChagned) return;


            if (entity.State == Enums.EntityState.Changed)
            {
                // get entity chagnes 
            }
            if (entity.HasAnyChildChanged())
            {
                var _childChanges = entity.GetChildChanges();

                foreach (var item in _childChanges)
                    _changes.Add(new ChangeLog<IBaseEntity>(entity.GetHashCode(), entity, state));
            }

        }

        public void Hold<T>(T entity) where T : IBaseEntity
        {
            _entityHolder.Add(entity);
        }

    }





    public enum EntityTrackState
    {
        Added,
        Modified,
        Deleted,
        UnChanged
    }
}
