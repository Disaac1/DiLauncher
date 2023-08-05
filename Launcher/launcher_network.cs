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
        statusLabel.Text = "Network Status: " + (Network.instance.rendezvousAvailable ? "Ready" : "Offline");
    }



}