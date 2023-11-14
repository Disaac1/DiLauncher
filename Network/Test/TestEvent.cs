using System;

public class TestEvent<T> : Event<T>
{


    public static TestEvent<T> PassSent = new TestEvent<T>((data) =>
    {
        GD.Print("Pass Sent: " + data.ToString());
    }, "PassSent");

    //These Events are used to 

    public Action<T> hook;

    public TestEvent(Action<T> hook, string name) : base(name)
    {
        this.hook = hook;
    }


    protected override void _invoke(T data)
    {
        GD.Print("Event, " + Name + " invoked with data: " + ISerializable.Serialize(data));
        hook(data);
    }

}