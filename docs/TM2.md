# SAO HF TM2 Texture Format

TIM2 is a texture format used by the PS1, PS2, PSP and PS Vita.  It is a "1st party" texture format designed by Sony, analogous to DDS on Xbox and PC.  It has no advantages over the newer [GXT](https://github.com/Kirby0Louise/SeedDK/blob/main/docs/GXT.md), however a handful of textures in SAO HF still use it.
## Main File
| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | TM2 Magic ("TIM2")
0x4 | 0x1 | VERSION | TIM2 Version
0x5 | 0x1 | FORMAT | Texture format (unknown what this does)
0x6 | 0x2 | NUM_TEXTURES | Number of textures in this TM2 (LE)
0x8 | 0x8 | PADDING1 | Padding
0x10 | (0x30 + TEX_SIZE) * NUM_TEXTURES | TEXTURE_TABLE | Table of textures, see below

## Texture Metadata
| Address | Size | Variable Name | Description
|--|--|--|--|
0x10 | 0x4 | TEX_SIZE | Texture size (LE)
0x14 | 0x4 | PAL_SIZE | Palette size (LE)
0x18 | 0x4 | TEX_DATA_SIZE | Size of pixel data for texture (LE)
0x1C | 0x2 | HEADER_SIZE | Header size (LE)
0x1E | 0x2 | PAL_COLORS | Number of colors in Palette (LE)
0x20 | 0x1 | TEX_FORMAT | Pixel data format
0x21 | 0x1 | NUM_MIPMAPS | Mipmap count
0x22 | 0x1 | PAL_TYPE | Palette type
0x23 | 0x1 | TEX_TYPE | Texture type
0x24 | 0x2 | TEX_WIDTH | Texture width (LE)
0x26 | 0x2 | TEX_HEIGHT | Texture height (LE)
0x28 | 0x8 | GS_TEX_REG1 | For PS2 GS
0x30 | 0x8 | GS_TEX_REG2 | For PS2 GS
0x38 | 0x4 | GS_FLAGS_REG | For PS2 GS
0x3C | 0x4 | GS_CLUT_REG | For PS2 GS
