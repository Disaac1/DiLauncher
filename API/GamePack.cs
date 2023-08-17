using Godot;
using Godot.Collections;
using System.IO.Compression;
using System.Linq;
using System.Net;

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
	public string packPath;

	public bool loaded = false;

	public bool downloaded = false;

	public string latestVersion;

	public bool online = false;
	public bool dev = false;

	private GamePack(string id, string packPath)
	{
        this.id = id;
        this.path = "res://Data/" + id + "/";
		this.packPath = packPath;
    }

	public static GamePack create(string id, string packPath)
	{
        GamePack old = GamePacks.packs.FirstOrDefault((GamePack p) => p.id == id, null);
        if (old != null)
        {
			return old;
        }

		GamePack pack = new GamePack(id, packPath);

		if (FileAccess.FileExists(packPath))
		{
			pack.downloaded = true;
		}

		return pack;
    }

	public static GamePack createDev(string id)
	{
        GamePack old = GamePacks.packs.FirstOrDefault((GamePack p) => p.id == id, null);
        if (old != null)
        {
            return old;
        }

        GamePack pack = new GamePack(id, "");

		pack.downloaded = true;
		pack.loaded = true;
		pack.dev = true;

		return pack;
    }

	public static GamePack createOnline(string id)
	{
        GamePack old = GamePacks.packs.FirstOrDefault((GamePack p) => p.id == id, null);
        if (old != null)
        {
			old.online = true;
            return old;
        }

        GamePack pack = new GamePack(id, "");
		pack.online = true;

        return pack;
    }

	public static GamePack deserilizeDev(string id, string data)
	{

		GD.Print("Loading pack with id: ", id);
		Dictionary dict = Json.ParseString(data).AsGodotDictionary();

		GamePack pack = createDev(id);
		pack.packName = dict["name"].AsString();
		pack.packVersion = dict["version"].AsString();
		return pack;
		
	}

    public static GamePack deserilize(string id, string data)
    {

        GD.Print("Loading pack with id: ", id);
        Dictionary dict = Json.ParseString(data).AsGodotDictionary();

        GamePack pack = create(id, "user://Data/"+id+"/pack.zip");
		if (pack.dev)
		{
			return pack;
		}
        pack.packName = dict["name"].AsString();
        pack.packVersion = dict["version"].AsString();
        return pack;

    }

    public void reloadPack()
	{
		//Read new json
		string data = FileAccess.GetFileAsString($"user://Data/{id}/pack.json");

		GD.Print("Reloading pack with id: ", id);

		Dictionary dict = Json.ParseString(data).AsGodotDictionary();

		packName = dict["name"].AsString();
		packVersion = dict["version"].AsString();

	}

	public void run()
	{

        if (OS.HasFeature("editor"))
        {
            GD.Print("Running Gamepack " + id);
            if (!loaded)
            {
                GD.PushError("Loading pck's in editor isn't supported... Workaround later");
                return;
            }
        }
		else
		{
            if (!loaded)
            {
                GD.Print("Loading .pck");
                bool success = ProjectSettings.LoadResourcePack(packPath);

                if (!success)
                {
                    GD.PrintErr("Failed to load pack");
                    return;
                }

                Util.Tree();
            }
        }
        Network.instance.GetTree().ChangeSceneToFile(path + "main.tscn");
    }

	public string Serialize()
	{
		Dictionary dict = new Dictionary();
		dict.Add("name", packName);
		dict.Add("version", packVersion);
		dict.Add("id", id);
		dict.Add("path", path);
		dict.Add("loaded", loaded);
		dict.Add("downloaded", downloaded);
		dict.Add("latest", latestVersion);

		return Json.Stringify(dict);
	}

	public void downloadLatest()
	{
		GD.Print("Downloading latest version of pack: "+id);
		if(packVersion == latestVersion)
		{
			GD.Print("Pack already up to date");
			return;
		}
		if (!online)
		{
			GD.Print("Pack isn't on the repo");
			return;
		}
		if(dev)
		{
			GD.Print("Dev pack can't be updated");
			return;
		}

		//Download the pack
		WebClient client = new WebClient();
		DiHub.get(id+"/"+id+"."+latestVersion+".zip", "user://"+id+".zip");

		//Extract the pack
		ZipFile.ExtractToDirectory(ProjectSettings.GlobalizePath($"user://{id}.zip"), ProjectSettings.GlobalizePath($"user://Data/{id}/"));

		DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath($"user://{id}.zip"));

		//Update the pack
		reloadPack();

		downloaded = true;
		packPath = $"user://Data/{id}/pack.zip";
	}

}
