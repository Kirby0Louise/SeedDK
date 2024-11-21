# SAO HF OFS3 Archive Format

OFS3 is an internal archive format used by SAO HF.  From my research, it is not a format specific to any one system, game or developer.  It may be a Criware middleware format, but there's no strong evidence to this.
Regardless, this doc is all the currently known information on how to unpack the version of OFS3 used by SAO HF.


| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | OFS3 Magic ("OFS3")
0x4 | 0x4 | HEADER_SIZE | OFS3 Header size (LE)
0x8 | 0x2 | ARCHIVE_TYPE | Archive type.  0 = parent archive?, 1 = child archive? (LE)
0xA | 0x2 | ARCHIVE_CONFIG | Archive configuration (flags)
0xC | 0x4 | NON_HEADER_SIZE | Size of rest of file (file size - HEADER_SIZE) (LE)
0x10 | 0x4 | NUM_FILES | Number of files in archive (LE)
0x14 | 0x8 * NUM_FILES | FILE_OFFSET_AND_SIZE_TABLE | Table of file offsets and sizes.  Offsets are from after header (add 0x10 to value to find true address in file) (LE)

## Misc

Archives and files always seem 8 byte aligned.

