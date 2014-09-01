using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public interface IEventHandlerErrorRepository
    {
        void AddError(EventHandlerError error);
        void UpdateError(EventHandlerError error);
        IMaybe<EventHandlerError> GetLastError();
        long GetTotalErrors();
    }
}