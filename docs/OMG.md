# SAO HF OMG Model Format

OMG is an model format used by SAO HF.  It is likely a developer (Aquria) specific model format based on the names of properties found in the file.  The files do not seem to carry this extenion, so the name was derived from the magic found inside them.  The M likely stands for "Model"


| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | OMG Magic ("OMG.")
0x4 | 0x4 | FILE_VERSION | OMG Version.  SAO HF uses 1.00 (LE)
0x8 | 0x8 | HEADER_PADDING | Padding/Unknown
0x10 | 0x4 | UNKNOWN1 | Unknown
0x14 | 0x4 | NON_HEADER_SIZE | Size of rest of file (file size - HEADER_SIZE) (LE)


## Properties

OMGs encode a table of properties of the model.  Each entry in the property table is as follows:

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | Property Magic (0x23 0x80 0x00 0x00)
0x4 | 0x4 | PROPERTY_LENGTH | Length of property (LE)
0x8 | 0x1 | PROPERTY_ID | Incremental ID of property
0x9 | 0x1 | UNKNOWN1 | Unknown.  Usually 0x10
0xA | Varies | PROPERTY_NAME | Plaintext name of property

## Shapes

Shapes are the highest level components of OMG model files.  They consist of several sub properties, including the mesh, vertex indices, texture filename, and possibly more.  They layout their sub-items similar to properties.

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x2 | SHAPE_PROPERTY_ID | Shape Property ID (starts at 3?)
0x2 | 0x2 | SHAPE_PROPERTY_MAGIC | Magic? (0x24 0x00)
0x4 | 0x4 | SHAPE_PROPERTY_LENGTH | Length of property (LE)
0x8 | 0x4 | UNKNOWN1 | Unknown.  Usually 0x20
0xC | 0x4 | UNKNOWN2 | Unknown.  Usually 0x20
0x10 | Varies | SHAPE_PROPERTY_NAME | Plaintext name of property
0x10 + sizeof(SHAPE_PROPERTY_NAME) | 0x4 | SHAPE_HASH_MAGIC | Magic that indicates hash????
0x14 + sizeof(SHAPE_PROPERTY_NAME) | 0x4 | SHAPE_HASH_SIZE | Hash size???? (LE)
0x18 + sizeof(SHAPE_PROPERTY_NAME) | Varies | SHAPE_HASH | Hash???? Tiger192????

## Misc 

- [value] 0x80 0x00 0x00 followed by [length] (LE) seems to be how all parts are broken down.  [value] is likely a type ID or something.
- [ID] [ID] 0x20 0x00 (LE) is magic for Shape IDs?
- 0x14 0x80 0x00 0x00 is magic for some kind of hash?  Always length 0x20 followed by 24 bytes of garbage?  Tiger192 hash?
