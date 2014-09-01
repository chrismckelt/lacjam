using System;
using System.Collections.Generic;
using System.Linq;

namespace Lacjam.Framework.FP
{

    public class PatternMatchContext<T>
    {
        private readonly T _value;

        internal PatternMatchContext(T value)
        {
            _value = value;
        }

        public PatternMatch<T, TResult> With<TResult>(
            Predicate<T> condition,
            Func<T, TResult> result)
        {
            var match = new PatternMatch<T, TResult>(_value);
            return match.With(condition, result);
        }
    }

    public static class PatternMatchExtensions
    {
        public static PatternMatchContext<T> Match<T>(this T value)
        {
            return new PatternMatchContext<T>(value);
        }
    }

    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(string message) : base(message) { }
    }

    public class PatternMatch<T, TResult>
    {
        private readonly T _value;
        private readonly List<Tuple<Predicate<T>, Func<T, TResult>>> _cases
            = new List<Tuple<Predicate<T>, Func<T, TResult>>>();
        private Func<T, TResult> _elseFunc;

        internal PatternMatch(T value)
        {
            _value = value;
        }

        public PatternMatch<T, TResult> With(Predicate<T> pred, Func<T, TResult> action)
        {
            _cases.Add(Tuple.Create(pred, action));
            return this;
        }

        public PatternMatch<T, TResult> Else(Func<T, TResult> action)
        {
            if (_elseFunc != null)
                throw new InvalidOperationException("Cannot have multiple else cases");

            _elseFunc = action;
            return this;
        }

        public TResult Do()
        {
            if (_elseFunc != null)
                _cases.Add(Tuple.Create<Predicate<T>, Func<T, TResult>>(x => true, _elseFunc));
            foreach (var item in _cases.Where(item => item.Item1(_value)))
                return item.Item2(_value);

            throw new MatchNotFoundException("Incomplete pattern match");
        }
    }
}

