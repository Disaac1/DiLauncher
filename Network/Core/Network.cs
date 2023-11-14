using System;
using Godot;
using Godot.Collections;

public partial class Network : Node
{


	public StreamPeerTcp tcp = new();

	public static Network instance;


	public override void _EnterTree()
	{
		instance ??= this;

        GodotLogger.info("Initializing Events");
        //Initialize all events
        EventInitializer.InitializeAllEvents();
	}

	public override void _Ready()
	{
		ConnectToRendezvous();
	}

	public void ConnectToRendezvous()
	{
		tcp.ConnectToHost("disaac1.com", 1256);
		
		
	}

	public void Login()
	{
		string id;
		//Read unique ID from file if it exists else create one and save it
		if(FileAccess.FileExists("user://secret.txt"))
		{
			FileAccess file = FileAccess.OpenEncrypted("user://secret.txt", FileAccess.ModeFlags.Read, "password".ToUtf8Buffer());

			id = file.GetAsText();

			//Send id to the server
		}
		else
		{
			//Generate a random id
			id = Guid.NewGuid().ToString();

			//Send id to the server
		}

		DefaultEvents.LOGIN.Send("0", id, (player) => {
			GodotLogger.info(player.ToString());
		});

	}

	public override void _Process(double delta)
	{
		tcp.Poll();
		if(tcp.GetStatus() == StreamPeerTcp.Status.Connected)
		{
			while(tcp.GetAvailableBytes() > 0)
			{
				string packet = tcp.GetUtf8String();
				handleData(packet);
			}
		}
	}


	public void handleData(string data)
	{
		GodotLogger.info(data);
		Dictionary dict = (Dictionary) Json.ParseString(data);

		if(!dict.ContainsKey("type"))
		{
			GodotLogger.warn("Invalid packet: Missing Type");
		}

		string type = (string) dict["type"];

		string name;
		Variant dataMessage;
		if(type == "pass"){
			//Pass Event no routing done through clients
				//confirm packet is for self
				if(!dict.ContainsKey("dest"))
				{
					GodotLogger.warn("Invalid pass packet: Missing Dest");
					GodotLogger.info(data);
					return;
				}
				if((string) dict["dest"] != "self" && false)//TODO: Hand client self id to be able to check for this
				{
					GodotLogger.warn("Invalid pass packet: Dest not self");
					GodotLogger.info(data);
					return;
				}

				if(!dict.ContainsKey("name"))
				{
					GodotLogger.warn("Invalid pass packet: Missing Event Name");
					GodotLogger.info(data);
					return;
				}
				if(!dict.ContainsKey("data"))
				{
					GodotLogger.warn("Invalid pass packet: Missing Data");
					GodotLogger.info(data);
					return;
				}

				name = (string) dict["name"];
				dataMessage = dict["data"];

				IEvent @event = Registry.GetEvent(name);

				if(@event == null)
				{
					GodotLogger.warn("Invalid pass packet: Event not found");
					GodotLogger.info(data);
					return;
				}

				@event._emit(dataMessage.AsString(), "");
				return;
		}
		else if(type == "requst")
		{
			if(!dict.ContainsKey("dest"))
				{
					GodotLogger.warn("Invalid Request Packet: Missing Dest");
					GodotLogger.info(data);
					return;
				}
				if((string) dict["dest"] != "self" && false)
				{
					GodotLogger.warn("Invalid Request Packet: Dest not self");
					GodotLogger.info(data);
					return;
				}
				if(!dict.ContainsKey("name"))
				{
					GodotLogger.warn("Invalid Request Packet: Missing Event Name");
					GodotLogger.info(data);
					return;
				}
				if(!dict.ContainsKey("data"))
				{
					GodotLogger.warn("Invalid Request Packet: Missing Data");
					GodotLogger.info(data);
					return;
				}

				name = (string) dict["name"];
				dataMessage = dict["data"];

				Registry.GetEvent(name)._emit(dataMessage.AsString(), dict["guid"].AsString());
				return;
		}

	}

	public static void SendData(Dictionary msg)
	{
		instance.tcp.PutData(Json.Stringify(msg).ToUtf8Buffer());
	}


}
