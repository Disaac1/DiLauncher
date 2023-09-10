using System;
using Godot;
using Godot.Collections;

public class Player
{

    private string _id = "";

    public string name = "";

    public Texture2D avatar = null;


    public Player(string id)
    {
        _id = id;

        loadAvatar();
    }

    public void loadAvatar()
    {
        string path = "avatars/" + _id + ".webp";

        try{
            Image res = (Image) DiHub.load(path);
            GD.Print("Loaded avatar: " + res + " filetype: " + res.GetType());
            avatar = ImageTexture.CreateFromImage(res);
        } catch(Exception err)
        {
            GD.Print("Error loading avatar: " + err.Message);
            return;
        }
    }

    public Player deserialize(string s)
    {
        GD.Print("Deserializing Player: " + s);

        Dictionary dict = (Dictionary)Json.ParseString(s);

        Player player = new Player(dict["id"].AsString());
        player.name = dict["name"].AsString();

        return player;

    }

    public string GetID()
    {
        return _id;
    }

}