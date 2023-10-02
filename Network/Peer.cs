using Godot;
using Godot.Collections;

public partial class Peer : Node {

	public string _id = "";
	public string _name = "";
	
	[Export]
	public string _ip = "";
	[Export]
	public int _port = 0;

	public Player player = null;

	public Peer(string id, string name, string ip, int port){
		_id = id;
		_name = name;
		_ip = ip;
		_port = port;

		requestPlayer();

		
	}

	public void requestPlayer()
	{
		GD.Print("Not Implemented");
		//Network.EmitRendezvous(RendezvousEvents.SignalName.RequestPlayerInfo, _id);
	}

	//TODO: Read name if it exists
	public static Peer deserialize(string s){

		//GD.Print("Deserializing peer: "+s);
		Dictionary dict = (Dictionary) Json.ParseString(s);
		return new Peer(dict["id"].AsString(), "", dict["ip"].AsString(), int.Parse(dict["port"].AsString()));
	}


	//TODO: Read Name if it exists
	public static Peer deserialize(Dictionary dict){
		GD.Print("Deserializing peer: "+dict);
		return new Peer(dict["id"].AsString(),"", dict["ip"].AsString(), int.Parse(dict["port"].AsString()));
	}

	public string Serialize(){
		Dictionary dict = new Dictionary();
		dict.Add("id", _id);
		dict.Add("ip", _ip);
		dict.Add("port", _port.ToString());
		return Json.Stringify(dict);
	}

	public string GetId(){
		return _id;
	}

	public string GetName(){
		return _name;
	}

	public string GetIp(){
		return _ip;
	}

	public int GetPort(){
		return _port;
	}


	private bool connected = false;


	//Eventually make this not have a seperate UDP connection for each peer
	public void Connect(PacketPeerUdp _udp){
		GD.Print("Connecting to peer: "+_id+ " from "+_udp.GetLocalPort());

		Timer timer = new Timer();
		AddChild(timer);
		timer.WaitTime = 1;
		timer.OneShot = false;
		timer.Start();
		//Ping the peer for 30 seconds every 5 seconds
		timer.Timeout += () => {
			_udp.SetDestAddress(_ip, _port);
			_udp.PutPacket("ping".ToUtf8Buffer());
			GD.Print("Sent ping to peer: "+_port);
			GD.Print("Local port: "+_udp.GetLocalPort());
		};

		GetTree().CreateTimer(30).Timeout += () => {
			if(!connected){
				Disconnect(_udp);
			}
			timer.QueueFree();
		};

	}
	

	public void Disconnect(PacketPeerUdp _udp){
		if(connected){
			//Let them know
		}
		connected = false;
		//_udp.Close();
		GD.Print("Disconnected from peer: "+_id);
	}


	public Peer Clone()
	{
		return new Peer(_id, _name, _ip, _port);
	}
}
