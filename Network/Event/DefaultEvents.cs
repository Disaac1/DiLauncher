using Godot;

public partial class DefaultEvents : Node {



    /**
    * Possible responses OK, ERROR, FULL, BAD
    */
    [Signal]
    public delegate void STATUSEventHandler(string status);

    [Signal]
    public delegate void IDEventHandler(string id);

    [Signal]
    public delegate void ListRoomsEventHandler(string[] rooms);

    [Signal]
    public delegate void CheckPortEventHandler(int port);

    public static DefaultEvents instance = null;

    public override void _EnterTree()
    {
        Name = "DefaultEvents";
        instance ??= this;
    }

    public override void _Ready()
    {
        Network.instance.OnEvent += ((string name, Variant data) => {
            GD.Print("DefaultEvents: "+name+" "+data);
            if(name == "status"){
                EmitSignal(DefaultEvents.SignalName.STATUS, data.ToString());
            }
        });

        Network.instance.SendRendezvous += (string eventName, Variant data) => {
            if(eventName == DefaultEvents.SignalName.STATUS){
                EmitSignal(Network.SignalName.OnSendRendezvous, "status", data);
            }
            if(eventName == DefaultEvents.SignalName.CheckPort){
                EmitSignal(Network.SignalName.OnSendRendezvous, "checkPort", data);
            }
        };
    }


}