using Godot;
using System.IO;

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

    public static void CopyDir(string path, string output)
    {
        DirAccess.MakeDirRecursiveAbsolute(output);

        DirAccess dir = DirAccess.Open(path);

        foreach (string item in dir.GetFiles())
        {
            DirAccess.CopyAbsolute(path+"/"+item, output+"/"+item);
            GD.Print("Copping " + path + "/" + item + " to " + output + "/" + item);
        }
        foreach (string item in dir.GetDirectories())
        {
            if(item.StartsWith("."))
            {
                GD.Print("Skipping " + item);
                continue;
            }
            CopyDir(path + "/" + item, output + "/" + item);
        }
    }

    public static void removeDirRecursive(string path)
    {
        if(path.StartsWith("res://"))
        {
            path = ProjectSettings.GlobalizePath(path);
        }

        Directory.Delete(path, true);
    }
}
