

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Common.Domain
{
    public sealed class EntityCollection<T> : INotifyCollectionChanged
        where T : IBaseEntity
    {
        private readonly List<T> _entites = new List<T>();


        #region Constractors
        public EntityCollection()
        {

        }

        public EntityCollection(IEnumerable<T> entites)
        {
            _entites = entites.ToList();
        }

        #endregion

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public List<T> CurrentList => _entites;


        #region Methods
        public void Add(T entity)
        {
            _entites.Add(entity);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, entity);
        }

        public void Remove(T entity)
        {
            var _entity = _entites.SingleOrDefault(e => e.Equals(entity));

            if (_entity == null) return;

            _entites.Remove(entity);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, entity);
        }

        public void Replace(T entity)
        {
            var _entity = _entites.SingleOrDefault(e => (object)e.GetHashCode() == (object)entity.GetHashCode());
            if (_entity == null) return;

            _entites.Remove(entity);
            _entites.Add(entity);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, entity);
        }

        #endregion

        private void OnCollectionChanged(NotifyCollectionChangedAction notify, T data)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(notify, data));
        }
    }
}
