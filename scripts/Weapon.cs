using System;
using Godot;

public partial class Weapon : Node2D
{
    [Signal]
    public delegate void ShootEventHandler(PackedScene projectile, float direction, Vector2 position, Color color, float Scale);

    [Signal]
    public delegate void UsedInkEventHandler(double ink);

    [Export] private Color Color = Color.Color8(255, 0, 0);

    private int Frame;

    [Export] private double InkUsage = 0.92;

    [Export] public double Mobility = 0.80;

    [Export] public int BaseDamage = 36;
    [Export] public float HitBoxSize = 0.2f;
    private AudioStreamPlayer2D ping;

    [Export] private PackedScene Projectile;
    public int cooldown_before_reload = 20;

    public bool Shooting = false;

    // WeaponFrames assumes max 60physics ticks per second
    [Export] private int WeaponFrames = 6;

    private double FPS;

    public override void _Ready()
    {
        ping = GetNode<AudioStreamPlayer2D>("PING");
        base._Ready();
    }


    public override void _PhysicsProcess(double delta)
    {
        Frame++;
        if (Shooting)
        {
            if (Frame % (int)(WeaponFrames * (Engine.PhysicsTicksPerSecond/60.00) * Math.Sqrt(Input.GetActionStrength("weapon_shoot"))) == 0)
            {
                ping.Play();
                ping.PitchScale = 1 + (float)(new Random().NextDouble() * 2 - 1) / 10;
                Input.StartJoyVibration(0, 0.5f, 0, 0.1f);
                EmitSignal(SignalName.Shoot, Projectile, GlobalRotation - Math.PI / 2, GlobalPosition, Color, Global.Unit/256.0 * HitBoxSize);
                EmitSignal(SignalName.UsedInk, -InkUsage);
            }
        }

        base._PhysicsProcess(delta);
    }


    public void SetColor(Color Color)
    {
        this.Color = Color;
    }
}