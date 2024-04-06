extends Node2D

var shooting = false

@export var weapon_frames = 6
@export var ink_usage =  0.92
@export var projectile_scene: PackedScene
@export var mobility = 0.288

signal used_ink(ink: float)
signal shoot(projectile, direction, location)


var frame = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	frame += 1
	if shooting:
		if frame%int(weapon_frames/sqrt(Input.get_action_strength("weapon_shoot"))) == 0:
			
			$PING.play()
			$PING.pitch_scale = 1 + randf_range(-0.1,0.1)
			Input.start_joy_vibration(0, 0.5, 0, 0.1)
			shoot.emit(projectile_scene,  global_rotation - PI/2, global_position)
			
			used_ink.emit(ink_usage)
			


		


	
	
