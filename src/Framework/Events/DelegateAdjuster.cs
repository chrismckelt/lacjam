using System;
using System.Linq.Expressions;

namespace Lacjam.Framework.Events
{
    public class DelegateAdjuster
    {
        public static Action<BaseT> CastArgument<BaseT, DerivedT>(Expression<Action<DerivedT>> source) where DerivedT : BaseT
        {
            if (typeof(DerivedT) == typeof(BaseT))
            {
                return (Action<BaseT>)((Delegate)source.Compile());

            }
            ParameterExpression sourceParameter = Expression.Parameter(typeof(BaseT), "source");
            var result = Expression.Lambda<Action<BaseT>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof(DerivedT))),
                sourceParameter);
            return result.Compile();
        }

        public static Action<BaseT,OtherArgT> CastArgument<BaseT, OtherArgT, DerivedT>(Expression<Action<DerivedT, OtherArgT>> source) where DerivedT : BaseT
        {
            if (typeof(DerivedT) == typeof(BaseT))
            {
                return (Action<BaseT,OtherArgT>)((Delegate)source.Compile());

            }
            ParameterExpression sourceParameter = Expression.Parameter(typeof(BaseT), "source");
            ParameterExpression otherParameter = Expression.Parameter(typeof(OtherArgT), "other");
            var result = Expression.Lambda<Action<BaseT,OtherArgT>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof(DerivedT)),otherParameter),
                sourceParameter, otherParameter);
            return result.Compile();
        }
    }
}