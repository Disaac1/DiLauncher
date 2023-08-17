using Godot;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public partial class CreatePack : SceneTree
{

	public override async void _Initialize()
	{
		Network.killSwitch = true;


		List<Task> tasks = new List<Task>();

		foreach(GamePack p in GamePacks.packs)
		{
			if(!p.isPack)
			{
				tasks.Add(Task.Run(() => create(p)));
			}
		}

		GD.Print(tasks.Count + " packs being exported");
		await Task.WhenAll(tasks);

		GD.Print("Finished everything");
		Quit(0);
	}

    public static void create(GamePack pack)
	{
        //Create a thread to handle creation of pack

            setupWorkspace(pack);

            //Run cmd to pack godot folder to temp loacation
            int pid = OS.CreateProcess(".export/createPack.bat", new string[] { pack.id }, true);

            while (OS.IsProcessRunning(pid))
            {
                OS.DelayMsec(500);
            }

            GD.Print("Finished export of " + pack.id);

           removeWorkspace(pack);

	}

	public static void removeWorkspace(GamePack pack)
	{
		GD.Print(pack.id + " cleanup");

		DirAccess.RemoveAbsolute(pack.path + "/export_presets.cfg");
		DirAccess.RemoveAbsolute(pack.path + "/project.godot");
		DirAccess.RemoveAbsolute(pack.path + "/.godot");
	}

	public static void setupWorkspace(GamePack pack)
	{
		GD.Print(pack.id + " being packed");
		
		DirAccess.CopyAbsolute("res://.export/ProjectSettings/export_presets.cfg", pack.path + "/export_presets.cfg");

        DirAccess.CopyAbsolute("res://.export/ProjectSettings/project.godot", pack.path + "/project.godot");
    }


	public static void createConfig()
	{
		FileAccess config = FileAccess.Open("export.cfg", FileAccess.ModeFlags.Write);

		config.StoreLine("[preset.0]");
		config.StoreLine(new Preset().stringify());

		config.StoreLine("");
		config.StoreLine("[preset.0.options]");
		config.StoreLine(new PresetOptions().all);

		config.Flush();
		config.Free();
	}

	private class Preset
	{
		private string name = "PCK";
		private string platform = "Windows Desktop";
		private bool runnable = false;
		private bool dedicated_server = false;
		private string custom_features = "";
		private string export_filter = "resources";
		private string export_files = "";
		private string include_filter = "";
		private string exclude_filter = "Network/*, Launcher/*, icon.svg";
		private string export_path = ".export/temp.pck";
		private string encryption_include_filters = "";
		private string encryption_exlude_filters = "";
		private bool enrypt_pck = false;
		private bool encrypt_directory = false;

		public string stringify()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"name\"=\"" + name + "\"\n");
			sb.Append("\"platform\"" + platform + "\"\n");
			sb.Append("\"runnable\"=" + runnable + "\n");
			sb.Append("\"dedicated_server\"=" + dedicated_server + "\n");
			sb.Append("\"custom_features\"=\"" + custom_features + "\"\n");
			sb.Append("\"export_filter\"=\"" + export_filter + "\"\n");
			sb.Append("\"export_files\"=\"" + export_files + "\"\n");
			sb.Append("\"include_filter\"=\"" + include_filter + "\"\n");
			sb.Append("\"exclude_filter\"=\"" + exclude_filter + "\"\n");
			sb.Append("\"export_path\"=\"" + export_path + "\"\n");
			sb.Append("\"encryption_include_filters\"=\"" + encryption_include_filters + "\"\n");
			sb.Append("\"encryption_exlude_filters\"=\"" + encryption_exlude_filters + "\"\n");
			sb.Append("\"encrypt_pck\"=" + enrypt_pck + "\n");
			sb.Append("\"encrypt_directory\"=" + encrypt_directory + "\n");

			return sb.ToString();
		}
	}
	private class PresetOptions
	{
		public string all = "custom_template/debug=\"\"\r\ncustom_template/release=\"\"\r\ndebug/export_console_wrapper=1\r\nbinary_format/embed_pck=false\r\ntexture_format/bptc=true\r\ntexture_format/s3tc=true\r\ntexture_format/etc=false\r\ntexture_format/etc2=false\r\nbinary_format/architecture=\"x86_64\"\r\ncodesign/enable=false\r\ncodesign/timestamp=true\r\ncodesign/timestamp_server_url=\"\"\r\ncodesign/digest_algorithm=1\r\ncodesign/description=\"\"\r\ncodesign/custom_options=PackedStringArray()\r\napplication/modify_resources=true\r\napplication/icon=\"\"\r\napplication/console_wrapper_icon=\"\"\r\napplication/icon_interpolation=4\r\napplication/file_version=\"\"\r\napplication/product_version=\"\"\r\napplication/company_name=\"\"\r\napplication/product_name=\"\"\r\napplication/file_description=\"\"\r\napplication/copyright=\"\"\r\napplication/trademarks=\"\"\r\nssh_remote_deploy/enabled=false\r\nssh_remote_deploy/host=\"user@host_ip\"\r\nssh_remote_deploy/port=\"22\"\r\nssh_remote_deploy/extra_args_ssh=\"\"\r\nssh_remote_deploy/extra_args_scp=\"\"\r\nssh_remote_deploy/run_script=\"Expand-Archive -LiteralPath '{temp_dir}\\\\{archive_name}' -DestinationPath '{temp_dir}'\r\n$action = New-ScheduledTaskAction -Execute '{temp_dir}\\\\{exe_name}' -Argument '{cmd_args}'\r\n$trigger = New-ScheduledTaskTrigger -Once -At 00:00\r\n$settings = New-ScheduledTaskSettingsSet\r\n$task = New-ScheduledTask -Action $action -Trigger $trigger -Settings $settings\r\nRegister-ScheduledTask godot_remote_debug -InputObject $task -Force:$true\r\nStart-ScheduledTask -TaskName godot_remote_debug\r\nwhile (Get-ScheduledTask -TaskName godot_remote_debug | ? State -eq running) { Start-Sleep -Milliseconds 100 }\r\nUnregister-ScheduledTask -TaskName godot_remote_debug -Confirm:$false -ErrorAction:SilentlyContinue\"\r\nssh_remote_deploy/cleanup_script=\"Stop-ScheduledTask -TaskName godot_remote_debug -ErrorAction:SilentlyContinue\r\nUnregister-ScheduledTask -TaskName godot_remote_debug -Confirm:$false -ErrorAction:SilentlyContinue\r\nRemove-Item -Recurse -Force '{temp_dir}'\"\r\ndotnet/include_scripts_content=false\r\ndotnet/include_debug_symbols=true";
	}

}
