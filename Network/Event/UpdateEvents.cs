using Godot;

public partial class UpdateEvents : Node
{


    [Signal]
    public delegate void RequestLatestXMLEventHandler();


    public static UpdateEvents instance = null;

    public override void _EnterTree()
    {
        Name = "DefaultEvents";
        if(instance == null){
            instance = this;
        }
    }


    public override void _Ready()
    {
        

        Network.instance.OnEvent += ((string name, Variant data) => {
            GD.Print("UpdateEvents: "+name+" "+data);
            if(name == "RequestLatestXML"){
                EmitSignal(UpdateEvents.SignalName.RequestLatestXML, data.AsGodotDictionary());
            }
        });

        Network.instance.SendRendezvous += (string eventName, Variant data) => {
            if(eventName == UpdateEvents.SignalName.RequestLatestXML){
                EmitSignal(Network.SignalName.OnSendRendezvous, "RequestLatestXML", data);
            }
        };


    }

}