extends Node2D

var image
var texture
# Called when the node enters the scene tree for the first time.
func _ready():
	image = Image.create(10000, 10000, false, Image.FORMAT_RGB8)
	texture = ImageTexture.create_from_image(image)
	$Sprite2D.texture = texture
	$Sprite2D.position = Vector2(10000/2,10000/2)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _on_projectile_landed(image_splat:Texture2D, color: int, position_splat: Vector2):
	image = $Sprite2D.texture.get_image()
	image.convert(5)
	var img_splat = image_splat.get_image()
	print(image.get_format())
	image.blend_rect(img_splat, Rect2i(0,0,256,256), position_splat)
	image.convert(4)
	texture.update(image)
	$Sprite2D.texture = texture
	

	
