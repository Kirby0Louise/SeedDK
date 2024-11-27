# SAO HF GXT Texture Format

GXT is a texture format used by the PS Vita.  It is a "1st party" texture format designed by Sony, analogous to DDS on Xbox and PC.  Unfortunately, it lacks many of the features of DDS and many of SAO HF's textures use older PS1/2/P style compression.
## Main File
| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | GXT Magic ("GXT\u0000")
0x4 | 0x4 | VERSION | GXT Version
0x8 | 0x4 | NUM_TEXTURES | Number of textures stored in this GXT (LE)
0xC | 0x4 | DATA_OFFSET | Ptr to beginning of texture data (LE)
0x10 | 0x4 | DATA_SIZE | Size of texture data (LE)
0x14 | 0x4 | NUM_4BPP_PALS | Number 4BPP palettes (LE)
0x18 | 0x4 | NUM_8BPP_PALS | Number 8BPP palettes (LE)
0x1C | 0x4 | PADDING1 | Padding
0x20 | 0x20 * NUM_TEXTURES | TEXTURE_METADATA_TABLE | Table of texture metadata, see below

## Texture Metadata
| Address | Size | Variable Name | Description
|--|--|--|--|
0x20 | 0x4 | TEX_OFFSET | Ptr to texture data (LE)
0x24 | 0x4 | TEX_SIZE | Texture size (LE)
0x28 | 0x4 | PAL_INDEX | Index into palette table for texture (LE)
0x2C | 0x4 | TEX_FLAGS | Texture flags
0x30 | 0x4 | TEX_TYPE | Tells Vita GPU how texture will be accessed (LE)
0x34 | 0x4 | TEX_FORMAT | Pixel data format (LE)
0x38 | 0x2 | TEX_WIDTH | Texture width (LE)
0x3A | 0x2 | TEX_HEIGHT | Texture height (LE)
0x3C | 0x2 | NUM_MIPMAPS | Number of mipmaps + 1 (LE)
0x3E | 0x2 | PADDING2 | Padding

## Misc

SAO HF uses an optional "BUV" section of the header, which changes some decoding behavior.  It is currently unknown exactly how this works, but some possible effects include scrolling and megatexture.
