using Godot;

public partial class  RendezvousEvents : Node 
{


    [Signal]
    public delegate void IDEventHandler(IDEvent @event);

    [Signal]
    public delegate void LeaveRoomEventHandler();

    [Signal]
    public delegate void JoinRoomEventHandler();

    [Signal]
    public delegate void CreateRoomEventHandler();

    [Signal]
    public delegate void RoomListEventHandler();

    [Signal]
    public delegate void ClientJoinedRoomEventHandler();

    [Signal]
    public delegate void ClientLeftRoomEventHandler();


    public partial class IDEvent : Resource
    {
        public string id;
        public int port;
    }

    public static RendezvousEvents instance;

    public override void _EnterTree()
    {
        Name = "RendezvousEvents";
        instance ??= this;
        GD.Print("Registered: " + Name);
    }

    public override void _Ready()
    {
        Network.instance.OnEvent += ((string name, Variant data) =>
        {
            if(name == "id")
            {
                GD.Print("ID:", data.ToString());
                IDEvent ev = new IDEvent();
                ev.id = "";
                ev.port = 0;
                EmitSignal(RendezvousEvents.SignalName.ID, ev);
            }
        });

        Network.instance.SendRendezvous += (string eventName, Variant data) =>
        {
            if(eventName == RendezvousEvents.SignalName.ID)
            {
                EmitSignal(Network.SignalName.OnSendRendezvous, "id", data);
            }
        };
    }
}