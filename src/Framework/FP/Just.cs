using System;
using System.Diagnostics;

namespace Lacjam.Framework.FP
{
    [DebuggerStepThrough]
    public class Just<T> : IMaybe<T>
    {
         [DebuggerStepThrough]
        public Just(T value)
        {
            this.Value = value;
        }

        public IMaybe<U> Fmap<U>(Func<T,U> apply)
        {
            return new Just<U>(apply(Value));
        }
         
        public IMaybe<U> Bind<U>(Func<T,IMaybe<U>> apply)
        {
            return apply(Value);
        }

        public IMaybe<V> Bind<U, V>(Func<T, IMaybe<U>> func, Func< U, V> map)
        {
            return func(Value).Fmap(map);
        }

        public IMaybe<T> Filter(Func<T, bool> predicate)
        {
            if (predicate(Value))
                return this;
            return new None<T>();
        }

        
        public void Foreach(Action<T> action)
        {
            action(Value);
        }

        public U Fold<U>(Func<T, U> some, Func<U> none)
        {
            return some(Value);
        }

        public void OnEmpty(Action action)
        {

        }

        public void OnNonEmpty(Action<T> action)
        {
            action(Value);
        }

        public T Value { get; set; }

        public bool Exists
        {
            get { return true; }
        }
    }
}