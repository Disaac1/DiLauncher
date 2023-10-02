using System;
using System.IO;
using Godot;
using Godot.Collections;

public partial class Player : Resource
{

	private string _id = "";

	public Peer peer;

	private string _name = "";

	public string name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			Sync();
		}
	}

	public Texture2D avatar = null;


	public Player(string id)
	{
		_id = id;

		loadAvatar();
	}

	public void loadAvatar()
	{

		if(_id == null) return;
		if(avatar != null) return;

		string path = "avatars/" + _id + ".webp";
		try{
			Image res = (Image) DiHub.load(path);
			if(res == null)
			{   
				GD.Print("Error loading avatar: " + path);
				return;
			};
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
		Dictionary dict = (Dictionary) Json.ParseString(s);

		if(!dict.ContainsKey("id")) return null;

		Player player = new Player(dict["id"].AsString())
		{
			_name = dict["name"].AsString()
		};


		if(dict.ContainsKey("avatar"))
		{
			Image image = new Image();
			image.LoadWebpFromBuffer(Convert.FromBase64String(dict["avatar"].AsString()));
			player.avatar = ImageTexture.CreateFromImage(image);
		}
		

		
		return player;

	}

	public static Player LoadFile(string path)
	{
		if(!Godot.FileAccess.FileExists(path)) return null;
		
		Godot.FileAccess file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		file.Close();

		Player player = deserialize(json);

		if(player == null)
		{
			GD.Print("Error loading player from file: " + path);
			return null;
		}
		return player;
	}

	public string ToJson()
	{
		Dictionary dict = new();

		if(avatar != null)
		{
			byte[] image = avatar.GetImage().SaveWebpToBuffer(true, 0.75f);
			string imageString = Convert.ToBase64String(image);
			dict.Add("avatar", imageString);
		}
		
		dict.Add("id", _id);
		dict.Add("name", name);
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
		GD.Print("Syncing player");
		EmitSignal(DefaultEvents.SignalName.UpdatePlayer, this);
	}

	public void Save()
	{
		string path = "user://" + "player" + ".json";
		Godot.FileAccess file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Write);
		string json = ToJson();
		string pretty = Util.prettyJSON(json);
		file.StoreString(pretty);
		file.Close();
	}

	

}
