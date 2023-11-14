using Microsoft.VisualBasic;

public class Room 
{

    public string ID;
    public string Name;

    public bool open;

    public int roomSize;

    public Player[] players;

    public static Room currentRoom;


    public Room(string ID)
    {
        this.ID = ID;
    }


    /**
    * <summary> Returns a room imediataly, the ID will be set automatically when the server finishes making the room </summary>
    **/
    public static Room CreateRoom()
    {
        Room room = new("null");
        RoomEvents.CreateRoom.Send("0", "", (data) => {
            room.ID = data[0];
        });
        currentRoom = room;
        return room;
    }

    public void Open()
    {

        RoomEvents.SetRoomSize.Send("0", "", (data) => {
            this.roomSize = data[0];
            players = new Player[roomSize];
            open = true;
            RoomEvents.SetOpenness.Pass("0", true);
        });
        
    }

    //Closes the room from allow more to join
    public void Close()
    {
        open = false;
        RoomEvents.SetOpenness.Pass("0", false);

        
    }

    



}