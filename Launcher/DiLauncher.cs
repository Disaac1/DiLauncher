using Godot;

public partial class DiLauncher : Control
{

    public override void _Ready()
    {
        initButtons();
    }


    public void initButtons()
    {
        Popup popup = GetNode<Popup>("main/join/Popup");
        /*popup.GetNode<LabelEdit>("Container/TextEdit").TextChanged += () =>

        {
            TextEdit edit = popup.GetNode<TextEdit>("VBoxContainer/TextEdit");
            string text = edit.Text.ToUpper();
            text = text.Length < 4 ? text : text[..4];
            
            string copyText = text;
            foreach(char c in text)
            {
                if (!"ABCDEFGHIJKLMNOPQRSTUVWXZ".Contains(c))
                {
                    copyText = copyText.Replace(c.ToString(), "");
                }
            }
            GD.Print(copyText);
            text = copyText;
            edit.Text = text;
            edit.SetCaretColumn(text.Length);

        };*/
        GetNode<Button>("main/join").Pressed += () =>
        {
            
            popup.CloseRequested += () =>
            {
                popup.Hide();
            };
            popup.GetNode<Button>("VBoxContainer/submit").Pressed += () =>
            {
                string roomCode = "";
                roomCode = popup.GetNode<TextEdit>("VBoxContainer/TextEdit").Text;

                //Sanitize and confirm only letters
                


                popup.Hide();

                GD.Print(roomCode);
                
            };

            popup.Popup();
        };

        GetNode<Button>("main/download").Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://Launcher/game_browser.tscn");
        };
    }

    

}
