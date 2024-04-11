extends Sprite2D
var splat_queue = []
@onready var parent_texture = get_parent().texture.get_size()
@onready var image = Image.create(parent_texture.x, parent_texture.y, false, 5)

# Called when the node enters the scene tree for the first time.
func _ready():
	texture = ImageTexture.create_from_image(image)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _draw():
	for s in splat_queue:
		var splat = s[0]
		var scale_splat = s[3]
		var brush_size = 256 * scale_splat

		for x in 256:
			for y in 256:
				var c = splat.get_pixel(x, y)
				if c.a != 0:
					splat.set_pixel(x, y, s[2])
		
		splat.resize(brush_size, brush_size, 4)
		var pos = s[1] - Vector2(brush_size / 2, brush_size / 2)
		var source_rect = Rect2i(0, 0, brush_size, brush_size)
		image.blend_rect(splat, source_rect, pos - offset)
	splat_queue = []
	texture.update(image)

func paint(splat: Texture2D, pos: Vector2, color: Color, scale_splat: float):
	splat_queue.append([splat.get_image(), pos, color, scale_splat])
	
	queue_redraw()
	
func get_color(pos: Vector2):
	return image.get_pixel(pos.x, pos.y)
