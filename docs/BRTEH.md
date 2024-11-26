# SAO HF BRTEH Format

BRTEH is an text array format containing the strings used to play back a VN-style cutscene

| Address | Size | Variable Name | Description
|--|--|--|--|
0x0 | 0x4 | UNKNOWN1 | Unknown, usually 1? (LE)
0x4 | 0x4 | PART | Which part of the cutscene data this is.  0 = .dat?, 1 = BRTNC, 2 = BRTEH (LE)
0x8 | 0x8 | CONST_COMMON | "common\u0000\u0000"
0x10 | 0x1C | UNKNOWN2 | Unknown
0x2C | 0x4 | TOTAL_STRINGS | Total number of strings in this array (LE)
0x30 | 0x8 * TOTAL_STRINGS | ARRAY_TABLE | String array position and offset from start of array (LE)
