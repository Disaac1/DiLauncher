using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Room : Node {


	public string _id = "";
	public string _name = "";
	
	public Peer _host = new Peer("", "", "", 0);
	public List<Peer> _clients = new List<Peer>();


	public override void _Ready()
	{
		RendezvousEvents.instance.ClientJoinedRoom += addClient;
		RendezvousEvents.instance.ClientLeftRoom += removeClient;
		RendezvousEvents.instance.LeaveRoom += () => leave();
    }

	public void SetHost(Peer p){
		_host.QueueFree();
		_host = p;
		_host.Name = "Host_"+p.GetId();
		AddChild(p);
	}

	public void addClient(Peer p){
		_clients.Add(p);
		p.Name = "Peer_"+p.GetId();
		AddChild(p);
	}

	public void removeClient(Peer p){
		p = _clients.Find(x => x.GetId() == p.GetId());
		_clients.Remove(p);
		RemoveChild(p);
		p.QueueFree();
	}

	public void leave(bool annouce = false){
		_id = "";
		_name = "";
		_host.QueueFree();
		_clients.ForEach(x => x.QueueFree()); 
		_host = new Peer("", "", "", 0);
		_clients = new List<Peer>();
		Name = "Room";
		if(annouce) Network.EmitRendezvous(RendezvousEvents.SignalName.LeaveRoom, "");
	}

	public void SetRoom(Room room)
	{
		leave();
		this._id = room._id;
		this._name = room._name;
		SetHost(room._host.Clone());
		foreach (Peer client in room._clients)
		{
            addClient(client.Clone());
        }
		this.Name = "Room_"+room._id;
	}

	public static Room deserialize(Dictionary dict)
	{
		Room room = new();

		room._id = dict["id"].AsString();
		room._name = dict["name"].AsString();
		room.Name = "Room_"+room._id;
		room.SetHost(Peer.deserialize(dict["host"].AsGodotDictionary()));
		foreach (Dictionary client in dict["clients"].AsGodotArray())
		{
            room.addClient(Peer.deserialize(client));
        }
		return room;
	}

}
