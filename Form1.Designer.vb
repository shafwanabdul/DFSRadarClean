<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.zg1 = New ZedGraph.ZedGraphControl()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnResetMax = New System.Windows.Forms.Button()
        Me.btnResetMin = New System.Windows.Forms.Button()
        Me.btnThreshold = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbThreshold = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkAverage = New System.Windows.Forms.CheckBox()
        Me.chkMaxHold = New System.Windows.Forms.CheckBox()
        Me.chkMinHold = New System.Windows.Forms.CheckBox()
        Me.chkMainCurve = New System.Windows.Forms.CheckBox()
        Me.chkSmooth = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnStopSweep = New System.Windows.Forms.Button()
        Me.btnStartSweep = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.tbCurrentChDisplay = New System.Windows.Forms.TextBox()
        Me.cbCurrentCh = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblRadarStatus = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.tbAMPStatus = New System.Windows.Forms.TextBox()
        Me.tbVGAGain = New System.Windows.Forms.TextBox()
        Me.tbLNAGain = New System.Windows.Forms.TextBox()
        Me.tbCurrentCh = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.TSCheckDevice = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(822, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(0, 27)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(822, 511)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(814, 485)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Setting"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.zg1)
        Me.GroupBox4.Controls.Add(Me.GroupBox5)
        Me.GroupBox4.Location = New System.Drawing.Point(6, 112)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(802, 367)
        Me.GroupBox4.TabIndex = 17
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Spectrum Display"
        '
        'zg1
        '
        Me.zg1.Location = New System.Drawing.Point(126, 19)
        Me.zg1.Name = "zg1"
        Me.zg1.ScrollGrace = 0R
        Me.zg1.ScrollMaxX = 0R
        Me.zg1.ScrollMaxY = 0R
        Me.zg1.ScrollMaxY2 = 0R
        Me.zg1.ScrollMinX = 0R
        Me.zg1.ScrollMinY = 0R
        Me.zg1.ScrollMinY2 = 0R
        Me.zg1.Size = New System.Drawing.Size(670, 342)
        Me.zg1.TabIndex = 3
        Me.zg1.UseExtendedPrintDialog = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.btnResetMax)
        Me.GroupBox5.Controls.Add(Me.btnResetMin)
        Me.GroupBox5.Controls.Add(Me.btnThreshold)
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.tbThreshold)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.chkAverage)
        Me.GroupBox5.Controls.Add(Me.chkMaxHold)
        Me.GroupBox5.Controls.Add(Me.chkMinHold)
        Me.GroupBox5.Controls.Add(Me.chkMainCurve)
        Me.GroupBox5.Controls.Add(Me.chkSmooth)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 19)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(114, 342)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Graph Setting"
        '
        'btnResetMax
        '
        Me.btnResetMax.Location = New System.Drawing.Point(17, 123)
        Me.btnResetMax.Name = "btnResetMax"
        Me.btnResetMax.Size = New System.Drawing.Size(75, 23)
        Me.btnResetMax.TabIndex = 21
        Me.btnResetMax.Text = "Reset"
        Me.btnResetMax.UseVisualStyleBackColor = True
        '
        'btnResetMin
        '
        Me.btnResetMin.Location = New System.Drawing.Point(17, 71)
        Me.btnResetMin.Name = "btnResetMin"
        Me.btnResetMin.Size = New System.Drawing.Size(75, 23)
        Me.btnResetMin.TabIndex = 20
        Me.btnResetMin.Text = "Reset"
        Me.btnResetMin.UseVisualStyleBackColor = True
        '
        'btnThreshold
        '
        Me.btnThreshold.Location = New System.Drawing.Point(12, 289)
        Me.btnThreshold.Name = "btnThreshold"
        Me.btnThreshold.Size = New System.Drawing.Size(83, 24)
        Me.btnThreshold.TabIndex = 1
        Me.btnThreshold.Text = "Set Threshold"
        Me.btnThreshold.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(83, 256)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(20, 13)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "dB"
        '
        'tbThreshold
        '
        Me.tbThreshold.Location = New System.Drawing.Point(11, 253)
        Me.tbThreshold.Name = "tbThreshold"
        Me.tbThreshold.Size = New System.Drawing.Size(66, 20)
        Me.tbThreshold.TabIndex = 18
        Me.tbThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 227)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Threshold"
        '
        'chkAverage
        '
        Me.chkAverage.AutoSize = True
        Me.chkAverage.Location = New System.Drawing.Point(12, 158)
        Me.chkAverage.Name = "chkAverage"
        Me.chkAverage.Size = New System.Drawing.Size(66, 17)
        Me.chkAverage.TabIndex = 10
        Me.chkAverage.Text = "Average"
        Me.chkAverage.UseVisualStyleBackColor = True
        '
        'chkMaxHold
        '
        Me.chkMaxHold.AutoSize = True
        Me.chkMaxHold.Location = New System.Drawing.Point(12, 100)
        Me.chkMaxHold.Name = "chkMaxHold"
        Me.chkMaxHold.Size = New System.Drawing.Size(71, 17)
        Me.chkMaxHold.TabIndex = 9
        Me.chkMaxHold.Text = "Max Hold"
        Me.chkMaxHold.UseVisualStyleBackColor = True
        '
        'chkMinHold
        '
        Me.chkMinHold.AutoSize = True
        Me.chkMinHold.Location = New System.Drawing.Point(12, 48)
        Me.chkMinHold.Name = "chkMinHold"
        Me.chkMinHold.Size = New System.Drawing.Size(68, 17)
        Me.chkMinHold.TabIndex = 8
        Me.chkMinHold.Text = "Min Hold"
        Me.chkMinHold.UseVisualStyleBackColor = True
        '
        'chkMainCurve
        '
        Me.chkMainCurve.AutoSize = True
        Me.chkMainCurve.Checked = True
        Me.chkMainCurve.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMainCurve.Location = New System.Drawing.Point(12, 25)
        Me.chkMainCurve.Name = "chkMainCurve"
        Me.chkMainCurve.Size = New System.Drawing.Size(80, 17)
        Me.chkMainCurve.TabIndex = 7
        Me.chkMainCurve.Text = "Main Trace"
        Me.chkMainCurve.UseVisualStyleBackColor = True
        '
        'chkSmooth
        '
        Me.chkSmooth.AutoSize = True
        Me.chkSmooth.Location = New System.Drawing.Point(12, 181)
        Me.chkSmooth.Name = "chkSmooth"
        Me.chkSmooth.Size = New System.Drawing.Size(76, 17)
        Me.chkSmooth.TabIndex = 6
        Me.chkSmooth.Text = "Smoothing"
        Me.chkSmooth.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnStopSweep)
        Me.GroupBox3.Controls.Add(Me.btnStartSweep)
        Me.GroupBox3.Location = New System.Drawing.Point(608, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 100)
        Me.GroupBox3.TabIndex = 16
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Control"
        '
        'btnStopSweep
        '
        Me.btnStopSweep.Location = New System.Drawing.Point(104, 27)
        Me.btnStopSweep.Name = "btnStopSweep"
        Me.btnStopSweep.Size = New System.Drawing.Size(85, 50)
        Me.btnStopSweep.TabIndex = 2
        Me.btnStopSweep.Text = "Stop Sweeping"
        Me.btnStopSweep.UseVisualStyleBackColor = True
        '
        'btnStartSweep
        '
        Me.btnStartSweep.Location = New System.Drawing.Point(11, 28)
        Me.btnStartSweep.Name = "btnStartSweep"
        Me.btnStartSweep.Size = New System.Drawing.Size(85, 50)
        Me.btnStartSweep.TabIndex = 1
        Me.btnStartSweep.Text = "Start Sweeping"
        Me.btnStartSweep.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tbCurrentChDisplay)
        Me.GroupBox2.Controls.Add(Me.cbCurrentCh)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.lblRadarStatus)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(156, 6)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(446, 100)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Frequency Setting"
        '
        'tbCurrentChDisplay
        '
        Me.tbCurrentChDisplay.Location = New System.Drawing.Point(282, 26)
        Me.tbCurrentChDisplay.Name = "tbCurrentChDisplay"
        Me.tbCurrentChDisplay.Size = New System.Drawing.Size(158, 20)
        Me.tbCurrentChDisplay.TabIndex = 16
        '
        'cbCurrentCh
        '
        Me.cbCurrentCh.FormattingEnabled = True
        Me.cbCurrentCh.Location = New System.Drawing.Point(282, 62)
        Me.cbCurrentCh.Name = "cbCurrentCh"
        Me.cbCurrentCh.Size = New System.Drawing.Size(157, 21)
        Me.cbCurrentCh.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(185, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Unused Channels"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(185, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Current Channel"
        '
        'lblRadarStatus
        '
        Me.lblRadarStatus.AutoSize = True
        Me.lblRadarStatus.Location = New System.Drawing.Point(22, 59)
        Me.lblRadarStatus.Name = "lblRadarStatus"
        Me.lblRadarStatus.Size = New System.Drawing.Size(128, 13)
        Me.lblRadarStatus.TabIndex = 12
        Me.lblRadarStatus.Text = "No Interference Detected"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(27, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(116, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Interference Status"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnCheck)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(144, 100)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection Setting"
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(18, 29)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(117, 56)
        Me.btnCheck.TabIndex = 0
        Me.btnCheck.Text = "Check Device"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox6)
        Me.TabPage2.Controls.Add(Me.GroupBox7)
        Me.TabPage2.Controls.Add(Me.Label1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(814, 485)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Report"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(6, 285)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(802, 194)
        Me.GroupBox6.TabIndex = 5
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Radar Detection and Predition"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.tbAMPStatus)
        Me.GroupBox7.Controls.Add(Me.tbVGAGain)
        Me.GroupBox7.Controls.Add(Me.tbLNAGain)
        Me.GroupBox7.Controls.Add(Me.tbCurrentCh)
        Me.GroupBox7.Controls.Add(Me.Label8)
        Me.GroupBox7.Controls.Add(Me.Label7)
        Me.GroupBox7.Controls.Add(Me.Label6)
        Me.GroupBox7.Controls.Add(Me.Label5)
        Me.GroupBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox7.Location = New System.Drawing.Point(6, 76)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(802, 203)
        Me.GroupBox7.TabIndex = 4
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Hardware Setting Specifications"
        '
        'tbAMPStatus
        '
        Me.tbAMPStatus.Location = New System.Drawing.Point(175, 143)
        Me.tbAMPStatus.Name = "tbAMPStatus"
        Me.tbAMPStatus.Size = New System.Drawing.Size(145, 26)
        Me.tbAMPStatus.TabIndex = 9
        '
        'tbVGAGain
        '
        Me.tbVGAGain.Location = New System.Drawing.Point(175, 109)
        Me.tbVGAGain.Name = "tbVGAGain"
        Me.tbVGAGain.Size = New System.Drawing.Size(145, 26)
        Me.tbVGAGain.TabIndex = 8
        '
        'tbLNAGain
        '
        Me.tbLNAGain.Location = New System.Drawing.Point(175, 74)
        Me.tbLNAGain.Name = "tbLNAGain"
        Me.tbLNAGain.Size = New System.Drawing.Size(145, 26)
        Me.tbLNAGain.TabIndex = 7
        '
        'tbCurrentCh
        '
        Me.tbCurrentCh.Location = New System.Drawing.Point(175, 39)
        Me.tbCurrentCh.Name = "tbCurrentCh"
        Me.tbCurrentCh.Size = New System.Drawing.Size(228, 26)
        Me.tbCurrentCh.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 146)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 20)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "Amplifier"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 112)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 20)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "VGA Gain"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 77)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 20)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "LNA Gain"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(125, 20)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Current Channel"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(96, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(623, 31)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Dynamic Frequency Selection Scanning Result"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSCheckDevice, Me.ToolStripStatusLabel2, Me.lblStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 539)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(822, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'TSCheckDevice
        '
        Me.TSCheckDevice.Name = "TSCheckDevice"
        Me.TSCheckDevice.Size = New System.Drawing.Size(132, 17)
        Me.TSCheckDevice.Text = "HackRF Status:  Waiting"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(22, 17)
        Me.ToolStripStatusLabel2.Text = "  |  "
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(101, 17)
        Me.lblStatus.Text = "Sweep Status: Idle"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(822, 561)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnCheck As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblRadarStatus As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cbCurrentCh As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents btnStopSweep As Button
    Friend WithEvents btnStartSweep As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents zg1 As ZedGraph.ZedGraphControl
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents chkAverage As CheckBox
    Friend WithEvents chkMaxHold As CheckBox
    Friend WithEvents chkMinHold As CheckBox
    Friend WithEvents chkMainCurve As CheckBox
    Friend WithEvents chkSmooth As CheckBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents TSCheckDevice As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents lblStatus As ToolStripStatusLabel
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents tbCurrentCh As TextBox
    Friend WithEvents tbAMPStatus As TextBox
    Friend WithEvents tbVGAGain As TextBox
    Friend WithEvents tbLNAGain As TextBox
    Friend WithEvents tbCurrentChDisplay As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents btnThreshold As Button
    Friend WithEvents Label10 As Label
    Friend WithEvents tbThreshold As TextBox
    Friend WithEvents btnResetMax As Button
    Friend WithEvents btnResetMin As Button
End Class
