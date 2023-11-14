using Godot;

public partial class launcher_network : Node
{

	[Export]
	private Label statusLabel;


	public override void _Ready()
	{
		statusLabel ??= GetNode<Label>("status");
	}

	public override void _Process(double delta)
	{
		/**statusLabel.Text = 
			"Network Status: " + (Network.instance.rendezvousAvailable ? "Ready" : "Offline")
			+ "\n  UPnP Status: " + (Network.instance.upnpAvailable ? "Ready" : "Offline")
			+ "\n  Port Status: " + (Network.instance.portAvailable ? "Ready" : "Offline")
			+ "\n  ID: " + Network.instance.rendezvous.getSelf().GetId()
			+ "\n  Room: " + Network.instance.rendezvous.getRoom()._id;
			;*/

	}



}
