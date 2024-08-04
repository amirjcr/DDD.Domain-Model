using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common.Domain
{

    public interface ITrackableEntity
    {
        EntityState EntityState { get; set; }
    }

    public abstract class BaseChagneTracker : ITrackableEntity, INotifyPropertyChanged
    {
        private EntityState _entityState = EntityState.UnChanged;
        private EntityState _previousState = EntityState.UnChanged;

        protected readonly Dictionary<ITrackableEntity, EntityState> _changes = new Dictionary<ITrackableEntity, EntityState>();


        public EntityState EntityState
        {
            get => _entityState;
            set
            {
                if (_entityState != value)
                {
                    _entityState = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Track(ITrackableEntity entiy);
    }


    public class ChageTracker : BaseChagneTracker
    {
        public override void Track(ITrackableEntity entiy)
        {
            throw new NotImplementedException();
        }
    }

    public enum EntityState
    {
        Deleted,
        Added,
        Modified,
        Untracked,
        UnChanged,
        Loaded
    }
}
