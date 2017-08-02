using System;
using System.ComponentModel;

namespace Lind.Microsoft.Core
{
    public interface INonNullReference : INotifyPropertyChanged, INotifyPropertyChanging { }
    public interface IONonNullReference<out T> : INonNullReference
    {
        T Value { get; }
    }
    public interface IINonNullReference<in T> : INonNullReference
    {
        T Value { set; }
    }
    public interface INonNullReference<T> : IONonNullReference<T>, IINonNullReference<T>
    { }
    public struct NonNullReference<T> : INonNullReference<T>
    {
        public NonNullReference(T initial)
        {
            value = initial.TestForNull();
            PropertyChanged = null;
            PropertyChanging = null;
        }
        private T value;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public T Value
        {
            get { return value; }
            set
            {
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Value"));
                this.value = value.TestForNull();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }
        public override bool Equals(object obj)
        {
            return value.Equals(obj);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(NonNullReference<T> reference, T value)
        {
            return reference.Equals(value);
        }
        public static bool operator !=(NonNullReference<T> reference, T value)
        {
            return !reference.Equals(value);
        }
        public static bool operator ==( T value, NonNullReference<T> reference)
        {
            return reference.Equals(value);
        }
        public static bool operator !=(T value, NonNullReference<T> reference)
        {
            return !reference.Equals(value);
        }
        public static NonNullReference<T> operator +(NonNullReference<T> reference, T value)
        {
            reference.Value = value;
            return reference;
        }
    }
}
