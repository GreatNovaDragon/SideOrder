using Godot;
using System;
using System.Collections.Generic;

public partial class Ink : Sprite2D
{
	List<(Image, Vector2I, Color, float)> splat_queue = new List<(Image, Vector2I, Color, float)>();
	Image image;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var parent_size = GetParent<Sprite2D>().Texture.GetSize();
		image = Image.Create((int)parent_size.X, (int)parent_size.Y, false, Image.Format.Rgba8);
		Texture = ImageTexture.CreateFromImage(image); ;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Draw()
	{
		GD.Print("BLURGH");
		GD.Print(splat_queue.Count);

		foreach (var s in splat_queue)
		{
			GD.Print("BLERGH");
			Image splat = s.Item1;
			float scale = s.Item4;
			int brush_size = (int)(256 * scale);
			Color color = s.Item3;

			for (int x = 0; x < 256; x++)
			{
				for (int y = 0; x < 256; y++)
				{
					GD.Print($"{x} {y}");
					var c = splat.GetPixel(x, y);
					if (c.A != 0) splat.SetPixel(x, y, color);
				}
			}

			splat.Resize(brush_size, brush_size);
			var pos = s.Item2 - new Vector2I(brush_size / 2, brush_size / 2);
			var source_rect = new Rect2I(0, 0, brush_size, brush_size);
			image.BlendRect(splat, source_rect, pos - new Vector2I((int)Offset.X, (int)Offset.Y));
		}
		splat_queue.Clear();
		((ImageTexture)Texture).Update(image);
	}

	public void Paint(Texture2D splat, Vector2I pos, Color color, float scale_splat)
	{
		GD.Print(splat_queue.Count);
		GD.Print("blap");
		var im = splat.GetImage();
		splat_queue.Add((im, pos, color, scale_splat));

		QueueRedraw();
	}

	public Color GetColor(Vector2I position)
	{
		return image.GetPixel(position.X, position.Y);
	}
}
