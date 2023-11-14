using System;
using System.Collections.Generic;
using Godot;

public class Event<T> : IEvent
{

	public string Name { get; private set; }

	private List<Action<T>> _actions = new List<Action<T>>();

	private List<Confirmation> _confirmations = new List<Confirmation>();

	public Event(string name)
	{
		Name = name;
		Registry.RegisterEvent((IEvent) this);
	}

	public virtual void SendRendezvous(T data)
	{
		Pass("0", data);
	}

	public virtual void Pass(string id, T data)
	{
		
		Godot.Collections.Dictionary dict = new()
        {
            { "dest", id },
            { "name", Name },
            { "type", "pass" },
            { "data", ISerializable.Serialize(data) }
        };
		Network.SendData(dict);
	}


	/**
	* <summary> Not really need since comunication is throrugh TCP so it will always be sent. </summary>
	**/
	private void PassConfirm(string id, T data, Action<bool> callback)
	{

		//Generate a random string to use as a confirmation ID
		string confirmationID = RandomID();

		Godot.Collections.Dictionary dict = new()
        {
            { "dest", id },
            { "name", Name },
            { "type", "confirm_pass" },
            { "data", data.ToString() },
			{ "guid", confirmationID}
        };


		Confirmation conf = new Confirmation(confirmationID, callback, DateTime.Now);

		_confirmations.Add(conf);
		

		string packet = Json.Stringify(dict);

		GD.Print("Passing Event: " + Name + " Data: " + packet + " To: " + id);
	}

	public static Event<T> operator +(Event<T> e, Action<T> action)
	{
		e._actions.Add(action);
		return e;
	}

	public static Event<T> operator -(Event<T> e, Action<T> action)
	{
		e._actions.Remove(action);
		return e;
	}

	protected virtual void _invoke(T data)
	{
		foreach (Action<T> action in _actions)
		{
			action(data);
		}
	}

	public void Emit(T data)
	{
		_invoke(data);
	}

	public void _emit(string data, string _messageID)
	{
		_invoke(ISerializable.Deserialize<T>(data));
	}

    public void _send(string id, object data)
    {
		Pass(id, (T)data);
    }


	protected struct Confirmation
	{

		public string ID { get; private set; }

		public Action<bool> Callback { get; private set; }

		public DateTime Time { get; private set; }

		public Confirmation(string id, Action<bool> callback, DateTime time)
		{
			ID = id;
			Callback = callback;
			Time = time;
		}

		public void check()
		{
			if (Time.AddSeconds(5) < DateTime.Now)
			{
				Callback(false);
			}
		}
	}

	
	public string RandomID()
    {
        return Guid.NewGuid().ToString();
    }

}