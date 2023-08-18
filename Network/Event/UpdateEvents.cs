using Godot;

public partial class UpdateEvents : Node
{


    [Signal]
    public delegate void RequestLatestXMLEventHandler();


    public static UpdateEvents instance = null;

    public override void _EnterTree()
    {
        Name = "UpdateEvents";
        instance ??= this;
        GD.Print("Registered: " + Name);
    }


    public override void _Ready()
    {
        

        Network.instance.OnEvent += ((string name, Variant data) => {
            if(name == "RequestLatestXML"){
                EmitSignal(UpdateEvents.SignalName.RequestLatestXML, data.AsGodotDictionary());
            }
        });

        Network.instance.SendRendezvous += (string eventName, Variant data) => {
            GD.Print($"{eventName} {data}");
            if(eventName == UpdateEvents.SignalName.RequestLatestXML){
                EmitSignal(Network.SignalName.OnSendRendezvous, "RequestLatestXML", data);
            }
        };


    }

}