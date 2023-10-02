using Godot;
using Godot.Collections;

public partial class Rendezvous : Node
{



    private Peer self = new("0", "Self", "127.0.0.1", 1256);
    private Room room = new();

    private PacketPeerUdp udp = new();

    public override void _Ready()
    {
        Name = "Rendezvous";
        self.Name = "Self";
        AddChild(self);
        room.Name = "Room";
        AddChild(room);

        RendezvousEvents.instance.ID += onID;
        RendezvousEvents.instance.CreateRoom += onRoomCreated;
        RendezvousEvents.instance.JoinRoom += onJoinRoom;
        RendezvousEvents.instance.RequestDirectConnect += onConnectRequest;
    }

    public Peer getSelf()
    {
        return self;
    }


    public void onID(RendezvousEvents.IDEvent id)
    {
        self._id = id.id;
        self._port = id.port;
        Network.player.peer = self;
    }
    
    public void CreateRoom(string name)
    {
        GD.Print("Requesting Room from Sever with name " + name);
        Network.EmitRendezvous(RendezvousEvents.SignalName.CreateRoom, name);
    }

    public void onRoomCreated(string id)
    {
        
        room.SetHost(self.Clone());
        room._name = "My Room";
        room._id = id;
        room.Name = "Room_"+room._id;
        self.Name = "(Host)Self_"+self._id;

    }

    public void JoinRoom(string roomID)
    {
        GD.Print("Requesting to join room " + roomID);
        Network.EmitRendezvous(RendezvousEvents.SignalName.JoinRoom, roomID);
    }

    public void onJoinRoom(Room room)
    {
        this.room.SetRoom(room);
    }

    public void Connect()
    {

    }

    public Room getRoom()
    {
        return room;
    }


    public override void _Process(double delta)
    {
        if(udp.GetAvailablePacketCount() > 0)
        {
            byte[] data = udp.GetPacket();
            string dataString = data.GetStringFromUtf8();

            GD.Print("UDP: ", dataString);
        }
    }

    public void StartPeerContact()
    {
        udp.Bind(self._port);
        
        //Have all peers attempt to ping the host
        //Have the host attempt to ping all peers

        //First ping with port given by server but then cascade to other ports if no response



    }

    public void onConnectRequest(int port)
    {
        //GD.Print("Requsted to connect to " + connect.ip + ":" + connect.port);
        //udp.ConnectToHost(connect.ip, int.Parse(connect.port));
        //udp.PutPacket("Hello".ToUtf8Buffer());
    }

}
