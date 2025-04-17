#pragma description SAO HF OMG Model
#pragma magic [4F 4D 47 2E]
#pragma endian big

enum byteMagics : u8 {
	
};

enum shortMagics : u16 {
		hash = 0x1480,
	modelProperty = 0x2380
};

enum intMagics : u32 {
	
};






struct property {
	u32 PROPERTY_MAGIC [[color("BCBD22")]];
	le u32 PROPERTY_LENGTH [[color("FF7F0E")]];
	u8 PROPERTY_ID [[color("17BECF")]];
	u8 UNKNOWN1 [[color("1F77B4")]];
	char PROPERTY_NAME_AND_DATA[PROPERTY_LENGTH - 10] [[color("D62728")]];
};


struct branch {
	le u16 BRANCH_ID [[color("BCBD22")]];
	le u16 BRANCH_DEPTH [[color("FF7F0E")]];
	le u32 BRANCH_LENGTH [[color("17BECF")]];
	le u32 UNKNOWN1 [[color("1F77B4")]];
	le u32 UNKNOWN2 [[color("D62728")]];
	char BRANCH_NAME_AND_DATA[BRANCH_LENGTH - 16] [[color("7F7F7F")]];
};

struct header {
	char MAGIC[4];
	char omgVersion[4];
	char headerPadding[8];
};

header root @ 0x0;