using Godot;
using Godot.Collections;

public partial class DiscordRichPressence : Node
{

	private static Node _wrapper = GD.Load<GDScript>("res://Network/DiscordRichPressence.gd").New().As<Node>();

	public static long app_id
	{
		get
		{
			return (long)_wrapper.Get("app_id");
		}
		set
		{
			GD.Print("Setting app_id to " + value);
			_wrapper.Set("app_id", value);
			
		}
	}

	public static bool instanced
	{
		get
		{
			return (bool)_wrapper.Get("instanced");
		}
		set
		{
			_wrapper.Set("instanced", value);
		}
	}

	public static string details
	{
		get
		{
			return ((string)_wrapper.Get("details"));
		}
		set
		{
			_wrapper.Set("details", value);
		}
	}

	public static string state
	{
		get
		{
			return ((string)_wrapper.Get("state"));
		}
		set
		{
			_wrapper.Set("state", value);
		}
	}

	public static string large_image
	{
		get
		{
			return ((string)_wrapper.Get("large_image"));
		}
		set
		{
			_wrapper.Set("large_image", value);
		}
	}

	public static string large_image_text
	{
		get
		{
			return ((string)_wrapper.Get("large_image_text"));
		}
		set
		{
			_wrapper.Set("large_image_text", value);
		}
	}

	public static string small_image
	{
		get
		{
			return ((string)_wrapper.Get("small_image"));
		}
		set
		{
			_wrapper.Set("small_image", value);
		}
	}

	public static string small_image_text
	{
		get
		{
			return ((string)_wrapper.Get("small_image_text"));
		}
		set
		{
			_wrapper.Set("small_image_text", value);
		}
	}

	public static double start_timestamp
	{
		get
		{
			return ((double)_wrapper.Get("start_timestamp"));
		}
		set
		{
			_wrapper.Set("start_timestamp", value);
		}
	}

	public static double end_timestamp
	{
		get
		{
			return ((double)_wrapper.Get("end_timestamp"));
		}
		set
		{
			_wrapper.Set("end_timestamp", value);
		}
	}

	public static string party_id
	{
		get
		{
			return ((string)_wrapper.Get("party_id"));
		}
		set
		{
			_wrapper.Set("party_id", value);
		}
	}

	public static int current_party_size
	{
		get
		{
			return ((int)_wrapper.Get("current_party_size"));
		}
		set
		{
			_wrapper.Set("current_party_size", value);
		}
	}

	public static int max_party_size
	{
		get
		{
			return ((int)_wrapper.Get("max_party_size"));
		}
		set
		{
			_wrapper.Set("max_party_size", value);
		}
	}

	public static string join_secret
	{
		get
		{
			return ((string)_wrapper.Get("join_secret"));
		}
		set
		{
			_wrapper.Set("join_secret", value);
		}
	}

	public static string spectate_secret
	{
		get
		{
			return ((string)_wrapper.Get("spectate_secret"));
		}
		set
		{
			_wrapper.Set("spectate_secret", value);
		}
	}

	public static string match_secret
	{
		get
		{
			return ((string)_wrapper.Get("match_secret"));
		}
		set
		{
			_wrapper.Set("match_secret", value);
		}
	}

	public static bool is_party_public
	{
		get
		{
			return ((bool)_wrapper.Get("is_party_public"));
		}
		set
		{
			_wrapper.Set("is_party_public", value);
		}
	}

	public static bool is_overlay_locked
	{
		get
		{
			return ((bool)_wrapper.Get("is_overlay_locked"));
		}
		set
		{
			_wrapper.Set("is_overlay_locked", value);
		}
	}

	public static void refresh()
	{
		_wrapper.Call("refresh");
	}

	public static void accept_invite(string user_id)
	{
		_wrapper.Call("accept_invite", user_id);
	}

	public static void clear()
	{
		_wrapper.Call("clear", false);
	}

	public static void clear(bool reset_values)
	{
		_wrapper.Call("clear", reset_values);
	}

	public static void unclear()
	{
		_wrapper.Call("unclear");
	}

	public static void regiser_command(string command)
	{
		_wrapper.Call("regiser_command", command);
	}

	public static void register_steam(int steam_id)
	{
		_wrapper.Call("register_steam", steam_id);
	}

	public static void accept_join_request(int user_id)
	{
		_wrapper.Call("accept_join_request", user_id);
	}

	public static void coreupdate()
	{
		_wrapper.Call("coreupdate");
	}

	public static void debug()
	{
		_wrapper.Call("debug");
	}
	
	public static Array get_all_relationships()
	{
		return ((Array)_wrapper.Call("get_all_relationships"));
	}

	public static Dictionary get_current_user()
	{
		return ((Dictionary)_wrapper.Call("get_current_user"));
	}

	public static bool get_overlay_enabled()
	{
		return ((bool)_wrapper.Call("get_overlay_enabled"));
	}

	public static void open_invite_overlay(bool is_spectate)
	{
		_wrapper.Call("open_invite_overlay", is_spectate);
	}

	public static void open_voice_settings()
	{
		_wrapper.Call("open_voice_settings");
	}

	public static bool get_is_discord_working()
	{
		return ((bool)_wrapper.Call("get_is_discord_working"));
	}

	public static int get_result_int()
	{
		return (int) _wrapper.Call("get_result_int");
	}

}
