namespace SongService.Messaging;

public interface IEventListener
{
    public void Subscribe<T>(string topic, Action<T> handler);
}