# OUTDATED - THIS SHOULD NOT BE RELIED ON, BUT IS HERE JUST IN CASE

# SAO HF OMG Model Format

OMG is an model format used by SAO HF.  It is likely a developer (Aquria) specific model format based on the names of properties found in the file.  The files do not seem to carry this extenion, so the name was derived from the magic found inside them.  The M likely stands for "Model"

OMGs are laid out in a tree based format.  See below for more details.


| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | OMG Magic ("OMG.")
0x4 | 0x4 | FILE_VERSION | OMG Version.  SAO HF uses 1.00 (LE)
0x8 | 0x8 | HEADER_PADDING | Padding/Unknown
0x10 | 0x4 | UNKNOWN1 | Unknown
0x14 | 0x4 | NON_HEADER_SIZE | Size of rest of file (file size - HEADER_SIZE) (LE)

## Branches

Branches are the highest level components of OMG model files.

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x2 | BRANCH_ID | Shape Property ID (starts at 3?)
0x2 | 0x2 | BRANCH_DEPTH | Tree Depth? (0x24 0x00)
0x4 | 0x4 | BRANCH_LENGTH | Length of property (LE)
0x8 | 0x4 | UNKNOWN1 | Unknown.
0xC | 0x4 | UNKNOWN2 | Unknown.
0x10 | BRANCH_LENGTH - 16 | BRANCH_DATA | Data

## Properties

OMGs encode a table of properties of the model.  Each entry in the property table is as follows:

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | MAGIC | Property Magic (0x23 0x80 0x00 0x00)
0x4 | 0x4 | PROPERTY_LENGTH | Length of property (LE)
0x8 | 0x1 | PROPERTY_ID | Incremental ID of property
0x9 | 0x1 | UNKNOWN1 | Unknown.  Usually 0x10
0xA | Varies | PROPERTY_NAME_AND_DATA | Plaintext name of property, null terminated, data follows




## ID Assignment

IDs are assigned sequentially by evaluating an entire tree to each of its leaves.  For example, if a tree structure exists like this:

- root (ID 2)
  - branch1 (ID 3)
    - branch1_1 (ID 4)
      - leaf1_1_1 (ID 5)
      - leaf1_1_2 (ID 6)
      - leaf1_1_3 (ID 7)
    - branch1_2 (ID 8)
      - leaf1_2_1 (ID 9)
      - leaf1_2_2 (ID 10)
  - branch2 (ID 11)
    - leaf2_1 (ID 12)
  - branch3 (ID 13)
  - branch4 (ID 14)

## Misc 

- 0x23 0x80 is magic for properties
- 0x14 0x80 is magic for some kind of hash?  Always length 0x20 followed by 24 bytes of garbage?  Tiger192 hash?
