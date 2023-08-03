using Godot;
using System;

public partial class networking_test : Node2D
{

    public Rendezvous rendezvous;


    public override void _Ready()
    {
        rendezvous = GetNode<Rendezvous>("/root/Rendezvous");

        GetNode<Button>("CanvasLayer/Control/createRoom").Pressed += onCreateRoom;
        GetNode<Button>("CanvasLayer/Control/joinRoom").Pressed += onJoinRoom;
        GetNode<Button>("CanvasLayer/Control/leaveRoom").Pressed += onLeaveRoom;
        GetNode<Button>("CanvasLayer/Control/connect").Pressed += establishP2P;
    }

    public override void _Process(double delta)
    {
        //Update the Rendezvous server label
        GetNode<Label>("CanvasLayer/Control/Label").Text 
            = "Rendezvous Server: " + rendezvous.isAlive + "\n"
            + "Room: " + rendezvous._room._name + "\n"
            + "Host: " + rendezvous._room._host.GetId() + "\n"
            + "Clients: " + rendezvous._room._clients.Count + "\n";
    
        tick();
    }

    public void onCreateRoom(){
        rendezvous.createRoom("Test Room");
    }

    public void onJoinRoom(){
        rendezvous.joinRoom("1234");
    }

    public void onLeaveRoom(){
        //rendezvous.leaveRoom();
        rendezvous.Connect();
    }


    public void establishP2P(){
        //rendezvous.requestP2P();
        GD.Print("Requsting P2P");
        testUDPConn();
    }



    private PacketPeerUdp _udp1 = new PacketPeerUdp();
    private PacketPeerUdp _udp2 = new PacketPeerUdp();
    public void testUDPConn(){



        _udp1.Bind(25561);
        _udp2.Bind(25562);
 
        GD.Print("1:"+_udp1.GetLocalPort());
        GD.Print("2:"+_udp2.GetLocalPort());

        _udp1.SetDestAddress("disaac1.com", 25562);
        _udp2.SetDestAddress("127.0.0.1", 25561);

        _udp1.PutPacket("Hello 1".ToUtf8Buffer());
        _udp2.PutPacket("Hello 2".ToUtf8Buffer());
    }

    public void tick(){
        while(_udp1.GetAvailablePacketCount() > 0){

            GD.Print("UDP1: "+_udp1.GetPacket().GetStringFromUtf8());
            GD.Print(_udp1.GetPacketPort());
        }
        while(_udp2.GetAvailablePacketCount() > 0){
            GD.Print("UDP2: "+_udp2.GetPacket().GetStringFromUtf8());
            GD.Print(_udp2.GetPacketPort());
        }
    }


}