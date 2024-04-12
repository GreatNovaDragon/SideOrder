using System;
using Godot;
using GodotPlugins.Game;

public partial class Player : Area2D
{

	[Export]
	double Speed = 4.0d;
	[Export]
	double InkLevel = 100d;
	[Export]
	double HP = 100d;
	double SpeedModifier = 1;
	double JoystickDirection = 0d;
	double WeaponDirection = 0d;

	[Export]
	double InkRecovery = 100 / 180;

	[Export]
	bool InkRefresh = false;

	Color Color = Color.Color8(255, 0, 0);

	int Frame = 0;

	[Export]
	PackedScene WeaponScene;

	Weapon Weapon;


	[Signal]
	public delegate void InklevelChangeEventHandler(double ink);

	public override void _Ready()
	{
		SetWeapon(WeaponScene);
		base._Ready();
	}


	public override void _PhysicsProcess(double delta)
	{
		Frame++;
		if (Input.IsActionPressed("weapon_shoot") && !InkRefresh) Weapon.Shooting = true;
		if (Input.IsActionJustPressed("weapon_shoot") && !InkRefresh && InkLevel > 0) SpeedModifier *= Weapon.Mobility;
		if (Input.IsActionJustReleased("weapon_shoot") || InkLevel <= 0)
		{
			Weapon.Shooting = false;
			SpeedModifier = 1;
		}

		if (!Weapon.Shooting) ChangeInklevel(InkRecovery);
		if (InkRefresh && InkLevel == 100) InkRefresh = false;
		Vector2 MovementDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		float SpeedTotal = (float)(Speed * SpeedModifier * Global.Unit * delta);
		if (MovementDirection.Length() > 0) Position += MovementDirection.Normalized() * new Vector2(SpeedTotal, SpeedTotal);


		Vector2 JoystickVector = Input.GetVector("weapon_dir_left", "weapon_dir_right", "weapon_dir_up", "weapon_dir_down");

		if (JoystickVector.Length() > 0) JoystickDirection = JoystickVector.Angle() + Math.PI / 2;
		Rotation = (float)JoystickDirection;
		base._PhysicsProcess(delta);
	}

	public void ChangeInklevel(double Change)
	{
		if (InkLevel >= 100 && Change > 0) { InkLevel = 100; return; }
		InkLevel += Change;
		if (InkLevel < 0) { InkRefresh = true; InkLevel = 0; }
		EmitSignal(SignalName.InklevelChange, InkLevel);
	}

	public void Start(Vector2 Position)
	{
		this.Position = Position;
	}

	public void SetWeapon(PackedScene WeaponScene)
	{
		if (Weapon != null)
		{
			RemoveChild(GetNode<Weapon>("Weapon"));
		}

		Weapon = WeaponScene.Instantiate<Weapon>();
		Weapon.SetColor(Color);
		Weapon.Shoot += GetNode<main>("/root/Main").OnWeaponShoot;
		AddChild(Weapon);
	}

}

