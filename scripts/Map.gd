extends Node2D
@onready var ink: Sprite2D = $Enviroment/Ink


func paint(text: Texture2D, pos: Vector2, color: Color, size_scale: float):
	ink.paint(text, pos, color, size_scale)



