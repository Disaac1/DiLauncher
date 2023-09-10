using Godot;

public partial class DiLauncher : Control
{

    public override void _Ready()
    {
        initButtons();

        Player player = new Player("dev");
        player.name = "Dev";
        GetNode<playerCard>("playerCard").setPlayer(player);
    }


    public void initButtons()
    {
        Popup popup = GetNode<Popup>("main/join/Popup");
        GetNode<Button>("main/join").Pressed += () =>
        {
            
            popup.CloseRequested += () =>
            {
                popup.Hide();
            };
            popup.GetNode<Button>("VBoxContainer/submit").Pressed += () =>
            {
                string roomCode = "";
                roomCode = popup.GetNode<LabelEdit>("VBoxContainer/Label").Text;

                //Sanitize and confirm only letters



                popup.Hide();

                GD.Print(roomCode);
                
            };

            popup.GetNode<LabelEdit>("VBoxContainer/Label").TextChanged += () =>
            {
                LabelEdit labelEdit = popup.GetNode<LabelEdit>("VBoxContainer/Label");
                string currentText = labelEdit.Text;

                if (currentText.Length > 4)
                {
                    labelEdit.Text = currentText.Substring(0, 4);
                }   
            };

            popup.Popup();
        };

        GetNode<Button>("main/download").Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Launcher/game_browser.tscn");
        };

        GetNode<Button>("main/host").Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Launcher/host.tscn");
        };
    }

    

}
