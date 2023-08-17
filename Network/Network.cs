using Godot;
using Godot.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class Network : Node {



	[Export]
	public int defaultPort = 1257;

	public Upnp upnp = new Upnp();

	public bool upnpAvailable = false;
	public bool rendezvousAvailable = false;
	public bool portAvailable = false;

	[Signal]
	public delegate void OnChecksCompletedEventHandler();

	[Signal]
	public delegate void OnEventEventHandler(string eventName, Variant data);

	[Signal]
	public delegate void SendRendezvousEventHandler(string eventName, Variant data);

	[Signal]
	public delegate void OnSendRendezvousEventHandler(string eventName, Variant data);

	public StreamPeerTcp tcp = new StreamPeerTcp();


	public static Network instance = null;

	public static bool killSwitch = true;


	public override void _EnterTree()
	{
		base._EnterTree();
		instance ??= this;
	}

	public override void _Ready()
	{
		Setup();
		Connect();

		if(!killSwitch) doChecks();
		
		OnChecksCompleted += Finshed;


		EmitSignal(SignalName.SendRendezvous, "status", "TESTS");
	}


	public void Setup(){
		//Setup EventHandler
		AddChild(new EventHandler());


		DefaultEvents.instance.STATUS += (string status) => {
			GD.Print("Status: "+status);
		};
		OnSendRendezvous += (string name, Variant data) => _SendRendezvous(name, data);
	}

	public void Connect(){
		tcp.ConnectToHost("disaac1.com", 1256);
	}

	public override void _Process(double delta)
	{
		tcp.Poll();
		if(tcp.GetStatus() == StreamPeerTcp.Status.Connected)
		{
            while (tcp.GetAvailableBytes() > 0)
            {
                string packet = tcp.GetUtf8String();
                handleData(packet);
            }
        }
		if (killSwitch)
		{
			SetProcess(false);
		}
			
	}

	public void handleData(string packet){

		Dictionary dict = (Dictionary) Json.ParseString(packet);
		//Example packet  {"event":"status","data":"OK","requestID":-1}
		if(!dict.ContainsKey("event")){
			GD.Print("Invalid packet");
			return;
		}
		if(!dict.ContainsKey("data")){
			GD.Print("Invalid packet");
			return;
		}

		GD.Print("Event: "+dict["event"]+" Data: "+dict["data"]);
		EmitSignal(Network.SignalName.OnEvent, dict["event"], dict["data"]);

	}


	public async void doChecks(){

        System.Collections.Generic.List<Task> tasks = new System.Collections.Generic.List<Task>
        {
            Task.Run(async () =>
            {
                await CheckIfRendezvousOpen();
                await CheckIfPortOpen();
            }),
            CheckUpnp()
        };


		await Task.WhenAny(Task.WhenAll(tasks), Task.Run(() =>
		{
			while(true)
			{
				if (killSwitch)
				{
					GD.Print("Killing tests");
					return;
				}

			}
		}));

		if(killSwitch) return;
		EmitSignal(Network.SignalName.OnChecksCompleted);


	}

	public void _SendRendezvous(string eventName, Variant data){

		// Example
		// {event:"name", data:DATA, requestID: -1}

		Dictionary dict = new Dictionary();
		dict.Add("event", eventName);
		dict.Add("data", data);
		dict.Add("requestID", -1);

		tcp.PutData(Json.Stringify(dict).ToUtf8Buffer());
		GD.Print("Sent to rendezvous: "+ eventName);

	}

	public static void EmitRendezvous(string eventName, Variant data){
		instance.EmitSignal(SignalName.SendRendezvous, eventName, data);
	}

	public static void RequestRendezvous(string eventName, Variant data)
	{
		instance.EmitSignal(SignalName.SendRendezvous, data);
	}

	public async Task CheckIfRendezvousOpen(){
		//Todo convert to propper event
		tcp.PutData("status".ToUtf8Buffer());

		//On a seperate thread poll the tcp socket

		await Task.WhenAny(
			Task.Delay(3000),
			Task.Run(() => {
				while (true) {
					tcp.Poll();
					if (tcp.GetStatus() == StreamPeerTcp.Status.Connected)
					{
						if(tcp.GetAvailableBytes() > 0){
							GD.Print("Rendezvous is open");
							rendezvousAvailable = true;
							break;
						}
					}
				}
			})
		);
	}

	public async Task CheckIfPortOpen(){

		//Check if the default is already open on the client
		GD.Print("Checking if port "+defaultPort+" is open");
		
		PacketPeerUdp socket = new PacketPeerUdp();

		Error err = socket.Bind(defaultPort);
		if(err != Error.Ok){
			GD.Print("Port "+defaultPort+" is already used");
			return;
		}
		//tcp.PutData(("checkPort:"+defaultPort).ToUtf8Buffer());
		_SendRendezvous("checkPort", defaultPort);

		await Task.WhenAny(Task.Delay(4000), Task.Run(() => {
			try
			{
				GD.Print("Waiting for response");
				while (true)
				{
					Task.Delay(100);
					if (socket.GetAvailablePacketCount() > 0)
					{
						GD.Print("Port " + defaultPort + " is open");
						portAvailable = true;
						break;
					}

				}
			} catch (ObjectDisposedException)
			{
				GD.Print("Socket was disposed... most likely due to user closing");
			}
		}));

		if(!portAvailable){
			GD.Print("Port "+defaultPort+" is not open");
		}
		socket.Close();
	}

	public async Task CheckUpnp(){
		GD.Print("Checking UPNP");
		Error err = await Task.Run<Error>(() => (Error) upnp.Discover());
		if(err == Error.Ok){
			GD.Print("UPNP is available");
			upnpAvailable = true;
		}
	}


	public void Finshed(){
		GD.Print("Checks completed");
	}
   




}
