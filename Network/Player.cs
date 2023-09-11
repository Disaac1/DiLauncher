using System;
using System.IO;
using Godot;
using Godot.Collections;

public partial class Player : Resource
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

    public static Player deserialize(string s)
    {
        GD.Print("Deserializing Player: " + s);

        Dictionary dict = (Dictionary)Json.ParseString(s);
        Image image = new Image();
        image.LoadWebpFromBuffer(Convert.FromBase64String(dict["avatar"].AsString()));

        Player player = new Player(dict["id"].AsString())
        {
            name = dict["name"].AsString(),
            avatar = ImageTexture.CreateFromImage(image)
        };

        return player;

    }

    public string ToJson()
    {
        byte[] image = avatar.GetImage().SaveWebpToBuffer(true, 0.75f);
        string imageString = Convert.ToBase64String(image);

        Dictionary dict = new Dictionary();
        dict.Add("id", _id);
        dict.Add("name", name);
        dict.Add("avatar", imageString);
        return Json.Stringify(dict);
    }

    public string GetID()
    {
        return _id;
    }


    /**
    *  <summary>Sync player data to the server</summary>
    */ 
    public void Sync()
    {
        EmitSignal(DefaultEvents.SignalName.UpdatePlayer, this);   
    }

}