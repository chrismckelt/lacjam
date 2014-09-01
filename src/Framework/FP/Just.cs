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
            this.value = value;
        }

        public IMaybe<U> Fmap<U>(Func<T,U> apply)
        {
            return new Just<U>(apply(value));
        }
         
        public IMaybe<U> Bind<U>(Func<T,IMaybe<U>> apply)
        {
            return apply(value);
        }

        public IMaybe<V> Bind<U, V>(Func<T, IMaybe<U>> func, Func< U, V> map)
        {
            return func(value).Fmap(map);
        }

        public IMaybe<T> Filter(Func<T, bool> predicate)
        {
            if (predicate(value))
                return this;
            return new None<T>();
        }

        
        public void Foreach(Action<T> action)
        {
            action(value);
        }

        public U Fold<U>(Func<T, U> some, Func<U> none)
        {
            return some(value);
        }

        public void OnEmpty(Action action)
        {

        }

        public void OnNonEmpty(Action<T> action)
        {
            action(value);
        }

        private readonly T value;
    }
}