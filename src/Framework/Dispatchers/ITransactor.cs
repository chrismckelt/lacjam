using System;

namespace Lacjam.Framework.Dispatchers
{
    public interface ITransactor
    {
        void EnlistOrCreateTransactionForLambda(Action action);
        void ApplyTransactionForLambda(Action action);
        TResult ApplyTransactionForLambda<TResult>(Func<TResult> function) where TResult : new();
    }
}