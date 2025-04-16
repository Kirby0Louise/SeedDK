Imports System.IO

Public Class frmMain
    Private Sub cmdSelectFile_Click(sender As Object, e As EventArgs) Handles cmdSelectFile.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "All files (*.*)|*.*"
            ofd.Multiselect = False
            If ofd.ShowDialog() = DialogResult.OK Then
                txtTargetPath.Text = ofd.FileName
            End If
        End Using
    End Sub

    Private Sub cmdSelectDir_Click(sender As Object, e As EventArgs) Handles cmdSelectDir.Click

    End Sub

    Private Sub cmdUnpack_Click(sender As Object, e As EventArgs) Handles cmdUnpack.Click
        If txtOutputPath.Text = "" Then
            txtOutputPath.Text = Application.StartupPath() + "\output"
            'MsgBox("Invalid output path")
            'Exit Sub
        End If
        txtLog.Text = ""
        addLog("Unpacking " + txtTargetPath.Text + " to " + txtOutputPath.Text + "....")

        If txtTargetPath.Text.EndsWith("\") Then
            folderUnpack(txtTargetPath.Text)
        Else
            unpack(txtTargetPath.Text, False)
        End If
    End Sub

    Private Sub unpack(ByVal arcPath As String, ByVal multi As Boolean)
        'File stream
        Dim fs1 As FileStream = File.Open(arcPath, FileMode.Open, FileAccess.Read, FileShare.Write)

        'reader
        Dim reader As BinaryReader = New BinaryReader(fs1)

        'check OFS3 magic
        Dim magicTest(3) As Byte
        Dim magicTest2() As Byte = {&H4F, &H46, &H53, &H33}
        fs1.Read(magicTest, 0, 4)
        If Not magicTest.SequenceEqual(magicTest2) Then
            addLog("Couldn't find OFS3 magic")
            Exit Sub
        End If

        'header stuff
        Dim headerLength As UInt32 = reader.ReadUInt32()
        Dim archiveType As UInt16 = reader.ReadUInt16()
        Dim archiveConfig As UInt16 = reader.ReadUInt16()
        Dim nonHeaderSize As UInt32 = reader.ReadUInt32()
        Dim numFiles As UInt32 = reader.ReadUInt32()

        'track offset in file name table
        Dim fileNameTableLocation As UInt32 = nonHeaderSize + 16
        Dim fileNameTableTracker As UInt32 = fileNameTableLocation
        reader.BaseStream.Seek(-1, SeekOrigin.End)
        Dim totalSize As UInt32 = reader.BaseStream.Position

        For i As Integer = 0 To numFiles - 1
            reader.BaseStream.Seek(20 + i * 8, SeekOrigin.Begin)
            Dim fileOffset As UInt32 = reader.ReadUInt32() + 16
            Dim fileSize As UInt32 = reader.ReadUInt32()
            Dim fileData(fileSize - 1) As Byte

            reader.BaseStream.Seek(fileOffset, SeekOrigin.Begin)
            reader.Read(fileData, 0, fileSize)
            reader.BaseStream.Seek(fileNameTableTracker, SeekOrigin.Begin)

            'remaining fnt to byte array
            Dim tempFileNameArray(totalSize - fileNameTableTracker) As Byte
            reader.Read(tempFileNameArray, 0, tempFileNameArray.Length - 1)

            'Find first index of null terminator
            Dim cutIndex As UInt32 = 0
            For j As Integer = 0 To tempFileNameArray.Length - 1
                If tempFileNameArray(j) = &H0 Then
                    Exit For
                Else
                    cutIndex += 1
                End If
            Next

            Dim finalFileNameArray(cutIndex - 1) As Byte
            reader.BaseStream.Seek(fileNameTableTracker, SeekOrigin.Begin)
            reader.Read(finalFileNameArray, 0, finalFileNameArray.Length)
            fileNameTableTracker += cutIndex + 1

            Dim finalFileName As String = System.Text.Encoding.ASCII.GetString(finalFileNameArray)

            If finalFileName = "" Then
                Continue For
            End If

            File.WriteAllBytes(txtOutputPath.Text + "\" + finalFileName, fileData)

            addLog("Wrote file " + finalFileName)
        Next

        If Not multi Then
            addLog("Finished!!")
        End If
    End Sub

    Private Sub addLog(ByVal message As String)
        If Not txtLog.Text = "" Then
            txtLog.Text += vbNewLine
        End If
        txtLog.Text += message
    End Sub

    Private Sub cmdNoFileNameUnpack_Click(sender As Object, e As EventArgs) Handles cmdNoFileNameUnpack.Click
        If txtOutputPath.Text = "" Then
            txtOutputPath.Text = Application.StartupPath() + "\output"
            'MsgBox("Invalid output path")
            'Exit Sub
        End If
        txtLog.Text = ""
        addLog("Unpacking " + txtTargetPath.Text + " to " + txtOutputPath.Text + "....")

        If txtTargetPath.Text.EndsWith("\") Then
            folderNoFNUnpack(txtTargetPath.Text)
        Else
            noFNUnpack(txtTargetPath.Text, False)
        End If

    End Sub

    Private Sub noFNUnpack(ByVal arcPath As String, ByVal multi As Boolean)
        'File stream
        Dim fs1 As FileStream = File.Open(arcPath, FileMode.Open, FileAccess.Read, FileShare.Write)

        'reader
        Dim reader As BinaryReader = New BinaryReader(fs1)

        'check OFS3 magic
        Dim magicTest(3) As Byte
        Dim magicTest2() As Byte = {&H4F, &H46, &H53, &H33}
        fs1.Read(magicTest, 0, 4)
        If Not magicTest.SequenceEqual(magicTest2) Then
            addLog("Couldn't find OFS3 magic")
            Exit Sub
        End If

        'header stuff
        Dim headerLength As UInt32 = reader.ReadUInt32()
        Dim archiveType As UInt16 = reader.ReadUInt16()
        Dim archiveConfig As UInt16 = reader.ReadUInt16()
        Dim nonHeaderSize As UInt32 = reader.ReadUInt32()
        Dim numFiles As UInt32 = reader.ReadUInt32()

        Dim fileNameInt As UInt32 = 0

        For i As Integer = 0 To numFiles - 1
            reader.BaseStream.Seek(20 + i * 8, SeekOrigin.Begin)
            Dim fileOffset As UInt32 = reader.ReadUInt32() + 16
            Dim fileSize As UInt32 = reader.ReadUInt32()
            Dim fileData(fileSize - 1) As Byte

            reader.BaseStream.Seek(fileOffset, SeekOrigin.Begin)
            reader.Read(fileData, 0, fileSize)

            Dim baseFN As String = arcPath.Substring(arcPath.LastIndexOf("\") + 1) + "_"

            Dim finalFilename As String = baseFN + fileNameInt.ToString()
            fileNameInt += 1

            File.WriteAllBytes(txtOutputPath.Text + "\" + finalFilename, fileData)

            addLog("Wrote file " + finalFilename)
        Next

        If Not multi Then
            addLog("Finished!!")
        End If

    End Sub

    Private Sub folderUnpack(ByVal arcPath As String)
        Dim di As New DirectoryInfo(arcPath)
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim fri As FileInfo
        For Each fri In fiArr
            unpack(fri.FullName, True)
        Next
        addLog("Finished!!")
    End Sub

    Private Sub folderNoFNUnpack(ByVal arcPath As String)
        Dim di As New DirectoryInfo(arcPath)
        Dim fiArr As FileInfo() = di.GetFiles()
        Dim fri As FileInfo
        For Each fri In fiArr
            noFNUnpack(fri.FullName, True)
        Next
        addLog("Finished!!")
    End Sub

    Private Sub cmdQuickScript_Click(sender As Object, e As EventArgs) Handles cmdQuickScript.Click
        'Dim di As New DirectoryInfo(txtOutputPath.Text)
        'Dim fiArr As FileInfo() = di.GetFiles()
        'Dim fri As FileInfo
        'For Each fri In fiArr
        '    If Not fri.FullName.EndsWith("_1") Then
        '        File.Delete(fri.FullName)
        '    End If
        'Next
        addLog("Ran Quick Script!!")
    End Sub
End Class
