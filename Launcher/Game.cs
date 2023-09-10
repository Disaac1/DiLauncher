using Godot;

public partial class Game : Node {


    private GamePack currentPack = null;


    public void loadPack(GamePack pack)
    {
        currentPack = pack;
        pack.run();
    }


}