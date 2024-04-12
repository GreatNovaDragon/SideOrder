using Godot;
using System;
using System.Threading;

public partial class Map : Node2D
{

	Ink ink;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ink = GetNode<Ink>("Enviroment/Ink");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public  void Paint(Texture2D splat, Vector2I pos, Color color, float scale_splat)
	{
		ink.Paint(splat, pos, color, scale_splat);
	}
}
