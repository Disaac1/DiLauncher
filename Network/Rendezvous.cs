using Godot;

public partial class Rendezvous : Node
{



    private Peer self = new Peer("0", "Self", "127.0.0.1", 1256);
    private Room room = new Room();

    public override void _Ready()
    {
        Name = "Rendezvous";
        self.Name = "Self";
        AddChild(self);
        room.Name = "Room";
        AddChild(room);

        RendezvousEvents.instance.ID += onID;
    }


    public void onID(RendezvousEvents.IDEvent id)
    {
        self._id = id.id;
        self._port = id.port;
    }
    
    public void CreateRoom(string name)
    {
        GD.Print("Requesting Room from Sever with name " + name);
        Network.EmitRendezvous(RendezvousEvents.SignalName.CreateRoom, name);
    }

    public void JoinRoom(string roomID)
    {
        Network.EmitRendezvous(RendezvousEvents.SignalName.JoinRoom, roomID);
    }

    public void onLeaveRoom()
    {

    }

    public void Connect()
    {

    }
    
}
