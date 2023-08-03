using Godot;


public partial class EventHandler : Node {


    public override void _Ready()
    {
        register(new DefaultEvents());
        register(new UpdateEvents());
    }


    public void register(Node node){
        AddChild(node);
        GD.Print("Registered: "+node.Name);
    }

}