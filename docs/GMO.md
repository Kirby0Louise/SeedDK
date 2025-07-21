# SAO HF GMO Model Format

GMO (Vita) is an model format used by SAO HF.  It is likely a developer (Aquria) specific modification of the PSP's standard GMO format based on the names of properties found in the file.  The files do not seem to carry this extenion, so the name was derived from the magic found inside them (endian is switched as PSP MIPS is BE).

GMO (Vita)s are laid out in a tree based format.  See below for more details.


| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | GMO (Vita) Magic ("GMO.") (LE)
0x4 | 0x4 | FILE_VERSION | GMO (Vita) Version.  SAO HF uses 1.00 (LE)
0x8 | 0x8 | HEADER_PADDING | Padding/Unknown

## Chunks

Chunks are the highest level components of GMO (Vita) model files.

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x2 | CHUNK_TYPE | See table below of values (LE)
0x2 | 0x2 | CHUNK_HEADER_SIZE | Size of header, sometimes 0x00, which means implicit size. (LE)
0x4 | 0x4 | CHUNK_SIZE | Size of chunk (LE)
0x10 | CHUNK_SIZE - CHUNK_HEADER_SIZE (unless implicit, then - 8) | CHUNK_DATA | Data

## Chunk Types

| Value | Description
|--|--|
0x0002 | Root Chunk
0x0003 | Branch Chunk
0x0004 | Bone Info
0x0005 | Model Surface
0x0006 | Mesh
0x0007 | Vertex Array
0x0008 | Material
0x0009 | Texture Reference
0x000A | Texture
0x000B | Shared Vertex Data
0x000F | Texture Animation
0x8014 | Unknown, 192-bit magic?
0x8015 | UV Scale and Bias
0x8023 | Aquria specific Parameters?
0x8061 | Mesh Material Info
0x8066 | Mesh Index Data
0x8081 | Material Texture Blend
0x8082 | Material RGBA
0x8083 | Material Specularity

## Sources 

https://web.archive.org/web/20090922200620/https://www.richwhitehouse.com/index.php?postid=34

https://squall-leonhart.net/ff-wiki/PSP/GMO_Format.html
