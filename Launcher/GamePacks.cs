using Godot;
using Godot.Collections;

public partial class GamePacks : Node
{


    public static GamePacks instance;

    public static Array<GamePack> packs = new Array<GamePack>();

    public override void _EnterTree()
    {
        instance ??= this;
    }

    public override void _Ready()
    {
        //Check local packs
        readDevPacks();
        //Check downloaded packs
        readPacks();
        //Check online packs
        checkOnlinePacks();
        
    }


    public GamePack getPack(string name)
    {
        foreach(GamePack pack in packs)
        {
            if(pack.packName == name)
            {
                return pack;
            }
        }
        return null;
    }

    public void checkOnlinePacks()
    {
        //Download the repo file
        DiHub.get("DiLauncher/repo.json", "user://repo.json");

        //Read the repo file
        FileAccess file = FileAccess.Open("user://repo.json", FileAccess.ModeFlags.Read);
        string jsonString = file.GetAsText();
        Dictionary dict = Json.ParseString(jsonString).AsGodotDictionary();

        foreach(Dictionary packDict in dict["packs"].AsGodotArray())
        {
            string name = packDict["name"].ToString();
            string latest = packDict["latest"].ToString();
            string id = packDict["id"].ToString();
            string description = packDict["description"].ToString();

            GamePack pack = GamePack.createOnline(id);

            pack.latestVersion = latest;
            if(!pack.downloaded)
            {
                pack.packName = name;
                packs.Add(pack);
                GD.Print("Online Pack registerd: ", pack.packName);
            }
            
        }
    }

    public void readPacks()
    {
        if (OS.HasFeature("editor"))
        {
            GD.Print("User packs are disabled in of the editor");
            return;
        }

        GD.Print("Discovering Game Packs in Data folder");
        //Read folders and or .pak's in DevGame "user://"
        if(!DirAccess.DirExistsAbsolute("user://Data"))
        {
            DirAccess.MakeDirAbsolute("user://Data");
        }
        DirAccess devFolder = DirAccess.Open("user://Data");

        string[] dirs = devFolder.GetDirectories();

        foreach (string dir in dirs)
        {
            DirAccess subFolder = DirAccess.Open("user://Data/" + dir);
            if (subFolder.FileExists("pack.json"))
            {
                FileAccess file = FileAccess.Open("user://Data/" + dir + "/pack.json", FileAccess.ModeFlags.Read);

                string jsonString = file.GetAsText();

                GamePack pack = GamePack.deserilize(dir, jsonString);

                if (!pack.dev)
                {
                    GamePacks.packs.Add(pack);
                    GD.Print("Pack registerd: ", pack.packName);
                }

            }

        }
    }

    public void readDevPacks()
    {

        if(!OS.HasFeature("editor"))
        {
            GD.Print("Dev packs are disabled out of the editor");
            return;
        }

        GD.Print("Discovering Game Packs in Data folder");
        //Read folders and or .pak's in DevGame "res://"
        DirAccess devFolder = DirAccess.Open("res://Data");

        string[] dirs = devFolder.GetDirectories();

        foreach (string dir in dirs)
        {
            DirAccess subFolder = DirAccess.Open("res://Data/" + dir);
            if (subFolder.FileExists("pack.json"))
            {
                FileAccess file = FileAccess.Open("res://Data/" + dir + "/pack.json", FileAccess.ModeFlags.Read);

                string jsonString = file.GetAsText();

                GamePack pack = GamePack.deserilizeDev(dir, jsonString);

                GamePacks.packs.Add(pack);
                GD.Print("Dev Pack registerd: ", pack.packName);

            }

        }
    }

}