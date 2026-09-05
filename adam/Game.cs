using Godot;
using System;

public partial class Game : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		var inpuntVec = new Vector2(Input.GetAxis("ui_Left", "ui_Right"), Input.GetAxis("ui_Up", "ui_Down"));
		GlobalPosition += inpuntVec * 5;
	}
}
