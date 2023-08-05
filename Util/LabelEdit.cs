using Godot;
using System;

[GlobalClass]
public partial class LabelEdit : Label
{

    [Signal]
    public delegate void TextChangedEventHandler();


    [Export]
    public string PlaceHolderText;

    [Export]
    public bool onlyUppercase;

    [Export]
    public bool readOnly = false;

    [Export]
    public bool editing = false;

    private string _text = "";


    public override void _Ready()
    {
        Panel panel = new Panel();
        AddChild(panel);
        Text = PlaceHolderText;

        GuiInput += (InputEvent @event) =>
        {
            if (@event is InputEventMouseButton eventMouseButton)
            {
                if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
                {
                    GrabFocus();
                }
            }
        };

        FocusEntered += () => editing = true;
        FocusExited += () => editing = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (editing)
        {
            if(@event is InputEventKey keyEvent)
            {
                if(keyEvent.Pressed)
                {
                    if(_text.Length > 0 && keyEvent.Keycode == Key.Backspace)
                    {
                        _text = _text.Substr(0, _text.Length - 1);
                     
                    } else if("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(keyEvent.AsTextKeycode()))
                    {
                        _text += keyEvent.AsText();
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
