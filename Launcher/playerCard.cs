using Godot;
using System;

public partial class playerCard : Control
{

	[Export]
	public bool IsEditable = false;


	public override void _Ready()
	{
		GetNode<LabelEdit>("name").TextSubmitted += updateName;
		GetNode<LabelEdit>("name").readOnly = !IsEditable;
	}


	public void updateName(string text)
	{
		if(IsEditable)
		{
			GD.Print("Setting name to: " + text);
			player.SetName(text);
		}
	}

	public PlayerNetwork player = null;

	public void setPlayer(PlayerNetwork player)
	{
		this.player = player;
		updateUI();
	}

	public void updateUI()
	{
		if(player == null)
		{
			return;
		}
		if(player.avatar != null)
		{
			GetNode<TextureRect>("icon").Texture = player.avatar;
		}
		if(player.Name != null)
		{
			GetNode<Label>("name").Text = player.Name;
		}
		QueueRedraw();
	}

}
