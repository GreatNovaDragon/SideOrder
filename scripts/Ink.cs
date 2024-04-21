using System.Collections.Generic;
using Godot;

public partial class Ink : Sprite2D
{
    private readonly List<(Image, Vector2I, Color, float)> splat_queue = new();
    private Image image;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var parent_size = GetParent<Sprite2D>().Texture.GetSize();
        image = Image.Create((int)parent_size.X, (int)parent_size.Y, false, Image.Format.Rgba8);
        Texture = ImageTexture.CreateFromImage(image);
        ;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Draw()
    {
        foreach (var s in splat_queue)
        {
            var splat = s.Item1;
            var scale = s.Item4;
            var brush_size = (int)(256 * scale);
            var color = s.Item3;

            for (var x = 1; x < 256; x++)
            for (var y = 1; y < 256; y++)
            {
                var c = splat.GetPixel(x, y);
                if (c.A != 0)
                    splat.SetPixel(x, y, color);
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
        var im = splat.GetImage();
        splat_queue.Add((im, pos, color, scale_splat));

        QueueRedraw();
    }

    public Color GetColor(Vector2I position)
    {
        return image.GetPixel(position.X, position.Y);
    }
}
