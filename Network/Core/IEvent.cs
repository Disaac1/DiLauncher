using System;
using Godot;

public interface IEvent
{
    public string Name { get; }

    public abstract void _emit(string data, string messageID);

    public abstract void _send(string id, object data);

}