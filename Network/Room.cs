using Godot;
using System;
using System.Collections.Generic;

public partial class Room : Node {


	public string _id = "";
	public string _name = "";
	
	public Peer _host = new Peer("", "", "", 0);
	public List<Peer> _clients = new List<Peer>();

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
		_clients.Remove(p);
		RemoveChild(p);
		p.QueueFree();
	}

	public void leave(){
		_id = "";
		_name = "";
		_host.QueueFree();
		_clients.ForEach(x => x.QueueFree()); 
		_host = new Peer("", "", "", 0);
		_clients = new List<Peer>();
		Name = "Room";
	}

}
