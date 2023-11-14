using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public class RequestEvent<T, K> : IEvent
{

    //These Events are used to request data from another peer
    protected List<Func<T, K>> _reponses = new();

    protected List<Request> _requests = new();

    public string Name { get; protected set; }

    public RequestEvent(string name)
    {
        Name = name;
        Registry.RegisterEvent((IEvent) this);
    }


    protected virtual void _invoke(T data, string requestID)
    {  
        List<K> answers = new();

        foreach (Func<T, K> response in _reponses)
        {
            answers.Add(response(data));
        }

        //Send answers to peer (assuming peer is rendezvous)

        _respond(requestID, answers);

    }

    protected virtual void _respond(string requestID, List<K> data)
    {

        string[] vars = new string[data.Count];

        for(int i = 0; i < data.Count; i++)
        {
            vars[i] = ISerializable.Serialize(data[i]);
        }

        Godot.Collections.Dictionary dict = new()
        {
            { "name", Name },
            { "type", "response" },
            { "data",  vars},
            { "guid", requestID }
        };

        Network.SendData(dict);
    }

    public void Send(string id, T data, Action<List<K>> response)
    {
        string responseID = RandomID();

		Godot.Collections.Dictionary dict = new()
		{
			{ "dest", id },
			{ "name", Name},
			{ "type", "reqeust"},
			{ "data", data.ToString()},
			{ "guid", responseID}
		};

        _requests.Add(new Request(responseID, false, response));


        //Send to rendezvous

        Network.SendData(dict);
    }


    public void _emit(string data, string messageID)
    {

        //Check messageID if this is a response
        //if it is, send to _respond
        int i = _requests.FindIndex((Request req) => req.requestID == messageID);
        if( i != -1)
        {
            //This is a response
            Dictionary dict = (Dictionary) Json.ParseString(data);
            string[] vars = (string[]) dict["data"];
            List<K> answers = new();

            for(int j = 0; j < vars.Length; j++)
            {
                answers.Add(ISerializable.Deserialize<K>(vars[j]));
            }

            _requests[i].callback(answers);
            return;
        }

        _invoke(ISerializable.Deserialize<T>(data), messageID);
    }

    public void _send(string id, object data)
    {
        //Shouldn't Be usec
        GodotLogger.warn("_send isn't used in RequestEvent");
        //Send(id, (T) data);
    }

    public static RequestEvent<T, K> operator +(RequestEvent<T, K> e, Func<T, K> action)
	{
		e._reponses.Add(action);
		return e;
	}

	public static RequestEvent<T, K> operator -(RequestEvent<T, K> e, Func<T, K> action)
	{
		e._reponses.Remove(action);
		return e;
	}

    public struct Request
    {
        public string requestID;
        public bool isRequest;
        public Action<List<K>> callback;

        public Request(string requestID, bool isRequest, Action<List<K>> callback)
        {
            this.requestID = requestID;
            this.isRequest = isRequest;
            this.callback = callback;
        }
    }

    
	public string RandomID()
    {
        return Guid.NewGuid().ToString();
    }

}