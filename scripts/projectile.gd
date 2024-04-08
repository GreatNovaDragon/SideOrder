extends Area2D

@export var shot_travel_speed = Global.Unit * 22
@export var shot_travel_speed_after_straight = Global.Unit * 14.495
@export var when_slowdown =  5
@export var size_scale: float = 1.0
@export var color: Color = Color.RED
@onready var frame = 0

@onready var map: Node2D = $"/root/Main/Map"

var velo = 0.0

func _ready():
	$Texture.frame = randi()%$Texture.sprite_frames.get_frame_count("default")
	$Texture.set_scale(Vector2(size_scale, size_scale))
	$CollisionShape2D.set_scale(Vector2(size_scale, size_scale))
	$Texture.rotation -= rotation
	modulate = color
	
func _physics_process(delta):
	frame += 1
	if frame > when_slowdown:
		velo = velo - (velo * 0.36)
	elif frame == when_slowdown:
		velo = shot_travel_speed_after_straight
	elif frame == 1:
		velo = shot_travel_speed * sqrt(randf())

		
	
	var velocity = Vector2(velo, 0.0).rotated(rotation)
	position += velocity*delta
	
	if velocity.length() <= 0.1:
		var text = $Texture.sprite_frames.get_frame_texture("default", $Texture.frame)
		map.paint(text, global_position, color, size_scale)
		queue_free()
	





