<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtTargetPath = New System.Windows.Forms.TextBox()
        Me.lblTargetPath = New System.Windows.Forms.Label()
        Me.lblOutputPath = New System.Windows.Forms.Label()
        Me.txtOutputPath = New System.Windows.Forms.TextBox()
        Me.cmdUnpack = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.cmdSelectFile = New System.Windows.Forms.Button()
        Me.cmdSelectDir = New System.Windows.Forms.Button()
        Me.cmdNoFileNameUnpack = New System.Windows.Forms.Button()
        Me.cmdQuickScript = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtTargetPath
        '
        Me.txtTargetPath.Location = New System.Drawing.Point(12, 25)
        Me.txtTargetPath.Name = "txtTargetPath"
        Me.txtTargetPath.Size = New System.Drawing.Size(776, 20)
        Me.txtTargetPath.TabIndex = 0
        '
        'lblTargetPath
        '
        Me.lblTargetPath.AutoSize = True
        Me.lblTargetPath.Location = New System.Drawing.Point(12, 9)
        Me.lblTargetPath.Name = "lblTargetPath"
        Me.lblTargetPath.Size = New System.Drawing.Size(63, 13)
        Me.lblTargetPath.TabIndex = 1
        Me.lblTargetPath.Text = "Target Path"
        '
        'lblOutputPath
        '
        Me.lblOutputPath.AutoSize = True
        Me.lblOutputPath.Location = New System.Drawing.Point(12, 80)
        Me.lblOutputPath.Name = "lblOutputPath"
        Me.lblOutputPath.Size = New System.Drawing.Size(64, 13)
        Me.lblOutputPath.TabIndex = 3
        Me.lblOutputPath.Text = "Output Path"
        '
        'txtOutputPath
        '
        Me.txtOutputPath.Location = New System.Drawing.Point(12, 96)
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.Size = New System.Drawing.Size(776, 20)
        Me.txtOutputPath.TabIndex = 2
        '
        'cmdUnpack
        '
        Me.cmdUnpack.Location = New System.Drawing.Point(12, 122)
        Me.cmdUnpack.Name = "cmdUnpack"
        Me.cmdUnpack.Size = New System.Drawing.Size(75, 23)
        Me.cmdUnpack.TabIndex = 4
        Me.cmdUnpack.Text = "Unpack"
        Me.cmdUnpack.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 151)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(776, 317)
        Me.txtLog.TabIndex = 5
        '
        'cmdSelectFile
        '
        Me.cmdSelectFile.Location = New System.Drawing.Point(12, 51)
        Me.cmdSelectFile.Name = "cmdSelectFile"
        Me.cmdSelectFile.Size = New System.Drawing.Size(75, 23)
        Me.cmdSelectFile.TabIndex = 6
        Me.cmdSelectFile.Text = "File"
        Me.cmdSelectFile.UseVisualStyleBackColor = True
        '
        'cmdSelectDir
        '
        Me.cmdSelectDir.Location = New System.Drawing.Point(93, 51)
        Me.cmdSelectDir.Name = "cmdSelectDir"
        Me.cmdSelectDir.Size = New System.Drawing.Size(75, 23)
        Me.cmdSelectDir.TabIndex = 7
        Me.cmdSelectDir.Text = "Folder"
        Me.cmdSelectDir.UseVisualStyleBackColor = True
        '
        'cmdNoFileNameUnpack
        '
        Me.cmdNoFileNameUnpack.Location = New System.Drawing.Point(93, 122)
        Me.cmdNoFileNameUnpack.Name = "cmdNoFileNameUnpack"
        Me.cmdNoFileNameUnpack.Size = New System.Drawing.Size(75, 23)
        Me.cmdNoFileNameUnpack.TabIndex = 8
        Me.cmdNoFileNameUnpack.Text = "No Name"
        Me.cmdNoFileNameUnpack.UseVisualStyleBackColor = True
        '
        'cmdQuickScript
        '
        Me.cmdQuickScript.Location = New System.Drawing.Point(174, 122)
        Me.cmdQuickScript.Name = "cmdQuickScript"
        Me.cmdQuickScript.Size = New System.Drawing.Size(83, 23)
        Me.cmdQuickScript.TabIndex = 9
        Me.cmdQuickScript.Text = "Quick Action"
        Me.cmdQuickScript.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 480)
        Me.Controls.Add(Me.cmdQuickScript)
        Me.Controls.Add(Me.cmdNoFileNameUnpack)
        Me.Controls.Add(Me.cmdSelectDir)
        Me.Controls.Add(Me.cmdSelectFile)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.cmdUnpack)
        Me.Controls.Add(Me.lblOutputPath)
        Me.Controls.Add(Me.txtOutputPath)
        Me.Controls.Add(Me.lblTargetPath)
        Me.Controls.Add(Me.txtTargetPath)
        Me.Name = "frmMain"
        Me.Text = "SAO HF Unpack OFS3"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtTargetPath As TextBox
    Friend WithEvents lblTargetPath As Label
    Friend WithEvents lblOutputPath As Label
    Friend WithEvents txtOutputPath As TextBox
    Friend WithEvents cmdUnpack As Button
    Friend WithEvents txtLog As TextBox
    Friend WithEvents cmdSelectFile As Button
    Friend WithEvents cmdSelectDir As Button
    Friend WithEvents cmdNoFileNameUnpack As Button
    Friend WithEvents cmdQuickScript As Button
End Class
