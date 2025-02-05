using Godot;

public partial class main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Player>("Player").Start(GetNode<Marker2D>("Middle").Position);
    }

    public void OnWeaponShoot(PackedScene projectile, float direction, Vector2 position, Color color, float scale)
    {
        var spawned_projectile = projectile.Instantiate<Projectile>();
        spawned_projectile.Rotation = direction;
        spawned_projectile.Position = position;
        spawned_projectile.Color = color;
        spawned_projectile.Scale = new Vector2(scale, scale);
        AddChild(spawned_projectile);
    }
}
