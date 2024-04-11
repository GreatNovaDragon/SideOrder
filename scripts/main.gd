extends Node

# Called when the node enters the scene tree for the first time.
func _ready():
	$Player.start($Middle.position)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _on_weapon_shoot(projectile, direction, location, color):
	var spawned_projectile = projectile.instantiate()
	spawned_projectile.rotation = direction
	spawned_projectile.position = location
	spawned_projectile.color = color
	add_child(spawned_projectile)
