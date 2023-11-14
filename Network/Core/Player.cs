using Godot;

public class PlayerNetwork
{

    //Handles the game side of the player.. Heavilty connected to the Peer/Network side


    /**
    * <summary>Player ID</summary>
    */
    private string ID;

    /**
    * <summary>Player Name</summary>
    */
    public string Name { get; private set; }


    public Texture2D avatar;

    public void SetName(string name)
    {
        Name = name;
    }

}