package game

// The 16 hues of the original (hue.json / BoilerplateHue), stored as 0xAABBGGRR so the bytes in memory are R, G, B, A.
Hue :: enum u8 { Black, Blue, Green, Cyan, Red, Purple, Brown, Light_Gray, Dark_Gray, Light_Blue, Light_Green, Orange, Pink, Tan, Yellow, White }

PALETTE := [Hue]u32{
	.Black = 0xFF000000, .Blue = 0xFFD74B2A, .Green = 0xFF14691D, .Cyan = 0xFFD0D029,
	.Red = 0xFF2323AD, .Purple = 0xFFC02681, .Brown = 0xFF194A81, .Light_Gray = 0xFFA0A0A0,
	.Dark_Gray = 0xFF575757, .Light_Blue = 0xFFFFAF9D, .Light_Green = 0xFF7AC581, .Orange = 0xFF3392FF,
	.Pink = 0xFFF3CDFF, .Tan = 0xFFBBDEE9, .Yellow = 0xFF33EEFF, .White = 0xFFFFFFFF,
}
