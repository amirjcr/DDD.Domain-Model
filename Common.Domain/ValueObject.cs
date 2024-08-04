using System;

namespace Common.Domain
{

    public abstract class ValueObject : IEquatable<ValueObject>
    {

        #region Overrides

        protected static bool EqualOpertor(ValueObject? left, ValueObject? right)
        {
            if (left == null || right == null) return false;
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;

            return ReferenceEquals(left, right) || left!.Equals(right);
        }


        protected static bool NotEqualOpertor(ValueObject? left, ValueObject? right)
            => !EqualOpertor(left, right);


        public static bool operator ==(ValueObject? left, ValueObject? right)
            => EqualOpertor(left, right);

        public static bool operator !=(ValueObject? left, ValueObject? right) 
            => NotEqualOpertor(left,right);


        public bool Equals(ValueObject? other)
        {
            if (other == null) return false;

            if (ReferenceEquals(other, this)) return true;

            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            return this.Equals((ValueObject)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * 3 / 1 + 39;
        }
        #endregion

    }

}


