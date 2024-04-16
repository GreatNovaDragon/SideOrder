using Godot;

public partial class Global : Node
{
    public const int Unit = 200;
    public static readonly double EngineSpeedMod = Engine.PhysicsTicksPerSecond / 60.00;
}