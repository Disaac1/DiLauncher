using System;
using System.Collections.Generic;

public class TestRequestEvent<T, K> : RequestEvent<T, K>
{
    public TestRequestEvent(string name, Action<T> hook, Action<K> hookResp) : base(name)
    {
        this.hook = hook;
        this.hookResp = hookResp;
    }

    public Action<T> hook;
    public Action<K> hookResp;

    protected override void _invoke(T data, string requestID)
    {
        GD.Print("Request Event, " + Name + " invoked with data: " + data.ToString());
        hook(data);

        base._invoke(data, requestID);
    }

    protected override void _respond(string requestID, List<K> data)
    {
        GD.Print("Request Event, " + Name + " responded with data: " + data.ToString());
        hookResp(data[0]);
    }


    public void add(Func<T, K> response)
    {
        _reponses.Add(response);
    }
    public void Remove(Func<T, K> response)
    {
        _reponses.Remove(response);
    }


}