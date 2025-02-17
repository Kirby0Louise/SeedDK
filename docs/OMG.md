# SAO HF OMG Model Format

OMG is an model format used by SAO HF.  It is likely a developer (Aquria) specific model format based on the names of properties found in the file.  The files do not seem to carry this extenion, so the name was derived from the magic found inside them.  The M likely stands for "Model"


| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | OMG Magic ("OMG.")
0x4 | 0x4 | FILE_VERSION | OMG Version.  SAO HF uses "00.1"
0x8 | 0x8 | HEADER_PADDING | Padding/Unknown
0x10 | 0x4 | UNKNOWN1 | Unknown
0x14 | 0x4 | NON_HEADER_SIZE | Size of rest of file (file size - HEADER_SIZE) (LE)


## Properties

OMGs encode a table of properties of the model.  Each entry in the property table is as follows:

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | Property Magic (0x23 0x80 0x00 0x00)
0x4 | 0x4 | PROPERTY_LENGTH | Length of property
0x8 | 0x1 | PROPERTY_ID | Incremental ID of property
0x9 | 0x1 | UNKNOWN1 | Unknown.  Usually 0x10
0xA | Varies | PROPERTY_NAME | Plaintext name of property

## Misc 

- [value] 0x80 0x00 0x00 followed by [length] (LE) seems to be how all parts are broken down.  [value] is likely a type ID or something.
