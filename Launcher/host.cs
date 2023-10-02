using Godot;
using System;

public partial class host : Control
{


	public override void _Ready()
	{
		Network.instance.rendezvous.CreateRoom("lobby");


		RendezvousEvents.instance.CreateRoom += (room) => {

			GetNode<Label>("lobby/code").Text = Network.instance.rendezvous.getRoom()._id;



		};

		RendezvousEvents.instance.ClientJoinedRoom += (client) => {

			

		};
	}


	public void addPlayer(Peer client)
	{
		
	}
	


}
