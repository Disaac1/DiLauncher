using Godot;

public partial class RichPressence : Node
{

    public override void _Ready()
    {
        GD.Print("RichPressence Ready");
        DiscordRichPressence.app_id = 1154891864487493773;
        DiscordRichPressence.details = "Test";

        string secret = "testsecret";
        DiscordRichPressence.party_id = "diLauncher_"+secret;
        DiscordRichPressence.current_party_size = 1;
        DiscordRichPressence.max_party_size = 4;
        DiscordRichPressence.match_secret = "m_"+secret;
        DiscordRichPressence.join_secret = "j_"+secret;
        DiscordRichPressence.spectate_secret = "s_"+secret;

        DiscordRichPressence.is_party_public = true;
        DiscordRichPressence.instanced = true;

        DiscordRichPressence.start_timestamp = Time.GetUnixTimeFromSystem();

        DiscordRichPressence.refresh();
    }


}