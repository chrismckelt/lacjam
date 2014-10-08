using System;

namespace Lacjam.Framework.FP
{
    public interface IMaybe<T>
    {
        IMaybe<U> Fmap<U>(Func<T,U> apply);
        IMaybe<U> Bind<U>(Func<T,IMaybe<U>> apply);
        IMaybe<V> Bind<U, V>(Func<T, IMaybe<U>> func, Func< U, V> map);
        IMaybe<T> Filter(Func<T, bool> predicate); 
        void Foreach(Action<T> action);
        U Fold<U> (Func<T,U> some, Func<U> none);
        void OnEmpty(Action action);
        void OnNonEmpty(Action<T> action);
        T Value { get; set; }
        bool Exists { get; }
    }
}