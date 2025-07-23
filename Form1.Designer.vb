<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadCsvSimulationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.lbInferredChannelMoves = New System.Windows.Forms.ListBox()
        Me.lbObservedWifiEvents = New System.Windows.Forms.ListBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.tbThresholdResult = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.tbDetectionAcc = New System.Windows.Forms.TextBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.tbBandwidth = New System.Windows.Forms.TextBox()
        Me.tbSampRate = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.tbLastChannel = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tbBBGain = New System.Windows.Forms.TextBox()
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
        Me.GroupBox6.SuspendLayout()
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
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenFileToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenFileToolStripMenuItem
        '
        Me.OpenFileToolStripMenuItem.Name = "OpenFileToolStripMenuItem"
        Me.OpenFileToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.OpenFileToolStripMenuItem.Text = "Open File"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadCsvSimulationToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(47, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'LoadCsvSimulationToolStripMenuItem
        '
        Me.LoadCsvSimulationToolStripMenuItem.Name = "LoadCsvSimulationToolStripMenuItem"
        Me.LoadCsvSimulationToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.LoadCsvSimulationToolStripMenuItem.Text = "Load Recorded Event"
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
        Me.btnThreshold.Location = New System.Drawing.Point(10, 251)
        Me.btnThreshold.Name = "btnThreshold"
        Me.btnThreshold.Size = New System.Drawing.Size(83, 24)
        Me.btnThreshold.TabIndex = 1
        Me.btnThreshold.Text = "Set Threshold"
        Me.btnThreshold.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(82, 228)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(20, 13)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "dB"
        '
        'tbThreshold
        '
        Me.tbThreshold.Location = New System.Drawing.Point(10, 225)
        Me.tbThreshold.Name = "tbThreshold"
        Me.tbThreshold.Size = New System.Drawing.Size(66, 20)
        Me.tbThreshold.TabIndex = 18
        Me.tbThreshold.Text = "-64"
        Me.tbThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(22, 209)
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
        Me.GroupBox6.Controls.Add(Me.lbInferredChannelMoves)
        Me.GroupBox6.Controls.Add(Me.lbObservedWifiEvents)
        Me.GroupBox6.Controls.Add(Me.Label24)
        Me.GroupBox6.Controls.Add(Me.tbThresholdResult)
        Me.GroupBox6.Controls.Add(Me.Label12)
        Me.GroupBox6.Controls.Add(Me.Label23)
        Me.GroupBox6.Controls.Add(Me.Label13)
        Me.GroupBox6.Controls.Add(Me.tbDetectionAcc)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(6, 285)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(802, 194)
        Me.GroupBox6.TabIndex = 5
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Radar Detection and System Switching"
        '
        'lbInferredChannelMoves
        '
        Me.lbInferredChannelMoves.FormattingEnabled = True
        Me.lbInferredChannelMoves.ItemHeight = 20
        Me.lbInferredChannelMoves.Location = New System.Drawing.Point(22, 135)
        Me.lbInferredChannelMoves.Name = "lbInferredChannelMoves"
        Me.lbInferredChannelMoves.Size = New System.Drawing.Size(754, 44)
        Me.lbInferredChannelMoves.TabIndex = 27
        '
        'lbObservedWifiEvents
        '
        Me.lbObservedWifiEvents.FormattingEnabled = True
        Me.lbObservedWifiEvents.ItemHeight = 20
        Me.lbObservedWifiEvents.Location = New System.Drawing.Point(22, 85)
        Me.lbObservedWifiEvents.Name = "lbObservedWifiEvents"
        Me.lbObservedWifiEvents.Size = New System.Drawing.Size(754, 44)
        Me.lbObservedWifiEvents.TabIndex = 26
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(753, 42)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(23, 20)
        Me.Label24.TabIndex = 25
        Me.Label24.Text = "%"
        '
        'tbThresholdResult
        '
        Me.tbThresholdResult.Location = New System.Drawing.Point(190, 42)
        Me.tbThresholdResult.Name = "tbThresholdResult"
        Me.tbThresholdResult.Size = New System.Drawing.Size(145, 26)
        Me.tbThresholdResult.TabIndex = 12
        Me.tbThresholdResult.Text = "-64"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(18, 45)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(127, 20)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "Signal Threshold"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(341, 45)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(42, 20)
        Me.Label23.TabIndex = 24
        Me.Label23.Text = "dBm"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(430, 42)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(147, 20)
        Me.Label13.TabIndex = 13
        Me.Label13.Text = "Detection Accuracy"
        '
        'tbDetectionAcc
        '
        Me.tbDetectionAcc.Location = New System.Drawing.Point(602, 39)
        Me.tbDetectionAcc.Name = "tbDetectionAcc"
        Me.tbDetectionAcc.Size = New System.Drawing.Size(145, 26)
        Me.tbDetectionAcc.TabIndex = 14
        Me.tbDetectionAcc.Text = "0,9879"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Label22)
        Me.GroupBox7.Controls.Add(Me.Label21)
        Me.GroupBox7.Controls.Add(Me.Label20)
        Me.GroupBox7.Controls.Add(Me.Label19)
        Me.GroupBox7.Controls.Add(Me.Label18)
        Me.GroupBox7.Controls.Add(Me.Label17)
        Me.GroupBox7.Controls.Add(Me.TextBox4)
        Me.GroupBox7.Controls.Add(Me.tbBandwidth)
        Me.GroupBox7.Controls.Add(Me.tbSampRate)
        Me.GroupBox7.Controls.Add(Me.Label14)
        Me.GroupBox7.Controls.Add(Me.Label15)
        Me.GroupBox7.Controls.Add(Me.Label16)
        Me.GroupBox7.Controls.Add(Me.tbLastChannel)
        Me.GroupBox7.Controls.Add(Me.Label11)
        Me.GroupBox7.Controls.Add(Me.tbBBGain)
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
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(704, 146)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(37, 20)
        Me.Label22.TabIndex = 23
        Me.Label22.Text = "Sec"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(704, 109)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(42, 20)
        Me.Label21.TabIndex = 22
        Me.Label21.Text = "MHz"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(704, 80)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(54, 20)
        Me.Label20.TabIndex = 21
        Me.Label20.Text = "MSPS"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(306, 143)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(29, 20)
        Me.Label19.TabIndex = 20
        Me.Label19.Text = "dB"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(306, 109)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(29, 20)
        Me.Label18.TabIndex = 19
        Me.Label18.Text = "dB"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(306, 77)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(29, 20)
        Me.Label17.TabIndex = 18
        Me.Label17.Text = "dB"
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(553, 143)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(145, 26)
        Me.TextBox4.TabIndex = 17
        Me.TextBox4.Text = "1"
        '
        'tbBandwidth
        '
        Me.tbBandwidth.Location = New System.Drawing.Point(553, 109)
        Me.tbBandwidth.Name = "tbBandwidth"
        Me.tbBandwidth.Size = New System.Drawing.Size(145, 26)
        Me.tbBandwidth.TabIndex = 16
        Me.tbBandwidth.Text = "20"
        '
        'tbSampRate
        '
        Me.tbSampRate.Location = New System.Drawing.Point(553, 77)
        Me.tbSampRate.Name = "tbSampRate"
        Me.tbSampRate.Size = New System.Drawing.Size(145, 26)
        Me.tbSampRate.TabIndex = 15
        Me.tbSampRate.Text = "20"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(430, 146)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(120, 20)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "Response Time"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(430, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(84, 20)
        Me.Label15.TabIndex = 13
        Me.Label15.Text = "Bandwidth"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(430, 77)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(102, 20)
        Me.Label16.TabIndex = 12
        Me.Label16.Text = "Sample Rate"
        '
        'tbLastChannel
        '
        Me.tbLastChannel.Location = New System.Drawing.Point(553, 39)
        Me.tbLastChannel.Name = "tbLastChannel"
        Me.tbLastChannel.Size = New System.Drawing.Size(228, 26)
        Me.tbLastChannel.TabIndex = 11
        Me.tbLastChannel.Text = "Channel 52 (5260 MHz, DFS)"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(430, 45)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(103, 20)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Last Channel"
        '
        'tbBBGain
        '
        Me.tbBBGain.Location = New System.Drawing.Point(149, 140)
        Me.tbBBGain.Name = "tbBBGain"
        Me.tbBBGain.Size = New System.Drawing.Size(145, 26)
        Me.tbBBGain.TabIndex = 9
        Me.tbBBGain.Text = "62"
        '
        'tbVGAGain
        '
        Me.tbVGAGain.Location = New System.Drawing.Point(149, 106)
        Me.tbVGAGain.Name = "tbVGAGain"
        Me.tbVGAGain.Size = New System.Drawing.Size(145, 26)
        Me.tbVGAGain.TabIndex = 8
        '
        'tbLNAGain
        '
        Me.tbLNAGain.Location = New System.Drawing.Point(149, 74)
        Me.tbLNAGain.Name = "tbLNAGain"
        Me.tbLNAGain.Size = New System.Drawing.Size(145, 26)
        Me.tbLNAGain.TabIndex = 7
        '
        'tbCurrentCh
        '
        Me.tbCurrentCh.Location = New System.Drawing.Point(149, 42)
        Me.tbCurrentCh.Name = "tbCurrentCh"
        Me.tbCurrentCh.Size = New System.Drawing.Size(228, 26)
        Me.tbCurrentCh.TabIndex = 6
        Me.tbCurrentCh.Text = "Channel 36 (5180 MHz)"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 146)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 20)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "BB Gain"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 112)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 20)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "IF Gain"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 77)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 20)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "RF Gain"
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
        Me.Text = "Dynamic Frequency Selection Validation Software"
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
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
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
    Friend WithEvents tbBBGain As TextBox
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
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents tbLastChannel As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents tbDetectionAcc As TextBox
    Friend WithEvents tbThresholdResult As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents tbBandwidth As TextBox
    Friend WithEvents tbSampRate As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents lbObservedWifiEvents As ListBox
    Friend WithEvents lbInferredChannelMoves As ListBox
    Friend WithEvents OpenFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadCsvSimulationToolStripMenuItem As ToolStripMenuItem
End Class
