using System;

public class RoomEvents : IEventRegistry
{

    public static void Init()
    {
        GodotLogger.info("Initializing Room Events");
    }

    static RoomEvents()
    {
        
    }

    /**
    *  <summary> Creates a room </summary>
    *  <para> Return String - THe id of the room just created </para>
    *  <c> Only succeeds when sent to the Rendezvous Server </c>
    */
    public static readonly RequestEvent<object, string> CreateRoom = new("CreateRoom");


    /// <summary>
    /// Set if the room is open for players to join
    /// </summary>
    public static readonly Event<bool> SetOpenness = new("SetOpenness");

    /// <summary>
    /// Request the room to be deleted will kick all connected players
    /// </summary>
    public static readonly Event<object> DeleteRoom = new("DeleteRoom");

    /// <summary>
    /// Player has joined the room
    /// </summary>
    public static readonly Event<Player> PlayerJoined = new("PlayerJoinedRoom");

    /// <summary>
    /// Player has left the room
    /// </summary>
    public static readonly Event<Player> PlayerLeft = new("PlayerLeftRoom");


    /// <summary>
    /// Request to the leave the room with the optional string for reason. 
    /// If received then was kicked for a string reason
    /// </summary>
    public static readonly Event<string> LeaveRoom = new("LeaveRoom");


    /// <summary>
    /// Request to join a room with a provided ID 
    /// <para>
    ///     Room ID : string
    /// </para>
    /// </summary>
    public static readonly Event<string> JoinRoom = new("JoinRoom");


    public static readonly RequestEvent<object, int> SetRoomSize = new("SetRoomSize");


     

}