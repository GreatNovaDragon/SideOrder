extends Area2D

@export var speed = 400.0
@export var inklevel = 100.0
@export var hp = 100.0
var speedmod = 1
var joy_dir = 0.0
var weapondir = 0.0
var inkrecovery = 100.00 / 180.00
var ink_deplete_refresh = false

var frames = 0

signal inklevel_change(ink: float)


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _physics_process(delta):
	if Input.is_action_pressed("weapon_shoot") && !ink_deplete_refresh:
			$Weapon.shooting = true
	if Input.is_action_just_pressed("weapon_shoot") && !ink_deplete_refresh:	
			if inklevel > 0:
				speedmod *= $Weapon.mobility
	if Input.is_action_just_released("weapon_shoot") || inklevel <= 0:
			$Weapon.shooting = false
			speedmod = 1.0	
		
	if !$Weapon.shooting :
		change_inklevel(inkrecovery)

	
	if ink_deplete_refresh && inklevel == 100:
		ink_deplete_refresh = false
		

			
	var goal_Accel = Vector2.ZERO
	goal_Accel.y = Input.get_axis("move_up", "move_down")
	goal_Accel.x = Input.get_axis("move_left", "move_right")
	if goal_Accel.length() > 0:
		position += goal_Accel.normalized() * speed * speedmod * delta
		
	var joy_vector = Input.get_vector("weapon_dir_left", "weapon_dir_right", "weapon_dir_up", "weapon_dir_down")
	
	if joy_vector.length() > 0:
		joy_dir = joy_vector.angle() + PI/2
	
	var mouse_dir = get_global_mouse_position().angle_to_point(position) - PI/2
	
	weapondir = joy_dir
	rotation = weapondir



	
func start(pos):
	position = pos

func change_inklevel(ink):
	if inklevel >= 100 && ink > 0:
		inklevel = 100
		return
	inklevel += ink
	if inklevel < 0:
		ink_deplete_refresh = true
		inklevel = 0
	inklevel_change.emit(inklevel)

func get_inklevel():
	return inklevel


func _on_weapon_used_ink(ink):
	print("haha")
	change_inklevel(-ink)
