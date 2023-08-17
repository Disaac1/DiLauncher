using Godot;
using Godot.Collections;
using System;

public partial class GamePack : Resource
{

	/**
	 * <summary>
	 * Name of the pack
	 * </summary>
	 */
	public string packName;

	/** <summary> Version of the pack.
	* Must be in the major.minor.revision format
	* </summary>
	*/
	public string packVersion;

	/**
	 * <summary> Id of pack matches folder or .pck name</summary>
	 */
	public readonly string id;

	public Texture2D icon = null;

	public readonly string path;

	public bool loaded = false;

	public bool isPack = true;

	public GamePack(string id)
	{
		this.id = id;

		//Check if dev folder or .pck
		if (FileAccess.FileExists("res://Data/" + id + "/pack.json"))
		{
			path = "res://Data/" + id;
		} else
		{
			path = "user://Data/" + id;
		}

        if (!FileAccess.FileExists(path + "/pack.pck") && !FileAccess.FileExists(path + "/pack.zip"))
        {
            loaded = true;
			isPack = false;
        }

		//Pack needs to be loaded
		if (OS.HasFeature("Editor") && !loaded)
		{
			GD.Print("Pack loaded in engine thar be dragons");
		}

	}

	public static GamePack deserilize(string id, string data)
	{

		GD.Print("Loading pack with id: ", id);
		GD.Print(data);
		Dictionary dict = Json.ParseString(data).AsGodotDictionary();

		GamePack pack = new GamePack(id);
		pack.packName = dict["name"].AsString();
		pack.packVersion = dict["version"].AsString();

		GD.Print("Path is ", pack.path);
		if (FileAccess.FileExists(pack.path+"/icon.png"))
		{
			GD.Print("Icon found at " + pack.path + "/icon.png");
			//pack.icon = GD.Load<Texture2D>(pack.path + "/icon.png");
		}


		return pack;
		
	}


	public void run()
	{
		GD.Print("Running Gamepack " + id);
		if(!loaded)
		{
			GD.PrintErr("Loading pck's in editor isn't supported... Workaround later");
			return;
		}
		/**if (!loaded)
		{
			GD.Print("Loading .pck");
			bool success = ProjectSettings.LoadResourcePack(path + "/pack.pck");
		
			if (!success)
			{
				GD.PrintErr("Failed to load pack");
				return;
			}

			Util.Tree();
		}*/
        Network.instance.GetTree().ChangeSceneToFile(path+"/main.tscn");
    }

}
