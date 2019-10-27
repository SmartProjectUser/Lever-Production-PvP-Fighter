using System;

public delegate void EventDelegate<TData>(Event<TData> data);
public struct Event<TData>
{
    public TData data;
}

static public class EventDispatcher<EventType>
{
    static public event Action<Event<EventType>> OnEvent;
    static public void Broadcast(EventType data)
    {
        if (OnEvent != null)
            OnEvent(new Event<EventType> { data = data });
    }
}

static public class EventDispatcherExt
{
    static public void Broadcast<T>(this T data)
    {
        EventDispatcher<T>.Broadcast(data);
    }
}