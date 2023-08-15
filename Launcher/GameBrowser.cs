using Godot;
using Godot.Collections;
using System;

public partial class GameBrowser : Control
{
	public bool devMode = true;

	public GamePack selectedPack = null;

	public override void _Ready()
	{
		displayPacks();

		GetNode<ItemList>("gameList").ItemSelected += onItemClicked;
		GetNode<Button>("panel/run").Pressed += runPack;
	}

	public void runPack()
	{
		if (selectedPack == null)
		{
			return;
		}
		GD.Print("running");
		selectedPack.run();
	}

	public void onItemClicked(long index)
	{
		GamePack pack = GamePacks.packs[(int)index];

		selectedPack = pack;
		updateInfo(pack);
	}

	public void updateInfo(GamePack pack)
	{
		GetNode<Label>("info/name").Text = pack.packName;
		GetNode<Label>("info/version").Text = "Installed: " + pack.packVersion + "\nLoaded: " +pack.loaded;
        GetNode<Label>("info/description").Text = pack.path;
        GetNode<TextureRect>("info/icon").Texture = pack.icon;
	}


	public void displayPacks()
	{

		ItemList itemList = GetNode<ItemList>("gameList");
		foreach(GamePack pack in GamePacks.packs)
		{
			itemList.AddItem(pack.packName, pack.icon);
		}
	}

}
