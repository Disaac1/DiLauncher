using System;
using System.Collections.Generic;

public class Peer_
{

    public string id { get; private set;}
    public string ip;
    public int    port;

    public Peer_(string id, int port)
    {
        this.id = id;
        this.port = port;
    }

    public virtual void Pass<T>(Event<T> e, T data)
    {
        //Passes an event to this peer.
        e.Pass(id, data);
    }

    public virtual void Request<T, K>(RequestEvent<T, K> e, T data, Action<List<K>> response)
    {
        //Request an event to this peer.
        e.Send(id, data, response);
    }




}