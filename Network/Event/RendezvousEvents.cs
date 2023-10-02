using System;
using Godot;

public partial class  RendezvousEvents : Node 
{


	[Signal]
	public delegate void IDEventHandler(IDEvent @event);

	[Signal]
	public delegate void LeaveRoomEventHandler();

	[Signal]
	public delegate void JoinRoomEventHandler(Room room);

	[Signal]
	public delegate void CreateRoomEventHandler(string id);

	[Signal]
	public delegate void RoomListEventHandler();

	[Signal]
	public delegate void ClientJoinedRoomEventHandler(Peer peer);

	[Signal]
	public delegate void ClientLeftRoomEventHandler(Peer peer);

	[Signal]
	public delegate void RequestDirectConnectEventHandler(int port);

	[Signal]
	public delegate void RequestPlayerInfoEventHandler(string peerID);


	

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
				ev.id = data.AsGodotDictionary()["id"].AsString();
				ev.port = data.AsGodotDictionary()["port"].AsInt32();
				EmitSignal(RendezvousEvents.SignalName.ID, ev);
			}
			if(name == "createRoom")
			{
				GD.Print("Room Created:", data.ToString());
				EmitSignal(RendezvousEvents.SignalName.CreateRoom, data.ToString());
			}
			if(name == "joinedRoom")
			{
				EmitSignal(RendezvousEvents.SignalName.JoinRoom, Room.deserialize(data.AsGodotDictionary()));
			}
			if(name == "clientJoined")
			{
				Peer peer = Peer.deserialize(data.AsGodotDictionary());
				EmitSignal(RendezvousEvents.SignalName.ClientJoinedRoom, peer);
			}
			if(name == "clientLeft")
			{
				EmitSignal(RendezvousEvents.SignalName.ClientLeftRoom, Peer.deserialize(data.AsGodotDictionary()));
			}
			if(name == "leaveRoom")
			{
				EmitSignal(RendezvousEvents.SignalName.LeaveRoom, "");
			}
			if(name == "requestDirectConnect")
			{
				EmitSignal(RendezvousEvents.SignalName.RequestDirectConnect, data.AsInt16());
			}
		});

		Network.instance.SendRendezvous += (string eventName, Variant data) =>
		{
			if(eventName == RendezvousEvents.SignalName.ID)
			{
				Network.instance._SendRendezvous("id", data);
			}
			if(eventName == RendezvousEvents.SignalName.CreateRoom)
			{
				Network.instance._SendRendezvous("createRoom", data);
			}
			if(eventName == RendezvousEvents.SignalName.JoinRoom)
			{
                Network.instance._SendRendezvous("joinRoom", data);
            }
			if(eventName == RendezvousEvents.SignalName.LeaveRoom)
			{
				  Network.instance._SendRendezvous("leaveRoom", data);
			}
			if(eventName == RendezvousEvents.SignalName.RequestDirectConnect)
			{
				Network.instance._SendRendezvous("requestDirectConnect", data);
			}
			if(eventName == RendezvousEvents.SignalName.RequestPlayerInfo)
			{
				Network.instance._SendRendezvous("requestPlayerInfo", data);
			}
		};
	}
}
