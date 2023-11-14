using Godot;
using System;

public partial class networking_test : Node2D
{

   public override void _Ready()
   {


      GetNode<Button>("CanvasLayer/Control/createRoom").Pressed += () => {
         Room room = Room.CreateRoom();
      };

      
   }





}
