@tool
extends RigidBody2D

@export var shot_travel_speed = 55
@export var shot_travel_speed_after_straight = 36
@export var when_slowdown = 5






var frame = 0

var velo = 0.0

func _ready():
	$Texture.frame = randi()%$Texture.sprite_frames.get_frame_count("default")
	$Texture.rotation = randf_range(-2*PI, 2*PI)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _physics_process(delta):
	frame += 1
	if frame > when_slowdown:
		velo = velo - (velo * 0.36)
	elif frame == when_slowdown:
		velo = shot_travel_speed_after_straight
	elif frame == 1:
		velo = shot_travel_speed * sqrt(randf())

		
	
	linear_velocity = Vector2(velo*60, 0.0).rotated(rotation)




