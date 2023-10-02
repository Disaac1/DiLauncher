

using Godot;
using System.IO;
using System.Net;

public partial class DiHub
{


	/**
	 * <summary>Get a file from the download repo at loaction</summary>
	 */
	public static bool get(string location, string saveLocation)
	{
		string uri = "https://download.disaac1.com/"+ location;

		if(saveLocation.StartsWith("user://"))
		{
			saveLocation = ProjectSettings.GlobalizePath(saveLocation);
		}

		//Download file from uri
		WebClient webClient = new WebClient();
		try
		{
			webClient.DownloadFile(uri, saveLocation);
		} catch(WebException err)
		{
			GD.Print("WebException: " + err.Message);
			GD.Print("Tried to download " + uri + " to " + saveLocation);
			return false;
		}
		

		GD.Print("Downloaded " + uri + " to " + saveLocation);
		return true;
	}

	public static Resource load(string location)
	{
		//Download file from DiHub and then load it from godot
		string ext = Path.GetExtension(location);

		if(ext != ".webp") return null;
		string tempFile = "user://temp/" + Path.GetRandomFileName() + ext;
		if(!get(location, tempFile))
		{
			return null;
		}
		//Godot doesn't allow loading files from user:// so we have to load it with file type specific code

		Resource res;

		switch(ext)
		{
			case ".webp":
				Image image = new Image();
				image.Load(tempFile);
				res = image;
				break;
			default:
				GD.Print("Unknown file type: " + ext);
				res = null;
				break;
		}
		DirAccess.RemoveAbsolute(tempFile);
		return res;
	}

}
