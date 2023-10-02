using Godot;
using System;
using System.IO;
using System.Linq;

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

    public static string prettyJSON(string json, string indent = "    ")
    {

        int indentation = 0;
        int quoteCount = 0;
        int escapeCount = 0;
        var result =
        from ch in json ?? string.Empty
        let escaped = (ch == '\\' ? escapeCount++ : escapeCount > 0 ? escapeCount-- : escapeCount) > 0
        let quotes = ch == '"' && !escaped ? quoteCount++ : quoteCount
        let unquoted = quotes % 2 == 0
        let colon = ch == ':' && unquoted ? ": " : null
        let nospace = char.IsWhiteSpace(ch) && unquoted ? string.Empty : null
        let lineBreak = ch == ',' && unquoted ? ch + System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, indentation)) : null
        let openChar = (ch == '{' || ch == '[') && unquoted ? ch + System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, ++indentation)) : ch.ToString()
        let closeChar = (ch == '}' || ch == ']') && unquoted ? System.Environment.NewLine + string.Concat(Enumerable.Repeat(indent, --indentation)) + ch : ch.ToString()
        select colon ?? nospace ?? lineBreak ?? (
            openChar.Length > 1 ? openChar : closeChar
        );

        return String.Concat(result);
    }
}