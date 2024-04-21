using System;
using Godot;
using Godot.Collections;

public partial class Projectile : Area2D
{
    [Export]
    public int BaseDamage = 36;
    private int Frame;
    private Map Map;
    private Texture2D SplatTexture;
    private double Velo;

    [Export]
    public double ShotTravelSpeed { get; set; } = 22;

    [Export]
    public double ShotTravelSpeedAfterStraight { get; set; } = 14.495F;

    [Export]
    public int WhenSlowdown { get; set; } = 5;

    [Export]
    public double SizeScale { get; set; } = 1;

    [Export]
    public Color Color { get; set; } = Color.Color8(255, 0, 0);

    [Export]
    public Array<Texture2D> Splats { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Frame = 0;
        Map = GetNode<Map>("/root/Main/Map");
        SplatTexture = Splats.PickRandom();
        GetNode<Sprite2D>("Texture").Texture = SplatTexture;
        Modulate = Color;
        Scale = new Vector2((float)SizeScale, (float)SizeScale);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        Frame++;

        if (Frame > WhenSlowdown)
            Velo -= Velo * 0.36f;
        else if (Frame == WhenSlowdown)
            Velo = ShotTravelSpeedAfterStraight * Global.EngineSpeedMod * Global.Unit;
        else if (Frame == 1)
            Velo =
                ShotTravelSpeed
                * (float)Math.Sqrt(new Random().NextDouble())
                * Global.EngineSpeedMod
                * Global.Unit;

        var velocity = new Vector2((float)Velo, 0.0f).Rotated(Rotation);
        Position += (float)delta * velocity;

        if (velocity.Length() <= 0.1)
        {
            Map.Paint(
                SplatTexture,
                new Vector2I((int)GlobalPosition.X, (int)GlobalPosition.Y),
                Color,
                (float)SizeScale
            );
            QueueFree();
        }

        base._PhysicsProcess(delta);
    }
}
