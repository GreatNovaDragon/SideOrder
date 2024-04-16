using System;
using Godot;

public partial class Player : Area2D
{
    [Signal]
    public delegate void InklevelChangeEventHandler(double ink);

    private Color Color = Color.Color8(255, 0, 0);
    private int cooldown_end_Frame;

    private int Frame;

    [Export] private double HP = 100d;

    [Export] private double InkLevel = 100d;

    // assuming 60 physics ticks per second
    [Export] private double InkRecovery = 100.0 / 180.0;

    [Export] private bool InkRefresh;

    private double JoystickDirection;

    // assuming 60 physics ticks per second
    [Export] private double Speed = 0.96;

    private double SpeedModifier = 1;

    private Weapon Weapon;
    private double WeaponDirection;

    [Export] private PackedScene WeaponScene;

    public override void _Ready()
    {
        SetWeapon(WeaponScene);
        base._Ready();
        cooldown_end_Frame = 0;
    }


    public override void _PhysicsProcess(double delta)
    {
        Frame++;
        if (Input.IsActionJustPressed("weapon_shoot") && !InkRefresh && InkLevel > 0) SpeedModifier *= Weapon.Mobility;
        if (Input.IsActionJustReleased("weapon_shoot") || InkLevel <= 0)
        {
            Weapon.Shooting = false;
            GD.Print($"cooldown end {cooldown_end_Frame}");
            SpeedModifier = 1;
        }

        if (Input.IsActionJustReleased("weapon_shoot"))
            cooldown_end_Frame = Frame + Weapon.cooldown_before_reload * Engine.PhysicsTicksPerSecond / 60;


        if (InkRefresh) Weapon.Shooting = false;

        if (!Weapon.Shooting && Frame > cooldown_end_Frame)
            ChangeInklevel(InkRecovery * (Engine.PhysicsTicksPerSecond / 60.00));

        if (InkRefresh && InkLevel == 100) InkRefresh = false;
        var MovementDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        var SpeedTotal = (float)(Speed * SpeedModifier * Global.EngineSpeedMod * Global.Unit * delta);
        if (MovementDirection.Length() > 0)
            Position += MovementDirection.Normalized() * new Vector2(SpeedTotal, SpeedTotal);


        var JoystickVector = Input.GetVector("weapon_dir_left", "weapon_dir_right", "weapon_dir_up", "weapon_dir_down");

        if (JoystickVector.Length() > 0) JoystickDirection = JoystickVector.Angle() + Math.PI / 2;
        Rotation = (float)JoystickDirection;
        base._PhysicsProcess(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("weapon_shoot") && !InkRefresh) Weapon.Shooting = true;
    }

    public void ChangeInklevel(double Change)
    {
        if (InkLevel >= 100 && Change > 0)
        {
            InkLevel = 100;
            return;
        }

        InkLevel += Change;
        if (InkLevel < 0)
        {
            InkRefresh = true;
            InkLevel = 0;
        }

        EmitSignal(SignalName.InklevelChange, InkLevel);
    }

    public void Start(Vector2 Position)
    {
        this.Position = Position;
    }

    public void OnWeaponUsedInk(double used_ink)
    {
        ChangeInklevel(used_ink);
    }

    public void SetWeapon(PackedScene WeaponScene)
    {
        if (Weapon != null) RemoveChild(GetNode<Weapon>("Weapon"));

        Weapon = WeaponScene.Instantiate<Weapon>();
        Weapon.SetColor(Color);
        Weapon.Shoot += GetNode<main>("/root/Main").OnWeaponShoot;
        Weapon.UsedInk += OnWeaponUsedInk;
        AddChild(Weapon);
    }
}