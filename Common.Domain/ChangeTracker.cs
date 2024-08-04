
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

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

        IEnumerable<IBaseEntity> Getchanges();
    }

    public sealed class ChangeTracker : IChangeTracker

    {
        private readonly HashSet<ChangeLog<IBaseEntity>> _changes = new HashSet<ChangeLog<IBaseEntity>>();

        public IEnumerable<IBaseEntity> Getchanges()
        {
            return _changes.Where(c => c.Data.IsChagned && c.State != EntityTrackState.UnChanged)
                           .Select(c => c.Data);
        }

        public void Track<T>(T entity, EntityTrackState state) where T : IBaseEntity
        {
            EntityTrackState _state = EntityTrackState.UnChanged;

            if (_changes.Any(c => c.DataHashCode == entity.GetHashCode()))
            {
                if (entity.IsChagned && state != EntityTrackState.Deleted)
                {

                    var currentlyChanged = entity.GetCurrentChanges();

                    var changeLog = _changes.SingleOrDefault(c => c.DataHashCode == entity.GetHashCode());
                    changeLog.UpdateLog(entity, state);
                    return;
                }
            }

            if (state == EntityTrackState.Added)
                _state = EntityTrackState.Added;
            else if (state == EntityTrackState.Modified)
                _state = EntityTrackState.Modified;
            else if (_state == EntityTrackState.Deleted)
                _state = EntityTrackState.Deleted;

            _changes.Add(new ChangeLog<IBaseEntity>(entity.GetHashCode(), entity, state));
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
