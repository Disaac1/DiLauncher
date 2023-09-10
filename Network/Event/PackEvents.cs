using Godot;

public partial class PackEvents : Node
{

    [Signal]
    public delegate void RequestPacksEventHandler();



    public static PackEvents instance;
    public override void _EnterTree()
    {
        Name = "PackEvents";
        instance ??= this;
        GD.Print("Registered: " + Name);
    }

    public override void _Ready()
    {
        Network.instance.OnEvent += ((string name, Variant data) =>
        {
            if (name == "RequestPack") EmitSignal(PackEvents.SignalName.RequestPacks, data);
        });

        Network.instance.SendRendezvous += (string eventName, Variant data) =>
        {
            if(eventName == PackEvents.SignalName.RequestPacks)
            {
                Network.instance._SendRendezvous("RequestPack", data);
            }
        };
    }




}
