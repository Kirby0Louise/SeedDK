# SAO HF BRTNC Format

BRTNC is an text array format containing the strings used to play back a VN-style cutscene

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | UNKNOWN1 | Unknown, usually 1? (LE)
0x4 | 0x4 | PART | Which part of the cutscene data this is.  0 = .dat?, 1 = BRTNC, 2 = BRTEH (LE)
0x8 | 0x4 | TOTAL_STRINGS | Total number of strings in this array (LE)
0xC | 0x4 | UNKNOWN2 | Unknown
0x10 | 0x4 | UNKNOWN3 | Unknown
0x14 | 0x4 | UNKNOWN4 | Unknown
0x18 | 0x4 | UNKNOWN5 | Unknown
0x1C | 0x4 | UNKNOWN6 | Unknown
0x20 | 0x4 | TOTAL_STRINGS2 | Total number of strings in this array (LE)
0x24 | 0x8 * TOTAL_STRINGS | ARRAY_TABLE | String array position and offset from start of array (LE)
