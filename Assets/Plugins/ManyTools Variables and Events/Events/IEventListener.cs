namespace ManyTools.Events.Types
{
    public interface IEventListener<in T>
    {
        void OnEventInvoked(T value);
    }
}