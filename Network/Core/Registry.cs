
using System.Collections.Generic;
using Godot;

public class Registry
{

	public static Dictionary<string, IEvent> EVENTS = new Dictionary<string, IEvent>();


	public static void RegisterEvent(IEvent Event)
	{
		EVENTS.Add(Event.Name, Event);
		GodotLogger.info("Registered Event: " + Event.Name);
	}

	public static IEvent GetEvent(string eventName)
	{
		return EVENTS[eventName];
	}

}

