using Godot;
using System;

[GlobalClass]
public partial class LabelEdit : Label
{

    //TODO:
    // Allow setting avaiable characters
    // Allow settting a regex for validation
    // Allow getter for if valid

    [Signal]
    public delegate void TextChangedEventHandler();

    [Signal]
    public delegate void TextSubmittedEventHandler(string submittedText);


    [Export]
    public string PlaceHolderText;

    [Export]
    public bool onlyUppercase;

    [Export]
    public bool readOnly = false;

    [Export]
    public bool editing = false;

    [Export]
    public bool submitOnEnter = false;

    [Export]
    public string acceptedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    private string _text = "";


    public override void _Ready()
    {
        FocusMode = FocusModeEnum.All;
        Panel panel = new Panel();
        AddChild(panel);
        Text = PlaceHolderText;

        FocusEntered += () => editing = true;
        FocusExited += () => editing = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (editing && !readOnly)
        {
            if(@event is InputEventKey keyEvent)
            {
                if(keyEvent.Pressed)
                {

                    if(keyEvent.Keycode == Key.Enter)
                    {
                        EmitSignal(SignalName.TextSubmitted, _text);
                        ReleaseFocus();
                    }

                    GD.Print(keyEvent.AsTextPhysicalKeycode());
                    if(_text.Length > 0 && keyEvent.Keycode == Key.Backspace)
                    {
                        _text = _text.Substr(0, _text.Length - 1);
                     
                    }
                    //If valid character from accepted characters
                    else if(acceptedCharacters.Contains(keyEvent.AsTextPhysicalKeycode()) || acceptedCharacters.Contains(keyEvent.Keycode.ToString()))
                    {
                        if(keyEvent.ShiftPressed){
                            _text += keyEvent.KeyLabel.ToString();
                        } else {
                            _text += keyEvent.AsTextPhysicalKeycode().ToLower();
                        }
                        
                    }

                    string newText = _text;
                    if (onlyUppercase)
                    {
                        newText = newText.ToUpper();
                    }

                    _text = newText;

                    if (_text != "") Text = _text;
                    else Text = PlaceHolderText;
                }
            }
        }
    }



}
