Imports System.IO
Imports System.Drawing
Imports System.Text.Encoding
Imports System.Runtime.InteropServices

Module Module1
    Public cmdArgs() As String
    Public trackerValue As UInt32 = 0
    Enum textureTypes As UInt32
        SWIZZLED = 0
        CUBE = &H40000000UI
        LINEAR = &H60000000UI
        TILED = &H80000000UI
        LINEAR_STRIDED = &HC0000000UI
    End Enum

    Enum textureFormats As UInt32
        PVRII4BPP = &H83000000UI
        DXT1 = &H85000000UI
        DXT5 = &H87000000UI
        PALETTE8BPP = &H95000000UI
    End Enum


    Sub Main()
        'About
        Console.Clear()
        Console.WriteLine("VitaGXTUtil v1.0 by Kirby0Louise")
        Console.WriteLine()
        Console.WriteLine("Use -h for help")

        cmdArgs = Environment.GetCommandLineArgs()


        If cmdArgs.Length = 1 Then
            'Ran without any args
        ElseIf cmdArgs.Length = 2 Then
            If cmdArgs(1) = "-h" Then
                'Show help
                Console.WriteLine("Usage:")
                Console.WriteLine("VitaGXTUtil -out <GXT PATH>")
                Console.WriteLine("VitaGXTUtil -out -f <GXT PATH>")
                Console.WriteLine("VitaGXTUtil -in <PNG/DDS PATH>")
                Console.WriteLine("VitaGXTUtil -in -f <PNG/DDS PATH>")
            ElseIf cmdArgs(1) = "-out" Then
                errorExit("ERROR - Did not provide GXT path")
            ElseIf cmdArgs(1) = "-in" Then
                errorExit("ERROR - Did not provide PNG/DDS path")
            Else
                'Did something weird?
                errorExit("ERROR - You did something weird.  Probably invalid arguments.")
            End If
        ElseIf cmdArgs.Length = 3 Then
            'Correct argument length
            If cmdArgs(1) = "-out" Then
                Dim gxtPath As String = cmdArgs(2)
                Console.WriteLine()
                Console.WriteLine("Converting " + gxtPath + " to PC format")
                GXTtoPC(gxtPath, False)
            ElseIf cmdArgs(1) = "-in" Then
                Dim pcPath As String = cmdArgs(2)
                Console.WriteLine()
                Console.WriteLine("Converting " + pcPath + " to GXT format")
                PCtoGXT(pcPath, False)
            Else
                errorExit("ERROR - Invalid 2nd argument.  You can only use -out for GXT to PNG/DDS and -in for PNG/DDS to GXT")
            End If
        ElseIf cmdArgs.Length = 4 Then
            'Correct argument length for folder
            Dim gxtPath As String = cmdArgs(3)
            Console.WriteLine()
            Console.WriteLine("Converting all of " + gxtPath + " to PC format")
            GXTFolderToPC(gxtPath)
        Else
            'too many args
            errorExit("ERROR - TOO MANY ARGUMENTS")
        End If
    End Sub

    Sub errorExit(ByVal errorString As String)
        Console.WriteLine(errorString)
        Console.WriteLine("Press enter to escape")
        Console.ReadLine()
        Environment.Exit(0)
    End Sub

    Sub GXTtoPC(ByVal gxtPath As String, ByVal multi As Boolean)
        Console.WriteLine("--------------------------------------------------------------------------------")
        'Dim gxtBytes() As Byte = File.ReadAllBytes(gxtPath)
        'File stream
        Dim fs1 As FileStream = File.Open(gxtPath, FileMode.Open, FileAccess.Read, FileShare.Write)

        'reader
        Dim reader As BinaryReader = New BinaryReader(fs1)

        'check GXT magic
        Dim magicTest(3) As Byte
        Dim magicTest2() As Byte = {&H47, &H58, &H54, &H0}
        fs1.Read(magicTest, 0, 4)
        If Not magicTest.SequenceEqual(magicTest2) And Not multi Then
            errorExit("ERROR - Couldn't find GXT magic")
            Exit Sub
        End If

        Dim gxtVersion(3) As Byte
        reader.Read(gxtVersion, 0, 4)
        Console.WriteLine("GXT Version - " + byteArrayToHex(gxtVersion))

        Dim numTextures As UInt32 = reader.ReadUInt32()
        Console.WriteLine("Number of Textures - " + numTextures.ToString())

        Dim dataOffset As UInt32 = reader.ReadUInt32()
        Console.WriteLine("Data Offset - 0x" + dataOffset.ToString("X4"))

        Dim dataSize As UInt32 = reader.ReadUInt32()
        Console.WriteLine("Data Size - 0x" + dataSize.ToString("X4"))

        Dim fourbppPalettes As UInt32 = reader.ReadUInt32()
        Console.WriteLine("4bpp Palettes - " + fourbppPalettes.ToString())

        Dim eightbppPalettes As UInt32 = reader.ReadUInt32()
        Console.WriteLine("8bpp Palettes - " + eightbppPalettes.ToString())

        reader.ReadUInt32()

        Dim baseFN As String = gxtPath.Substring(gxtPath.LastIndexOf("\") + 1)
        Dim outFolder As String = Environment.CurrentDirectory + "\out\"
        Console.WriteLine("Dumping metadata to " + outFolder + "tex_" + baseFN + ".meta")

        Dim metadata(dataOffset - 1) As Byte
        reader.BaseStream.Seek(0, SeekOrigin.Begin)
        reader.Read(metadata, 0, dataOffset)
        reader.BaseStream.Seek(32, SeekOrigin.Begin)



        File.WriteAllBytes(outFolder + "tex_" + baseFN + ".meta", metadata)

        Dim inFileName As String = gxtPath.Substring(gxtPath.LastIndexOf("\") + 1)
        For i As UInt32 = 0 To numTextures - 1
            Dim textureName As String = "Texture " + i.ToString()
            Console.WriteLine("--------------------------------------------------------------------------------")

            Dim texOffset As UInt32 = reader.ReadUInt32()
            Console.WriteLine(textureName + " Data Offset - 0x" + texOffset.ToString("X4"))

            Dim texSize As UInt32 = reader.ReadUInt32()
            Console.WriteLine(textureName + " Data Size - 0x" + texSize.ToString("X4"))

            Dim paletteIndex As UInt32 = reader.ReadUInt32()
            Console.WriteLine(textureName + " Palette Index - " + paletteIndex.ToString("X4"))

            Dim texFlags As UInt32 = reader.ReadUInt32()
            Console.WriteLine(textureName + " Flags - " + switchEndian(texFlags).ToString("X4")) 'fix me with binary conversion

            Dim texType As UInt32 = reader.ReadUInt32()
            Dim texTypeString As String
            Select Case texType
                Case textureTypes.SWIZZLED
                    texTypeString = "SWIZZLED"
                Case textureTypes.CUBE
                    texTypeString = "CUBE"
                Case textureTypes.LINEAR
                    texTypeString = "LINEAR"
                Case textureTypes.TILED
                    texTypeString = "TILED"
                Case textureTypes.LINEAR_STRIDED
                    texTypeString = "LINEAR_STRIDED"
                Case Else
                    texTypeString = "ERROR"
                    errorExit("ERROR - Invalid texture type, type value was - " + texType.ToString("X4"))
            End Select
            Console.WriteLine(textureName + " Type - " + texTypeString)

            Dim texFormat As UInt32 = reader.ReadUInt32()
            Dim textFormatString As String
            Select Case texFormat
                Case textureFormats.PVRII4BPP
                    textFormatString = "PVRII4BPP"
                Case textureFormats.DXT1
                    textFormatString = "DXT1"
                Case textureFormats.DXT5
                    textFormatString = "DXT5"
                Case textureFormats.PALETTE8BPP
                    textFormatString = "PALETTE8BPP"
                Case Else
                    textFormatString = "ERROR"
                    errorExit("ERROR - Invalid texture format, format value was - " + texFormat.ToString("X4"))
            End Select
            Console.WriteLine(textureName + " Format - " + textFormatString)

            Dim texWidth As UInt16 = reader.ReadUInt16()
            Console.WriteLine(textureName + " Width - " + texWidth.ToString())

            Dim texHeight As UInt16 = reader.ReadUInt16()
            Console.WriteLine(textureName + " Height - " + texHeight.ToString())

            Dim numMipmaps As UInt16 = reader.ReadUInt16()
            Console.WriteLine(textureName + " Mipmaps - " + (numMipmaps - 1).ToString())

            Console.WriteLine()
            Console.WriteLine()

            reader.ReadUInt16()

            reader.BaseStream.Seek(texOffset, SeekOrigin.Begin)
            Dim tData(texSize - 1) As Byte
            reader.Read(tData, 0, texSize)
            Dim tPal(0) As Byte 'Temporary

            If texFormat = textureFormats.DXT1 Or texFormat = textureFormats.DXT5 Then
                dumpDDS(texType, texFormat, tData, texWidth, texHeight, numMipmaps, i, outFolder, inFileName)
            ElseIf texFormat = textureFormats.PALETTE8BPP Then
                ReDim tPal(1023)
                reader.BaseStream.Seek(texOffset + texSize, SeekOrigin.Begin)
                reader.Read(tPal, 0, 1024)
                dumpPNG(texType, texFormat, tData, tPal, texWidth, texHeight, i, outFolder, inFileName)
            ElseIf texFormat = textureFormats.PVRII4BPP Then
                dumpPVRTCII(texType, texFormat, texWidth, texHeight, i, outFolder, inFileName)
            Else
                errorExit("ERROR - Somehow we got an invalid texture type after getting a valid one.  Make sure nothing is causing a race condition")
            End If

        Next

        Console.WriteLine()
        Console.WriteLine("Finished conversion of GXT to PC!!")
        If Not multi Then
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
        End If

    End Sub

    Private Sub GXTFolderToPC(ByVal gxtPath As String)
        Dim di As New DirectoryInfo(gxtPath)
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim fri As FileInfo
        For Each fri In fiArr
            GXTtoPC(gxtPath + fri.Name, True)
        Next
        Console.WriteLine("Press any key to exit")
        Console.ReadKey()
    End Sub

    Private Function byteArrayToHex(ByRef ByteArray() As Byte) As String
        Dim l As Long
        Dim retStr As String

        For l = LBound(ByteArray) To UBound(ByteArray)
            retStr = retStr & Hex$(ByteArray(l)) & " "
        Next l

        byteArrayToHex = Left$(retStr, Len(retStr) - 1)
    End Function

    Private Function switchEndian(value As UInteger) As UInteger
        Dim result As UInteger = (value And &HFFUI) << 24 Or (value And &HFF00UI) << 8 Or (value And &HFF0000UI) >> 8 Or (value And &HFF000000UI) >> 24
        Return result
    End Function

    Private Function uInt32ToByteArray(value As UInt32)
        Dim result(3) As Byte
        result(0) = (value >> 24) And &HFFUI
        result(1) = (value >> 16) And &HFFUI
        result(2) = (value >> 8) And &HFFUI
        result(3) = value And &HFFUI
        Return result
    End Function

    Private Sub dumpPNG(ByVal tType As UInt32, ByVal tFormat As UInt32, ByVal tData As Byte(), ByVal tPal As Byte(), ByVal tWidth As UInt16, ByVal tHeight As UInt16, ByVal texNum As UInt32, ByVal dumpPath As String, ByVal baseFileName As String)
        Select Case tFormat
            Case textureFormats.PALETTE8BPP
                Dim outBitmap As Bitmap = New Bitmap(tWidth, tHeight, Imaging.PixelFormat.Format32bppArgb)
                For i As UInt32 = 0 To tData.Length - 1
                    Dim shade As Byte = tData(i)
                    Dim pixColor As Color = Color.FromArgb(tPal(shade * 4 + 3), tPal(shade * 4), tPal(shade * 4 + 1), tPal(shade * 4 + 2))

                    Dim u As UInt16 = i Mod tWidth
                    Dim v As UInt16 = i \ tWidth

                    outBitmap.SetPixel(u, v, pixColor)

                Next
                Console.WriteLine("Dumping Texture " + texNum.ToString())
                outBitmap.Save(dumpPath + baseFileName + "_" + texNum.ToString() + ".png", Imaging.ImageFormat.Png)
        End Select
    End Sub

    Private Sub dumpDDS(ByVal tType As UInt32, ByVal tFormat As UInt32, ByVal tData As Byte(), ByVal tWidth As UInt16, ByVal tHeight As UInt16, ByVal mipmaps As UInt16, ByVal texNum As UInt32, ByVal dumpPath As String, ByVal baseFileName As String)
        Select Case tFormat
            Case textureFormats.DXT1
                Select Case tType
                    Case textureTypes.SWIZZLED

                        Dim ddsHeaderArray(127) As Byte
                        ddsHeaderArray(0) = &H44 'D
                        ddsHeaderArray(1) = &H44 'D
                        ddsHeaderArray(2) = &H53 'S
                        ddsHeaderArray(3) = &H20 ' 

                        ddsHeaderArray(4) = &H7C '124 (LE)
                        ddsHeaderArray(5) = &H0
                        ddsHeaderArray(6) = &H0
                        ddsHeaderArray(7) = &H0

                        ddsHeaderArray(8) = &H7 'pitch 0, width 1, height 1, caps 1

                        ddsHeaderArray(9) = &H10 'pixelFormat 1

                        ddsHeaderArray(10) = &HA 'depth 0, linear size 1, mipmap count 1

                        ddsHeaderArray(11) = &H0 'padding

                        ddsHeaderArray(12) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(0) 'height (LE)
                        ddsHeaderArray(13) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(1)
                        ddsHeaderArray(14) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(2)
                        ddsHeaderArray(15) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(3)

                        ddsHeaderArray(16) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(0) 'width (LE)
                        ddsHeaderArray(17) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(1)
                        ddsHeaderArray(18) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(2)
                        ddsHeaderArray(19) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(3)

                        ddsHeaderArray(20) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(0) 'non-header size (LE)
                        ddsHeaderArray(21) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(1)
                        ddsHeaderArray(22) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(2)
                        ddsHeaderArray(23) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(3)

                        ddsHeaderArray(24) = &H1 'depth 1
                        ddsHeaderArray(25) = &H0
                        ddsHeaderArray(26) = &H0
                        ddsHeaderArray(27) = &H0

                        ddsHeaderArray(28) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(0) 'mipmaps (LE)
                        ddsHeaderArray(29) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(1)
                        ddsHeaderArray(30) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(2)
                        ddsHeaderArray(31) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(3)

                        'reserved DWORDS skipped (initializing memory is for scrubs)

                        ddsHeaderArray(76) = &H20 'ddspf size 32 (LE)
                        ddsHeaderArray(77) = &H0
                        ddsHeaderArray(78) = &H0
                        ddsHeaderArray(79) = &H0

                        ddsHeaderArray(80) = &H4 'alpha pixels 0, alpha 0, fourcc 1, uncompressed 0.  These probably map to the GXT flags, but impossible to know which since basically nothing uses it.  Uncompressed should never be set since GXT provides Sony method for non-S3T textures
                        ddsHeaderArray(81) = &H0 'yuv 0, luminance 0
                        ddsHeaderArray(82) = &H0
                        ddsHeaderArray(83) = &H0

                        ddsHeaderArray(84) = &H44 'D
                        ddsHeaderArray(85) = &H58 'X
                        ddsHeaderArray(86) = &H54 'T
                        ddsHeaderArray(87) = &H31 '1

                        'next DWORD is always 0

                        'next 4 DWORDs are probably always 0

                        ddsHeaderArray(108) = &H0 'flags, probably always 0
                        ddsHeaderArray(109) = &H10 'texture 1

                        'rest are probably always 0

                        Dim unSwizzledTData(tData.Length - 1) As Byte
                        'iterate over mipmaps
                        For i As UInt32 = 0 To mipmaps - 1
                            Dim thisWidth As UInt16 = tWidth / 2 ^ i
                            Dim thisHeight As UInt16 = tHeight / 2 ^ i
                            Dim blockCount As UInt32 = CType(thisWidth, UInt32) * CType(thisHeight, UInt32) / 16
                            Dim blockWidth As UInt32 = thisWidth / 4
                            If blockCount = 0 Then
                                blockCount = 1
                                blockWidth = 1
                            End If
                            Dim thisMipmapData() As Byte = unVitaSwizzle(tData, blockCount, 4, trackerValue, blockWidth)

                            unSwizzledTData = manualSubArrayCopy(unSwizzledTData, thisMipmapData, trackerValue)
                            trackerValue = trackerValue + blockCount * 8
                        Next



                        Dim finalData() As Byte = ddsHeaderArray.Concat(unSwizzledTData).ToArray()

                        File.WriteAllBytes(dumpPath + baseFileName + ".dds", finalData)


                End Select
            Case textureFormats.DXT5
                Select Case tType
                    Case textureTypes.SWIZZLED

                        Dim ddsHeaderArray(127) As Byte
                        ddsHeaderArray(0) = &H44 'D
                        ddsHeaderArray(1) = &H44 'D
                        ddsHeaderArray(2) = &H53 'S
                        ddsHeaderArray(3) = &H20 ' 

                        ddsHeaderArray(4) = &H7C '124 (LE)
                        ddsHeaderArray(5) = &H0
                        ddsHeaderArray(6) = &H0
                        ddsHeaderArray(7) = &H0

                        ddsHeaderArray(8) = &H7 'pitch 0, width 1, height 1, caps 1

                        ddsHeaderArray(9) = &H10 'pixelFormat 1

                        ddsHeaderArray(10) = &HA 'depth 0, linear size 1, mipmap count 1

                        ddsHeaderArray(11) = &H0 'padding

                        ddsHeaderArray(12) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(0) 'height (LE)
                        ddsHeaderArray(13) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(1)
                        ddsHeaderArray(14) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(2)
                        ddsHeaderArray(15) = uInt32ToByteArray(switchEndian(CType(tHeight, UInt32)))(3)

                        ddsHeaderArray(16) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(0) 'width (LE)
                        ddsHeaderArray(17) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(1)
                        ddsHeaderArray(18) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(2)
                        ddsHeaderArray(19) = uInt32ToByteArray(switchEndian(CType(tWidth, UInt32)))(3)

                        ddsHeaderArray(20) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(0) 'non-header size (LE)
                        ddsHeaderArray(21) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(1)
                        ddsHeaderArray(22) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(2)
                        ddsHeaderArray(23) = uInt32ToByteArray(switchEndian(CType(tData.Length, UInt32)))(3)

                        ddsHeaderArray(24) = &H1 'depth 1
                        ddsHeaderArray(25) = &H0
                        ddsHeaderArray(26) = &H0
                        ddsHeaderArray(27) = &H0

                        ddsHeaderArray(28) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(0) 'mipmaps (LE)
                        ddsHeaderArray(29) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(1)
                        ddsHeaderArray(30) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(2)
                        ddsHeaderArray(31) = uInt32ToByteArray(switchEndian(CType(mipmaps, UInt32)))(3)

                        'reserved DWORDS skipped (initializing memory is for scrubs)

                        ddsHeaderArray(76) = &H20 'ddspf size 32 (LE)
                        ddsHeaderArray(77) = &H0
                        ddsHeaderArray(78) = &H0
                        ddsHeaderArray(79) = &H0

                        ddsHeaderArray(80) = &H4 'alpha pixels 0, alpha 0, fourcc 1, uncompressed 0.  These probably map to the GXT flags, but impossible to know which since basically nothing uses it.  Uncompressed should never be set since GXT provides Sony method for non-S3T textures
                        ddsHeaderArray(81) = &H0 'yuv 0, luminance 0
                        ddsHeaderArray(82) = &H0
                        ddsHeaderArray(83) = &H0

                        ddsHeaderArray(84) = &H44 'D
                        ddsHeaderArray(85) = &H58 'X
                        ddsHeaderArray(86) = &H54 'T
                        ddsHeaderArray(87) = &H35 '5

                        'next DWORD is always 0

                        'next 4 DWORDs are probably always 0

                        ddsHeaderArray(108) = &H0 'flags, probably always 0
                        ddsHeaderArray(109) = &H10 'texture 1

                        'File.WriteAllBytes(dumpPath + "dds.meta", ddsHeaderArray)

                        'rest are probably always 0

                        Dim unSwizzledTData(tData.Length - 1) As Byte
                        'iterate over mipmaps
                        For i As UInt32 = 0 To mipmaps - 1
                            Dim thisWidth As UInt16 = tWidth / 2 ^ i
                            Dim thisHeight As UInt16 = tHeight / 2 ^ i
                            Dim blockCount As UInt32 = CType(thisWidth, UInt32) * CType(thisHeight, UInt32) / 16
                            Dim blockWidth As UInt32 = thisWidth / 4
                            If blockCount = 0 Then
                                blockCount = 1
                                blockWidth = 1
                            End If
                            Dim thisMipmapData() As Byte = unVitaSwizzle(tData, blockCount, 8, trackerValue, blockWidth)

                            unSwizzledTData = manualSubArrayCopy(unSwizzledTData, thisMipmapData, trackerValue)
                            trackerValue = trackerValue + blockCount * 16
                        Next

                        Dim finalData() As Byte = ddsHeaderArray.Concat(unSwizzledTData).ToArray()

                        File.WriteAllBytes(dumpPath + baseFileName + ".dds", finalData)


                End Select
        End Select
    End Sub

    Private Function unVitaSwizzle(ByVal input As Byte(), ByVal blocks As UInt32, ByVal blockBpp As Integer, ByVal trackerIndex As UInt32, ByVal bWidth As UInt32) As Byte()
        Dim outArraySize As UInt32 = blocks * 16
        If blockBpp = 4 Then
            outArraySize = outArraySize / 2
        End If
        Dim outArray(outArraySize - 1) As Byte

        For i As Integer = 0 To blocks - 1
            Dim correctX As Integer = 0
            Dim correctY As Integer = 0
            blockToXY(i, correctX, correctY)
            Dim blockBytes As Integer = 16 * blockBpp / 8
            Dim sArray(blockBytes - 1) As Byte

            For j As Integer = 0 To sArray.Length - 1
                sArray(j) = input(trackerIndex + i * blockBytes + j)
            Next
            outArray = manualSubArrayCopyTex(outArray, sArray, correctX, correctY, blockBytes, bWidth)
        Next
        Return outArray
    End Function

    Private Sub blockToXY(ByVal value As UInt32, <Out> ByRef x As UInteger, <Out> ByRef y As UInteger)
        x = compactBits(value >> 1)
        y = compactBits(value)
    End Sub

    Private Function compactBits(ByVal value As UInt32) As UInt32
        value = value And &H55555555UI
        value = (value Xor value >> 1) And &H33333333
        value = (value Xor value >> 2) And &HF0F0F0F
        value = (value Xor value >> 4) And &HFF00FF
        value = (value Xor value >> 8) And &HFFFF
        Return value
    End Function

    Private Function interleaveBits(ByVal x As UInt32, ByVal y As UInt32) As UInt32
        Dim z As System.UInt32 = 0
        For i As Integer = 0 To 31
            z = z Or (y And 1UI << i) << i Or (x And 1UI << i) << i + 1
        Next
        Return z
    End Function

    Private Function manualSubArrayCopyTex(ByVal target As Byte(), ByVal values As Byte(), ByVal x As Integer, ByVal y As Integer, ByVal blockBytes As Integer, ByVal bwidth As UInt32) As Byte()
        For i As Integer = 0 To blockBytes - 1
            Dim whereWrite As Integer = y * bwidth * blockBytes + x * blockBytes + i
            target(y * bwidth * blockBytes + x * blockBytes + i) = values(i)
        Next
        Return target
    End Function

    Private Function manualSubArrayCopy(ByVal target As Byte(), ByVal values As Byte(), ByVal pos As Integer) As Byte()
        For i As Integer = 0 To values.Length - 1
            target(pos + i) = values(i)
        Next
        Return target
    End Function

    Private Sub PCtoGXT(ByVal pcPath As String, ByVal multi As Boolean)
        Dim metaPath As String = pcPath.Substring(0, pcPath.LastIndexOf(".")) + ".gxt.meta"

        If Not File.Exists(metaPath) Then
            errorExit("ERROR - Texture is missing metadata.  Convert the original file with VitaGXTUtil or create it by hand.")
        End If

        'File stream
        Dim fs1 As FileStream = File.Open(metaPath, FileMode.Open, FileAccess.Read, FileShare.Write)

        'reader
        Dim reader As BinaryReader = New BinaryReader(fs1)

        'copy metadata to array
        Dim metaArray(reader.BaseStream.Length - 1) As Byte
        reader.Read(metaArray, 0, 80)

        reader.BaseStream.Seek(52, SeekOrigin.Begin)

        'get texture info
        Dim texFormat As UInt32 = reader.ReadUInt32()
        Dim tWidth As UInt16 = reader.ReadUInt16()
        Dim tHeight As UInt16 = reader.ReadUInt16()

        'setup data array
        Dim dataArray(CType(tWidth, UInt32) * CType(tHeight, UInt32) - 1) As Byte

        Select Case texFormat
            Case textureFormats.DXT1

            Case textureFormats.DXT5

            Case textureFormats.PALETTE8BPP
                Dim palArray(1023) As Byte
                Dim inBitmap As Bitmap = New Bitmap(tWidth, tHeight, Imaging.PixelFormat.Format32bppArgb)
                Dim texBitmap As Bitmap = New Bitmap(pcPath)

                'zero color bugfix
                Dim zeroColor As Boolean = False

                'read in each pixel, check against palette, recording new palette data if needed, and erroring out if too many colors
                Dim palTracker As Integer = -1
                For i As Integer = 0 To texBitmap.Height - 1
                    For j As Integer = 0 To texBitmap.Width - 1
                        'get colors
                        Dim targetR As Byte = texBitmap.GetPixel(j, i).R
                        Dim targetG As Byte = texBitmap.GetPixel(j, i).G
                        Dim targetB As Byte = texBitmap.GetPixel(j, i).B
                        Dim targetA As Byte = texBitmap.GetPixel(j, i).A

                        'loop through palette to find first matching index, or add if not present
                        Dim found As Boolean = False
                        Dim palIndex As Integer = 0
                        For k As Integer = 0 To palArray.Length - 1 Step 4
                            If (targetR = palArray(k) And targetG = palArray(k + 1) And targetB = palArray(k + 2) And targetA = palArray(k + 3)) Then
                                found = True
                                palIndex = k / 4
                                Exit For
                            End If
                        Next

                        If found And targetA = 0 And Not zeroColor Then
                            palTracker += 1
                            zeroColor = True
                        ElseIf Not found Then
                            palTracker += 1
                        End If

                        'check for palette overflow
                        If palTracker > 256 Then
                            'File.WriteAllBytes(pcPath + ".dp", palArray)
                            errorExit("ERROR - Palette Overflow.  Your image must contain at most 256 colors")
                        End If

                        If Not found Then
                            'now we have palette index, write it to data array
                            dataArray(i * tWidth + j) = palTracker

                            'and write color to palette array
                            palArray(palTracker * 4) = targetR
                            palArray(palTracker * 4 + 1) = targetG
                            palArray(palTracker * 4 + 2) = targetB
                            palArray(palTracker * 4 + 3) = targetA

                            Console.WriteLine("Palette Entry " + palTracker.ToString("X2") + " - " + targetR.ToString("X2") + " " + targetG.ToString("X2") + " " + targetB.ToString("X2") + " " + targetA.ToString("X2"))
                        Else
                            'now we have palette index, write it to data array
                            dataArray(i * tWidth + j) = palIndex
                        End If


                    Next
                Next

                'Combine all arrays
                Dim finalArray1() As Byte = metaArray.Concat(dataArray).ToArray()
                Dim finalArray2() As Byte = finalArray1.Concat(palArray).ToArray()

                'write output
                Dim baseFN As String = pcPath.Substring(pcPath.LastIndexOf("\") + 1)
                Dim correctFN As String = baseFN.Substring(0, baseFN.LastIndexOf("."))
                Dim outFolder As String = Environment.CurrentDirectory + "\out\"
                Dim finalPath As String = Environment.CurrentDirectory + "\out\" + correctFN + ".gxt"
                Console.WriteLine("Wrote " + palTracker.ToString() + " colors to " + finalPath.Substring(finalPath.LastIndexOf("\") + 1))
                File.WriteAllBytes(finalPath, finalArray2)

        End Select

        Console.WriteLine()
        Console.WriteLine("Finished conversion of PC to GXT!!")
        If Not multi Then
            Console.WriteLine("Press any key to exit")
            Console.ReadKey()
        End If
    End Sub

End Module
