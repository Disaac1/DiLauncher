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
        readDevPacks();
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
        string[] files = devFolder.GetFiles();

        foreach (string dir in dirs)
        {
            DirAccess subFolder = DirAccess.Open("res://Data/" + dir);
            if (subFolder.FileExists("pack.json"))
            {
                FileAccess file = FileAccess.Open("res://Data/" + dir + "/pack.json", FileAccess.ModeFlags.Read);

                string jsonString = file.GetAsText();

                GamePack pack = GamePack.deserilize(dir, jsonString);

                GamePacks.packs.Add(pack);

            }

        }
    }

}