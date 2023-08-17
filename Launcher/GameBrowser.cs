using Godot;

public partial class GameBrowser : Control
{
	public bool devMode = true;

	public GamePack selectedPack = null;

	public override void _Ready()
	{
		displayPacks();

		GetNode<ItemList>("gameList").ItemSelected += onItemClicked;
		GetNode<Button>("panel/run").Pressed += runPack;
		GetNode<Button>("panel/install").Pressed += () => selectedPack.downloadLatest();
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
		GD.Print(pack.Serialize());
		GetNode<Label>("info/name").Text = pack.packName;
		GetNode<Label>("info/version").Text = "Installed: " + pack.packVersion + "\nLoaded: " +pack.loaded + "\nOnline: "+((pack.latestVersion==null)?"None":pack.latestVersion);
        GetNode<Label>("info/description").Text = pack.packPath;
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
