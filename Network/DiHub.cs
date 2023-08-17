

using Godot;
using System.Net;

public partial class DiHub
{


	/**
	 * <summary>Get a file from the download repo at loaction</summary>
	 */
	public static void get(string location, string saveLocation)
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
			return;
		}
		

		GD.Print("Downloaded " + uri + " to " + saveLocation);
	}

}
