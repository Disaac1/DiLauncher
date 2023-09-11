using Godot;
using System;

public partial class playerCard : Control
{

	[Export]
	public bool IsEditable
	{
		get
		{
			return GetNode<LabelEdit>("name").readOnly;
		}
		set
		{
			GetNode<LabelEdit>("name").readOnly = value;
		}
	}


	public override void _Ready()
	{
		GetNode<LabelEdit>("name").TextSubmitted += updateName;
	}


	public void updateName(string text)
	{
		if(IsEditable)
		{
			player.name = text;
		}
	}

	public Player player = null;

	public void setPlayer(Player player)
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
		if(player.name != null)
		{
			GetNode<Label>("name").Text = player.name;
		}
		QueueRedraw();
	}

}
