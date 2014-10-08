using System;
using System.Diagnostics;

namespace Lacjam.Framework.FP
{
    [DebuggerStepThrough]
    public class None<T> : IMaybe<T>
    {
        public IMaybe<U> Fmap<U>(Func<T,U> apply)
        {
            return new None<U>();
        }

        public IMaybe<U> Bind<U>(Func<T,IMaybe<U>> apply)
        {
            return new None<U>();
        }

        public IMaybe<V> Bind<U, V>(Func<T, IMaybe<U>> func, Func<U, V> map)
        {
            return new None<V>();
        }

        public IMaybe<T> Filter(Func<T, bool> predicate)
        {
            return this;
        }

        [DebuggerStepThrough]
        public void Foreach(Action<T> action)
        {
            
        }

        public U Fold< U>(Func<T, U> some, Func<U> none)
        {
            return none();
        }

        public void OnEmpty(Action action)
        {
            action();
        }

        public void OnNonEmpty(Action<T> action)
        {
            
        }

        public T Value { get; set; }

        public bool Exists
        {
            get { return false; }
        }
    }
}