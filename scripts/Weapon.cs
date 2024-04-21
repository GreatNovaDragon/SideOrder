using System;
using Godot;

public partial class Weapon : Node2D
{
    [Signal]
    public delegate void ShootEventHandler(
        PackedScene projectile,
        float direction,
        Vector2 position,
        Color color,
        float Scale
    );

    [Signal]
    public delegate void UsedInkEventHandler(double ink);

    [Export]
    public int BaseDamage = 36;

    [Export]
    private Color Color = Color.Color8(255, 0, 0);
    public int cooldown_before_reload = 20;

    private double FPS;

    private int Frame;

    [Export]
    public float HitBoxSize = 0.2f;

    [Export]
    private double InkUsage = 0.92;

    [Export]
    public double Mobility = 0.80;
    private AudioStreamPlayer2D ping;

    [Export]
    private PackedScene Projectile;

    public bool Shooting = false;

    // WeaponFrames assumes max 60physics ticks per second
    [Export]
    private int WeaponFrames = 6;

    public override void _Ready()
    {
        ping = GetNode<AudioStreamPlayer2D>("PING");
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        Frame++;
        if (Shooting)
            if (
                Frame % (int)(WeaponFrames * Global.EngineSpeedMod * Math.Sqrt(Input.GetActionStrength("weapon_shoot")))
                == 0
            )
            {
                ping.Play();
                ping.PitchScale = 1 + (float)(new Random().NextDouble() * 2 - 1) / 10;
                Input.StartJoyVibration(0, 0.5f, 0, 0.1f);
                EmitSignal(
                    SignalName.Shoot,
                    Projectile,
                    GlobalRotation - Math.PI / 2,
                    GlobalPosition,
                    Color,
                    Global.Unit / 256.0 * HitBoxSize
                );
                EmitSignal(SignalName.UsedInk, -InkUsage);
            }

        base._PhysicsProcess(delta);
    }

    public void SetColor(Color Color)
    {
        this.Color = Color;
    }
}
