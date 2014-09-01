using System;
using System.Diagnostics;

namespace Lacjam.Framework.FP
{
    [DebuggerStepThrough]
    public static class MaybeExtensions
    {
        [DebuggerStepThrough]
        public static IMaybe<T> Just<T>(this T value)
        {
            return new Just<T>(value);
        }

        [DebuggerStepThrough]
        public static IMaybe<T> ToMaybe<T>(this T value)
        {
            if (value == null)
                return new None<T>();

            return new Just<T>(value);
        }

        [DebuggerStepThrough]
        public static IMaybe<U> Select<T, U>(this IMaybe<T> m, Func<T, U> select)
        {
            return m.Fmap(select);
        }

        [DebuggerStepThrough]
        public static IMaybe<V> SelectMany<T, U, V>(this IMaybe<T> m, Func<T, IMaybe<U>> func, Func<T,U,V> select)
        {
            return m.Bind(a => func(a).Bind(b => select(a, b).ToMaybe()));
        }
    }
}