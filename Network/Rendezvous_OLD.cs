using System;
using Godot;
using Godot.Collections;

public partial class Rendezvous : Node {


	//Since the Rendezvous server is centrealized we know the IP and PORT and can use a TCP connection

	private static string _IP = "disaac1.com";
	private static int _PORT = 1256;

	private StreamPeerTcp _tcp = new StreamPeerTcp();

	private PacketPeerUdp _udp = new PacketPeerUdp();


	[Export]
	public Peer _self = new Peer("0", "Self", "127.0.0.1", 1256);

	[Export]
	public bool usePDANet = false;

	public Room _room = new Room();

	public bool isAlive = false;

	public Rendezvous(){
		_self.Name = "Self";
		AddChild(_self);
		_room.Name = "Room";
		AddChild(_room);
		//Connect();
	}

	public void Connect(){
		if(usePDANet){
			_tcp.Bind(1255, "192.168.86.31");
		}
		Error err = _tcp.ConnectToHost(_IP, _PORT);
		if(err != Error.Ok){
			GD.Print("Error connecting to Rendezvous server");
		}else{
			GD.Print("Connected to Rendezvous server");
			GD.Print("Local Port: "+_tcp.GetLocalPort());
			_self._port = _tcp.GetLocalPort();
		}
	}

	public override void _Process(double delta)
	{

		if(_tcp.GetStatus() == StreamPeerTcp.Status.Connected){
			_tcp.Poll();
			while(_tcp.GetAvailableBytes() > 0){
				string packet = _tcp.GetUtf8String();
				handleData<object,object>(packet);
			}
		}
		

		while(_udp.GetAvailablePacketCount() > 0){
			string packet = _udp.GetPacket().GetStringFromUtf8();
			GD.Print("Recieved packet: "+packet);
		}
	}




	public void handleData<T,K>(string data){
		GD.Print("Recieved msg: "+ data);




		// if(isAlive == false && data != "OK"){
		// 	//Server responded to our connection
		// 	//Request server status
		// 	sendToRendezvous("status");
		// }
		// if(data == "OK"){
		// 	//Rendezvous Server is alive
		// 	isAlive = true;
		// }
		// if(data.StartsWith("id")){
		// 	//Server responded with our ID
		// 	_self._id = data.Split(':')[1];
		// 	_self._port = data.Split(':')[2].ToInt();
		// }
		// if(data.StartsWith("lR")){
		// 	//List of rooms
		// 	string[] rooms = data.Split(':');
		// 	foreach(string room in rooms){
		// 		Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary) Json.ParseString(room);
		// 		GD.Print(dict);
		// 	}
		// } else if(data.StartsWith("cJ")){
		// 	//Client joined room
		// 	string[] client = data.Split(":;");

		// 	Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary) Json.ParseString(client[1]);

		// 	GD.Print("Client joined room: "+dict["id"]);

		// 	Peer peer = new Peer((string) dict["id"], "", (string) dict["ip"], int.Parse((string) dict["port"]));
		// 	_room.addClient(peer);

		// } else if(data.StartsWith("rR")){
		// 	string[] strings = data.Split(":");

		// 	_room.SetHost(Peer.deserialize(_self.Serialize()));
		// 	_room._id = strings[1];
		// 	_room._name = "My Room";
		// 	_room.Name = "Room_"+_room._id;
		// 	_self.Name = "(Host)Self_"+_self.GetId();

		// } else if(data.StartsWith("jR")){
		// 	//This should be a client only functon
		// 	//If host. Close the room and join the new room

		// 	string[] strings = data.Split(":;");

		// 	Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary) Json.ParseString(strings[1]);

		// 	GD.Print("Joined room: "+dict["id"]);

		// 	_room.SetHost(Peer.deserialize(dict["host"].AsString()));
		// 	_room._id = dict["id"].AsString();
		// 	_room._name = dict["name"].AsString();
		// 	_room._clients = new List<Peer>();
		// 	_room.Name = "Room_"+_room._id;
		// 	_self.Name = "(Peer)Self_"+_self.GetId();
		// 	GD.Print(dict["clients"].AsGodotArray());
		// 	foreach(string clientString in dict["clients"].AsGodotArray()){
		// 		//GD.Print("Derseriliazing client: "+client);
		// 		Peer client = Peer.deserialize(clientString);
		// 		_room.addClient(client);

		// 	}

		// } else if(data.StartsWith("cR")){
		// 	string clientID = data.Split(":")[1];
		// 	GD.Print("Client left room: "+clientID);
		// 	_room.removeClient(_room._clients.Find(x => x.GetId() == clientID));
		// } else if(data.StartsWith("rC")){
		// 	GD.Print("Host requested a p2p connection");
		// 	establishP2P();
		// } else if(data == "roomRemoved"){
		// 	//The host left the room
		// 	//Leave the room
		// 	leaveRoom();
		// }
	}


	/**
	? Create a room on the Rendezvous server
	*/
	public void createRoom(string roomName){
		sendToRendezvous("rR:"+roomName); //Request Room
	}

	public void joinRoom(string roomName){
		sendToRendezvous("jR:"+roomName); //Join Room
	}

	public void leaveRoom(){
		sendToRendezvous("lR"); //Leave Room
		_room.leave();
		_self.Name = "Self";
	}

	

	public void establishP2P(){
		_udp.Bind(_self._port);
		GD.Print("Bound to port: "+_self._port);
		//Check if we are the host
		if(_room._host.GetId() == _self.GetId()){
			//We are the host
			//Establish a connection to each client
			foreach(Peer client in _room._clients){
				client.Connect(_udp);
			}
		} else {
			//We are a client
			//Establish a connection to the host
			_room._host.Connect(_udp);
		}
	}

	//Called by the host to request a P2P connection to a client
	public void requestP2P(){
		sendToRendezvous("rC");
	}

	public void sendToRendezvous(string data){

		_tcp.PutData(data.ToUtf8Buffer());

	}

}
