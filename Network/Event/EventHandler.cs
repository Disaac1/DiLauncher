using Godot;


public partial class EventHandler : Node {


    public override void _Ready()
    {
        Name = "EventHandler";
        register(new DefaultEvents());
        register(new UpdateEvents());
        register(new PackEvents());
    }


    public void register(Node node){
        AddChild(node);
    }

}