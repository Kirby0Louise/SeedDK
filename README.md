# EVERYTHING HERE IS A MAJOR WIP!!

# SeedDK
SeedDK is a repository for research, reverse engineering, tooling and editors for Sword Art Online:  Hollow Fragment.
The name is clever too.

## Data Crystal ##
https://datacrystal.tcrf.net/wiki/Sword_Art_Online:_Hollow_Fragment

## Compatibility

Currently SeedDK only officially supports v1.00 of the Asian release.  While you are free to use it on other versions, no official support will be provided and your mileage may vary.  The following table is the currently known cross version support of each of the functions of SeedDK.

Legend -
✅ = Fully supported
🟨 = Partial support
❌ = Unsupported

| Feature | AS (PCSH00070) | JP (PCSG000294) | US (PCSE00465) | EU (PCSB00618) | Re:  PS4 | Re:  PC
| ------------- | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: | :-------------: |
| OFS3 Unpack | 🟨 | 🟨 | 🟨 | 🟨 | ❌ | 🟨 |
| Basic Text Editing | ✅ | ✅ | ✅ | ✅ | ❌ | ✅ |
| Script Editing | 🟨 | ❌ | 🟨 | 🟨 | ❌ | ❌ |
| Texture Editing | ✅* | ✅* | ✅* | ✅* | ❌ | ✅ |
| Model Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Level Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Skill Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Sword Skill Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Quest Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Shader Editing | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Assembly Injection | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |

* PVRTC2 textures are currently handled with Imagination's PVRTexTool, a free tool external to SeedDK.  There are plans to implement native PVRTC2 decode in the future
