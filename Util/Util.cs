using Godot;

public class Util
{
    public static void Tree(string path = "res://", int level = 1)
    {
        DirAccess dir = DirAccess.Open(path);

        foreach (string item in dir.GetFiles())
        {
            GD.Print("---------------------------------------".Substr(0,level)+item);
        }
        foreach (string item in dir.GetDirectories())
        {
            GD.Print("---------------------------------------".Substr(0,level)+item);
            if(item.StartsWith("."))
            {
                GD.Print("------------------------------------------".Substring(0, level+1) + " Skipping");
                continue;
            }
            Tree(path + "/" + item, level + 1);
        }
    }
}
