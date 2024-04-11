extends Area2D

@export var speed = 4 * Global.Unit
@export var ink_level = 100.0
@export var HP = 100.0
var speed_modifier = 1
var joystick_direction = 0.0
var weapon_direction = 0.0
@export var ink_recovery = 100.00 / 180.00
@export var ink_refresh = false
@export var color: Color = Color.RED

var frames = 0

signal inklevel_change(ink: float)

# Called when the node enters the scene tree for the first time.
func _ready():
	$Weapon.color = color

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _physics_process(delta):
	if Input.is_action_pressed("weapon_shoot")&&!ink_refresh:
			$Weapon.shooting = true
	if Input.is_action_just_pressed("weapon_shoot")&&!ink_refresh:
			if ink_level > 0:
				speed_modifier *= $Weapon.mobility
	if Input.is_action_just_released("weapon_shoot")||ink_level <= 0:
			$Weapon.shooting = false
			speed_modifier = 1.0
		
	if !$Weapon.shooting:
		change_inklevel(ink_recovery)
	
	if ink_refresh&&ink_level == 100:
		ink_refresh = false
			
	var movement_direction = Vector2.ZERO
	movement_direction.y = Input.get_axis("move_up", "move_down")
	movement_direction.x = Input.get_axis("move_left", "move_right")
	if movement_direction.length() > 0:
		position += movement_direction.normalized() * speed * speed_modifier * delta
		
	var joystick_vector = Input.get_vector("weapon_dir_left", "weapon_dir_right", "weapon_dir_up", "weapon_dir_down")
	
	if joystick_vector.length() > 0:
		joystick_direction = joystick_vector.angle() + PI / 2
	
	var mouse_direction = get_global_mouse_position().angle_to_point(position) - PI / 2
	
	weapon_direction = joystick_direction
	rotation = weapon_direction
	
func start(pos):
	position = pos

func change_inklevel(ink_change):
	if ink_level >= 100&&ink_change > 0:
		ink_level = 100
		return
	ink_level += ink_change
	if ink_level < 0:
		ink_refresh = true
		ink_level = 0
	inklevel_change.emit(ink_level)

func _on_weapon_used_ink(ink):
	print("haha")
	change_inklevel( - ink)
