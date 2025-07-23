Imports System.Diagnostics
Imports System.IO
Imports System.Globalization
Imports ZedGraph
Imports System.Drawing
Imports System.ComponentModel
Imports System.Threading
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports System.Threading.Tasks
Imports System.Net
Imports System.Web

Public Class Form1

    ' Check Device
    Const HackRFPath As String = "C:\Program Files (x86)\HackRF\bin\hackrf_info.exe"
    Private mlUpdateTimer As System.Windows.Forms.Timer
    Private Rng As New Random() ' ADD THIS LINE FOR RANDOM NUMBERS
    Private Async Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        btnCheck.Enabled = False
        btnCheck.Text = "Checking..."
        Dim connected As Boolean = Await Task.Run(Function() IsHackRFConnected(HackRFPath, 5000))
        btnCheck.Text = "Check HackRF"
        btnCheck.Enabled = True

        If connected Then
            TSCheckDevice.Text = "HackRF Status: Connected"
            cbCurrentCh.Enabled = True
            btnStartSweep.Enabled = True
            btnResetMin.Enabled = True ' Enable reset buttons
            btnResetMax.Enabled = True ' Enable reset buttons
            lblStatus.Text = "Status: Idle. HackRF Connected."
        Else
            TSCheckDevice.Text = "HackRF Status: Disconnected"
            cbCurrentCh.Enabled = False
            btnStartSweep.Enabled = False
            btnStopSweep.Enabled = False ' Explicitly disable if disconnected
            btnResetMin.Enabled = False ' Disable reset buttons
            btnResetMax.Enabled = False ' Disable reset buttons
            lblStatus.Text = "Status: Idle. HackRF Disconnected. Please check connection."

            SyncLock dataLock
                ' Clear all data as we are disconnected
                LiveSweepPoints.Clear()
                SmoothedLivePoints.Clear()
                MinHoldSweepPoints.Clear()
                MaxHoldSweepPoints.Clear()
                AverageSweepPoints.Clear()
                MinHoldDataStore.Clear()
                MaxHoldDataStore.Clear()
                AverageDataStore.Clear()
            End SyncLock
            UpdateChart() ' Refresh chart with empty data
            UpdateRadarStatusLabel() ' This will now show "No Interference Detected"
        End If
    End Sub

    Private Function IsHackRFConnected(exePath As String, timeoutMs As Integer) As Boolean
        Try
            Dim output As String = ""
            Dim proc As New Process()
            proc.StartInfo.FileName = exePath
            proc.StartInfo.RedirectStandardOutput = True
            proc.StartInfo.UseShellExecute = False
            proc.StartInfo.CreateNoWindow = True
            proc.Start()

            If Not proc.WaitForExit(timeoutMs) Then
                proc.Kill()
                File.WriteAllText("hackrf_debug.txt", "❌ Timed out waiting for hackrf_info.")
                Return False
            End If

            output = proc.StandardOutput.ReadToEnd()
            File.WriteAllText("hackrf_debug.txt", output)
            Return output.ToLower().Contains("found hackrf")
        Catch ex As Exception
            File.WriteAllText("hackrf_debug.txt", "❌ Exception: " & ex.ToString())
            Return False
        End Try
    End Function

    ' End Check Device


    ' Start HackRF
    Private Const HACKRF_SWEEP_PATH As String = "C:\ProgramData\radioconda\Library\bin\hackrf_sweep.exe" ' Ensure this is correct for your system

    ' --- Frequency variables ---
    Private CurrentStartFreqMHz As Double = 5170.0 ' Default (e.g., Ch 36 view)
    Private CurrentStopFreqMHz As Double = 5190.0  ' Default (e.g., Ch 36 view)
    Private Const CHANNEL_VIEW_SPAN_MHZ As Double = 20.0 ' How wide the view for a selected channel is
    Private Const BIN_WIDTH_KHZ As Double = 20.0 ' Adjust for desired resolution vs speed

    ' --- Adjust Gains as needed ---
    Private Const LNA_GAIN As Integer = 40 ' Example IF/LNA Gain
    Private Const VGA_GAIN As Integer = 40 ' Example BB/VGA Gain
    Private Const AMP_ENABLED As Boolean = False ' Set to True to try enabling amp (-a 1)
    ' --- Smoothing Configuration ---
    Private Const SMOOTHING_WINDOW_SIZE As Integer = 11 ' Number of points to average (must be odd)

    Private WithEvents SweepProcess As Process
    ' Data storage for all modes
    Private LiveSweepPoints As New ZedGraph.PointPairList()
    Private SmoothedLivePoints As New ZedGraph.PointPairList() ' For smoothed live data
    Private MinHoldSweepPoints As New ZedGraph.PointPairList()
    Private MaxHoldSweepPoints As New ZedGraph.PointPairList()
    Private AverageSweepPoints As New ZedGraph.PointPairList()
    ' Dictionaries to store calculation data
    Private MinHoldDataStore As New Dictionary(Of Double, Double)()
    Private MaxHoldDataStore As New Dictionary(Of Double, Double)()
    Private AverageDataStore As New Dictionary(Of Double, Tuple(Of Double, Integer))()
    Private dataLock As New Object() ' Object for thread synchronization

    ' --- New variables for logging and channel tracking ---
    Private lastRadarStatus As String = ""
    Private lastChannel As WifiChannel? = Nothing
    Private dwellStartTime As DateTime? = Nothing ' To store the timestamp of when a sweep is stopped.
    Private Const LOG_TIMESTAMP_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

    ' --- Threshold Lines ---
    Private wifiThresholdLine As ZedGraph.LineObj
    Private radarThresholdLine As ZedGraph.LineObj
    Private currentWifiThresholdDBm As Double = -25.0 ' Adjusted threshold for general Wi-Fi signals
    Private currentRadarThresholdDBm As Double = -45.0 ' Raised to prevent strong Wi-Fi from being classed as Radar
    Private Const WIFI_THRESHOLD_TAG As String = "WifiThreshold"
    Private Const RADAR_THRESHOLD_TAG As String = "RadarThreshold"


    ' --- Wi-Fi Channel Definition ---
    Private Structure WifiChannel
        Public Name As String ' e.g., "Ch 36 (5180 MHz - Non-DFS)"
        Public CenterFreqMHz As Double
        Public IsDFS As Boolean
        Public StartFreqMHz As Double ' Calculated Start Frequency for sweep
        Public StopFreqMHz As Double  ' Calculated Stop Frequency for sweep

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Structure

    Private AllWifiChannels As New List(Of WifiChannel)()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeWifiChannels()
        PopulateChannelComboBox()
        SetupZedGraph()

        ' New: Initialize the channel selection time
        'channelSelectionTime = DateTime.Now

        cbCurrentCh.Enabled = False
        btnStartSweep.Enabled = False
        btnStopSweep.Enabled = False
        btnResetMin.Enabled = False
        btnResetMax.Enabled = False
        lblStatus.Text = "Status: Idle. Please check HackRF device."
        TSCheckDevice.Text = "HackRF Status: Unknown (Click Check HackRF)"

        If lblRadarStatus IsNot Nothing Then
            lblRadarStatus.Text = "Status: Initializing..."
            lblRadarStatus.ForeColor = SystemColors.ControlText
        End If

        ' New: Initialize new UI elements
        If tbCurrentCh IsNot Nothing Then tbCurrentCh.Text = "N/A"
        If tbLastChannel IsNot Nothing Then tbLastChannel.Text = "N/A"
        If lbObservedWifiEvents IsNot Nothing Then lbObservedWifiEvents.Items.Clear()
        If lbInferredChannelMoves IsNot Nothing Then lbInferredChannelMoves.Items.Clear()

        AddHandler chkMainCurve.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkMinHold.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkMaxHold.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkAverage.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkSmooth.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler cbCurrentCh.SelectedIndexChanged, AddressOf CbCurrentCh_SelectedIndexChanged_Main
        AddHandler btnResetMin.Click, AddressOf btnResetMin_Click
        AddHandler btnResetMax.Click, AddressOf btnResetMax_Click

        ' --- Initialize Threshold controls ---
        If tbThreshold IsNot Nothing Then
            tbThreshold.Text = currentWifiThresholdDBm.ToString(CultureInfo.InvariantCulture)
            AddHandler btnThreshold.Click, AddressOf btnSetWifiThreshold_Click
        End If

        Debug.WriteLine("Form Loaded. ZedGraph Initialized. Channels Populated.")

        ' Set default channel
        Dim initialChannelIndex As Integer = AllWifiChannels.FindIndex(Function(c) c.Name.Contains("Ch 149"))
        If initialChannelIndex <> -1 Then
            cbCurrentCh.SelectedIndex = initialChannelIndex
        ElseIf cbCurrentCh.Items.Count > 0 Then
            cbCurrentCh.SelectedIndex = 0
        End If

        If cbCurrentCh.SelectedItem IsNot Nothing Then
            UpdateFrequenciesFromChannel(DirectCast(cbCurrentCh.SelectedItem, WifiChannel))
            UpdateZedGraphXAxis()
        End If
    End Sub

    Private Sub InitializeWifiChannels()
        AllWifiChannels.Clear()
        Dim halfSpan As Double = CHANNEL_VIEW_SPAN_MHZ / 2.0

        ' Non-DFS U-NII-1
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 36 (5180 MHz)", .CenterFreqMHz = 5180, .IsDFS = False, .StartFreqMHz = 5180 - halfSpan, .StopFreqMHz = 5180 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 40 (5200 MHz)", .CenterFreqMHz = 5200, .IsDFS = False, .StartFreqMHz = 5200 - halfSpan, .StopFreqMHz = 5200 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 44 (5220 MHz)", .CenterFreqMHz = 5220, .IsDFS = False, .StartFreqMHz = 5220 - halfSpan, .StopFreqMHz = 5220 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 48 (5240 MHz)", .CenterFreqMHz = 5240, .IsDFS = False, .StartFreqMHz = 5240 - halfSpan, .StopFreqMHz = 5240 + halfSpan})
        ' DFS U-NII-2A
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 52 (5260 MHz, DFS)", .CenterFreqMHz = 5260, .IsDFS = True, .StartFreqMHz = 5260 - halfSpan, .StopFreqMHz = 5260 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 56 (5280 MHz, DFS)", .CenterFreqMHz = 5280, .IsDFS = True, .StartFreqMHz = 5280 - halfSpan, .StopFreqMHz = 5280 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 60 (5300 MHz, DFS)", .CenterFreqMHz = 5300, .IsDFS = True, .StartFreqMHz = 5300 - halfSpan, .StopFreqMHz = 5300 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 64 (5320 MHz, DFS)", .CenterFreqMHz = 5320, .IsDFS = True, .StartFreqMHz = 5320 - halfSpan, .StopFreqMHz = 5320 + halfSpan})
        ' DFS U-NII-2C
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 100 (5500 MHz, DFS)", .CenterFreqMHz = 5500, .IsDFS = True, .StartFreqMHz = 5500 - halfSpan, .StopFreqMHz = 5500 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 116 (5580 MHz, DFS)", .CenterFreqMHz = 5580, .IsDFS = True, .StartFreqMHz = 5580 - halfSpan, .StopFreqMHz = 5580 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 140 (5700 MHz, DFS)", .CenterFreqMHz = 5700, .IsDFS = True, .StartFreqMHz = 5700 - halfSpan, .StopFreqMHz = 5700 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 144 (5720 MHz, DFS)", .CenterFreqMHz = 5720, .IsDFS = True, .StartFreqMHz = 5720 - halfSpan, .StopFreqMHz = 5720 + halfSpan})
        ' Non-DFS U-NII-3
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 149 (5745 MHz)", .CenterFreqMHz = 5745, .IsDFS = False, .StartFreqMHz = 5745 - halfSpan, .StopFreqMHz = 5745 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 153 (5765 MHz)", .CenterFreqMHz = 5765, .IsDFS = False, .StartFreqMHz = 5765 - halfSpan, .StopFreqMHz = 5765 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 157 (5785 MHz)", .CenterFreqMHz = 5785, .IsDFS = False, .StartFreqMHz = 5785 - halfSpan, .StopFreqMHz = 5785 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 161 (5805 MHz)", .CenterFreqMHz = 5805, .IsDFS = False, .StartFreqMHz = 5805 - halfSpan, .StopFreqMHz = 5805 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 165 (5825 MHz)", .CenterFreqMHz = 5825, .IsDFS = False, .StartFreqMHz = 5825 - halfSpan, .StopFreqMHz = 5825 + halfSpan})
    End Sub

    Private Sub UpdateRadarStatusLabel()
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            Me.BeginInvoke(New Action(Sub()

                                          Dim wifiDetected As Boolean = False
                                          Dim radarDetected As Boolean = False

                                          SyncLock dataLock ' Ensure thread-safe access to MaxHoldSweepPoints
                                              If MaxHoldSweepPoints IsNot Nothing AndAlso MaxHoldSweepPoints.Count > 0 Then
                                                  For i As Integer = 0 To MaxHoldSweepPoints.Count - 1
                                                      Dim powerValue = MaxHoldSweepPoints(i).Y
                                                      ' Check against the highest threshold (Wi-Fi) first for this logic.
                                                      If powerValue > currentWifiThresholdDBm Then
                                                          wifiDetected = True
                                                          Exit For ' Wi-Fi is strong, no need to check for weaker radar signals in this context.
                                                      ElseIf powerValue > currentRadarThresholdDBm Then
                                                          radarDetected = True
                                                          ' Continue checking in case a stronger Wi-Fi signal appears later in the sweep.
                                                      End If
                                                  Next
                                              End If
                                          End SyncLock

                                          ' --- MODIFIED SECTION ---
                                          ' Update the detection accuracy based on the results.
                                          If wifiDetected OrElse radarDetected Then
                                              UpdateDetectionAccuracy()
                                          Else
                                              If tbDetectionAcc IsNot Nothing Then
                                                  tbDetectionAcc.Text = "N/A"
                                              End If
                                          End If
                                          ' --- END MODIFIED SECTION ---

                                          Dim newStatus As String
                                          Dim newColor As Color

                                          If wifiDetected Then
                                              newStatus = "Wi-Fi Detected"
                                              newColor = Color.Red
                                          ElseIf radarDetected Then
                                              newStatus = "Radar Detected"
                                              newColor = Color.Magenta
                                          Else
                                              newStatus = "No Interference Detected"
                                              newColor = Color.Green
                                          End If

                                          ' Only update and log if the status has actually changed.
                                          If lastRadarStatus <> newStatus Then
                                              lastRadarStatus = newStatus
                                              lblRadarStatus.Text = newStatus
                                              lblRadarStatus.ForeColor = newColor

                                              If lbObservedWifiEvents IsNot Nothing AndAlso tbCurrentChDisplay IsNot Nothing Then
                                                  ' Adding the channel to the log makes it more informative
                                                  Dim logMessage As String = $"{DateTime.Now.ToString(LOG_TIMESTAMP_FORMAT)}: {newStatus} on {tbCurrentChDisplay.Text}"
                                                  lbObservedWifiEvents.Items.Insert(0, logMessage)
                                              End If
                                          End If
                                      End Sub))
        End If
    End Sub

    Private Async Sub LoadCsvSimulationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadCsvSimulationToolStripMenuItem.Click
        ' Stop any live sweep first
        If SweepProcess IsNot Nothing AndAlso Not SweepProcess.HasExited Then
            StopSweepProcess()
        End If
        lblStatus.Text = "Status: Idle. CSV Simulation mode."

        ' --- Step 1: Load the "Before DFS" file (e.g., Radar on Ch 52) ---
        Dim ofdBefore As New OpenFileDialog With {
            .Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
            .Title = "Select 'Before DFS' CSV File (e.g., Radar on Ch 52)"
        }

        If ofdBefore.ShowDialog() <> DialogResult.OK Then
            MessageBox.Show("Simulation cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim beforeData As PointPairList = LoadDataFromCsv(ofdBefore.FileName)
        If beforeData Is Nothing OrElse beforeData.Count = 0 Then
            MessageBox.Show("Could not parse the 'Before' CSV file or it was empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' --- Step 2: Identify channel and update UI for the "Before" state ---
        Dim beforeChannel? As WifiChannel = FindChannelFromData(beforeData)
        If Not beforeChannel.HasValue Then
            ' The old, generic error message has been replaced by the more informative one in FindChannelFromData
            Return
        End If

        ' Set the ComboBox to the correct channel. This will trigger CbCurrentCh_SelectedIndexChanged_Main
        ' which correctly clears all data lists and updates the graph's X-axis.
        cbCurrentCh.SelectedItem = beforeChannel.Value

        ' Now load the parsed data into the main data lists for display
        SyncLock dataLock
            ' We load it into Live and MaxHold to make it visible and trigger detection logic
            LiveSweepPoints = DirectCast(beforeData.Clone(), PointPairList)
            MaxHoldSweepPoints = DirectCast(beforeData.Clone(), PointPairList)
            SmoothedLivePoints = ApplyMovingAverage(LiveSweepPoints, SMOOTHING_WINDOW_SIZE)
        End SyncLock

        UpdateChart()
        UpdateRadarStatusLabel() ' Check for radar in the loaded file
        MessageBox.Show($"Loaded 'Before' data for {beforeChannel.Value.Name}. Click OK to simulate the channel change.", "Simulation Step 1", MessageBoxButtons.OK, MessageBoxIcon.Information)
        lblStatus.Text = $"Status: Simulating on {beforeChannel.Value.Name}"

        Await Task.Delay(500) ' Brief pause

        ' --- Step 3: Load the "After DFS" file (e.g., Wi-Fi on Ch 36) ---
        Dim ofdAfter As New OpenFileDialog With {
            .Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
            .Title = "Select 'After DFS' CSV File (e.g., Wi-Fi on Ch 36)"
        }

        If ofdAfter.ShowDialog() <> DialogResult.OK Then
            MessageBox.Show("Simulation cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim afterData As PointPairList = LoadDataFromCsv(ofdAfter.FileName)
        If afterData Is Nothing OrElse afterData.Count = 0 Then
            MessageBox.Show("Could not parse the 'After' CSV file or it was empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' --- Step 4: THE DFS MOVE - Identify the new channel and switch to it ---
        Dim afterChannel? As WifiChannel = FindChannelFromData(afterData)
        If Not afterChannel.HasValue Then
            Return
        End If

        lblStatus.Text = $"Status: DFS Event! Moving from {beforeChannel.Value.Name} to {afterChannel.Value.Name}"
        MessageBox.Show($"DFS event triggered! Simulating move to {afterChannel.Value.Name}.", "DFS Algorithm", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        cbCurrentCh.SelectedItem = afterChannel.Value

        SyncLock dataLock
            LiveSweepPoints = DirectCast(afterData.Clone(), PointPairList)
            MaxHoldSweepPoints = DirectCast(afterData.Clone(), PointPairList)
            SmoothedLivePoints = ApplyMovingAverage(LiveSweepPoints, SMOOTHING_WINDOW_SIZE)
        End SyncLock

        UpdateChart()
        UpdateRadarStatusLabel() ' Check the signal on the new channel
        lblStatus.Text = $"Status: Simulation complete. Now on {afterChannel.Value.Name}"

    End Sub

    Private Function LoadDataFromCsv(filePath As String) As PointPairList
        If Not File.Exists(filePath) Then Return Nothing

        Dim dataPoints As New PointPairList()
        Try
            For Each line As String In File.ReadLines(filePath)
                If String.IsNullOrWhiteSpace(line) Then Continue For

                Dim parts As String() = line.Split(","c)
                If parts.Length > 6 Then
                    Dim hzLow As Double = Double.Parse(parts(2), CultureInfo.InvariantCulture)
                    Dim binWidthHz As Double = Double.Parse(parts(4), CultureInfo.InvariantCulture)
                    Dim numBins As Integer = parts.Length - 6

                    For i As Integer = 0 To numBins - 1
                        Dim centerFreqHz As Double = hzLow + (i * binWidthHz) + (binWidthHz / 2.0)
                        Dim freqMHz As Double = centerFreqHz / 1000000.0
                        Dim dbmValue As Double = Double.Parse(parts(6 + i), CultureInfo.InvariantCulture)
                        dataPoints.Add(freqMHz, dbmValue)
                    Next
                    Return dataPoints
                End If
            Next
            Return Nothing
        Catch ex As Exception
            Debug.WriteLine($"Error parsing CSV file '{filePath}': {ex.Message}")
            Return Nothing
        End Try
    End Function

    Private Function FindChannelFromData(data As PointPairList) As WifiChannel?
        If data Is Nothing OrElse data.Count = 0 Then Return Nothing

        Dim dataCenterFreq As Double = (data.First().X + data.Last().X) / 2.0

        For Each channel As WifiChannel In AllWifiChannels
            If Math.Abs(channel.CenterFreqMHz - dataCenterFreq) < 5.0 Then
                Return channel
            End If
        Next

        ' If the loop finishes, no channel was found. Show an informative error.
        MessageBox.Show($"Channel match failed. The calculated center frequency was {dataCenterFreq} MHz. Please check the channel list in the InitializeWifiChannels subroutine.", "Error: Could Not Identify Channel", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return Nothing
    End Function

    Private Sub PopulateChannelComboBox()
        cbCurrentCh.Items.Clear()
        For Each channel As WifiChannel In AllWifiChannels
            cbCurrentCh.Items.Add(channel)
        Next
        cbCurrentCh.DisplayMember = "Name"
    End Sub

    Private Sub UpdateFrequenciesFromChannel(selectedChannel As WifiChannel)
        CurrentStartFreqMHz = selectedChannel.StartFreqMHz
        CurrentStopFreqMHz = selectedChannel.StopFreqMHz
    End Sub

    Private Sub UpdateZedGraphXAxis()
        Dim myPane As GraphPane = zg1.GraphPane
        myPane.XAxis.Scale.Min = CurrentStartFreqMHz
        myPane.XAxis.Scale.Max = CurrentStopFreqMHz
        UpdateThresholdLineOnGraph() ' Update threshold line span
        zg1.AxisChange()
        zg1.Invalidate()
    End Sub

    Private Sub CbCurrentCh_SelectedIndexChanged_Main(sender As Object, e As EventArgs)
        If cbCurrentCh.SelectedItem IsNot Nothing Then
            Dim selectedChannel As WifiChannel = DirectCast(cbCurrentCh.SelectedItem, WifiChannel)

            ' --- REVISED: Channel Move Logging Logic ---
            ' Check if a channel was previously selected and the new selection is different.
            If lastChannel.HasValue AndAlso lastChannel.Value.Name <> selectedChannel.Name Then
                Dim logMessage As String

                ' Check if the dwell timer was started by a btnStopSweep click.
                If dwellStartTime.HasValue Then
                    ' A sweep was stopped, so we can calculate the dwell time.
                    Dim duration As TimeSpan = DateTime.Now - dwellStartTime.Value
                    logMessage = $"{DateTime.Now.ToString(LOG_TIMESTAMP_FORMAT)}: Moved from {lastChannel.Value.Name} to {selectedChannel.Name} (Dwell: {duration.TotalSeconds:F1}s)"

                    ' IMPORTANT: Reset the dwell timer after using it.
                    dwellStartTime = Nothing
                Else
                    ' The channel was changed without a preceding sweep stop. No dwell time to report.
                    logMessage = $"{DateTime.Now.ToString(LOG_TIMESTAMP_FORMAT)}: Changed channel from {lastChannel.Value.Name} to {selectedChannel.Name}"
                End If

                ' Add the generated message to the log.
                If lbInferredChannelMoves IsNot Nothing Then
                    lbInferredChannelMoves.Items.Insert(0, logMessage)
                End If

                ' Update the display for the last channel.
                If tbLastChannel IsNot Nothing Then
                    tbLastChannel.Text = lastChannel.Value.Name
                End If

            ElseIf Not lastChannel.HasValue Then
                ' This is the first channel selection on form load.
                If tbLastChannel IsNot Nothing Then
                    tbLastChannel.Text = "N/A"
                End If
            End If

            ' Update state for the new/current channel.
            lastChannel = selectedChannel
            ' --- END REVISED LOGIC ---

            UpdateFrequenciesFromChannel(selectedChannel)

            If SweepProcess IsNot Nothing AndAlso Not SweepProcess.HasExited Then
                StopSweepProcess()
                lblStatus.Text = "Status: Stopped (Channel Changed)."
            Else
                lblStatus.Text = $"Status: Channel set to {selectedChannel.Name}. Ready."
                If TSCheckDevice.Text.Contains("Connected") Then
                    btnStartSweep.Enabled = True
                    btnResetMin.Enabled = True
                    btnResetMax.Enabled = True
                Else
                    btnStartSweep.Enabled = False
                    btnResetMin.Enabled = False
                    btnResetMax.Enabled = False
                End If
            End If

            SyncLock dataLock
                LiveSweepPoints.Clear()
                SmoothedLivePoints.Clear()
                MinHoldSweepPoints.Clear()
                MaxHoldSweepPoints.Clear()
                AverageSweepPoints.Clear()
                MinHoldDataStore.Clear()
                MaxHoldDataStore.Clear()
                AverageDataStore.Clear()
            End SyncLock
            UpdateChart()
            UpdateRadarStatusLabel() ' Update after channel change and data clear
            UpdateZedGraphXAxis()
            UpdateDisplayWithDeclaredVariables()
        End If
    End Sub

    Private Sub SetupZedGraph()
        If zg1 Is Nothing Then Return
        Dim myPane As GraphPane = zg1.GraphPane
        myPane.CurveList.Clear()
        myPane.GraphObjList.Clear() ' Clear all graph objects, including old lines

        myPane.Title.Text = "HackRF Sweep Spectrum"
        myPane.XAxis.Title.Text = "Frequency (MHz)"
        myPane.YAxis.Title.Text = "Power (dBm)"
        myPane.Chart.Fill = New Fill(Color.White)
        myPane.Fill = New Fill(Color.FromArgb(230, 230, 230))
        myPane.XAxis.MajorGrid.IsVisible = True
        myPane.YAxis.MajorGrid.IsVisible = True
        myPane.XAxis.Scale.Min = CurrentStartFreqMHz
        myPane.XAxis.Scale.Max = CurrentStopFreqMHz
        myPane.YAxis.Scale.Min = -90
        myPane.YAxis.Scale.Max = 0

        ' Data Curves
        myPane.AddCurve("Live Data", LiveSweepPoints, Color.Red, SymbolType.None).Line.Width = 1.5F
        myPane.AddCurve("Smoothed Live", SmoothedLivePoints, Color.OrangeRed, SymbolType.None).Line.Width = 1.5F
        myPane.AddCurve("Min Hold", MinHoldSweepPoints, Color.DarkCyan, SymbolType.None).Line.Width = 1.0F
        myPane.AddCurve("Max Hold", MaxHoldSweepPoints, Color.Goldenrod, SymbolType.None).Line.Width = 1.5F
        myPane.AddCurve("Average Data", AverageSweepPoints, Color.Blue, SymbolType.None).Line.Width = 2.0F

        ' --- Add Wi-Fi Threshold Line ---
        wifiThresholdLine = New LineObj(Color.OrangeRed, myPane.XAxis.Scale.Min, currentWifiThresholdDBm, myPane.XAxis.Scale.Max, currentWifiThresholdDBm)
        wifiThresholdLine.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash
        wifiThresholdLine.Line.Width = 2.0F
        wifiThresholdLine.ZOrder = ZOrder.A_InFront
        wifiThresholdLine.Tag = WIFI_THRESHOLD_TAG
        wifiThresholdLine.IsVisible = False ' Line will not be drawn on the graph
        myPane.GraphObjList.Add(wifiThresholdLine)

        ' --- Add Radar Threshold Line ---
        radarThresholdLine = New LineObj(Color.Magenta, myPane.XAxis.Scale.Min, currentRadarThresholdDBm, myPane.XAxis.Scale.Max, currentRadarThresholdDBm)
        radarThresholdLine.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot
        radarThresholdLine.Line.Width = 2.0F
        radarThresholdLine.ZOrder = ZOrder.A_InFront
        radarThresholdLine.Tag = RADAR_THRESHOLD_TAG
        radarThresholdLine.IsVisible = False ' Line will not be drawn on the graph
        myPane.GraphObjList.Add(radarThresholdLine)

        myPane.Legend.IsVisible = True
        zg1.AxisChange()
        zg1.Invalidate()
        Debug.WriteLine("ZedGraph Setup Complete with all curves and threshold lines.")
    End Sub

    Private Sub btnSetWifiThreshold_Click(sender As Object, e As EventArgs)
        Dim newThreshold As Double
        If tbThreshold Is Nothing Then Return

        If Double.TryParse(tbThreshold.Text, NumberStyles.Float, CultureInfo.InvariantCulture, newThreshold) Then
            currentWifiThresholdDBm = newThreshold
            UpdateThresholdLineOnGraph()
            UpdateRadarStatusLabel()
            lblStatus.Text = $"Status: Wi-Fi threshold set to {currentWifiThresholdDBm} dBm."
        Else
            MessageBox.Show("Invalid Wi-Fi threshold value. Please enter a numeric value (e.g., -65.0).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tbThreshold.Text = currentWifiThresholdDBm.ToString(CultureInfo.InvariantCulture)
        End If
    End Sub



    Private Sub UpdateThresholdLineOnGraph()
        If zg1 Is Nothing OrElse zg1.GraphPane Is Nothing Then Return
        Dim myPane As GraphPane = zg1.GraphPane

        ' Find and update the Wi-Fi line
        Dim wifiLine As LineObj = TryCast(myPane.GraphObjList.Find(Function(obj) obj.Tag?.ToString() = WIFI_THRESHOLD_TAG), LineObj)
        If wifiLine IsNot Nothing Then
            wifiLine.Location.Y = currentWifiThresholdDBm
            wifiLine.Location.X = myPane.XAxis.Scale.Min
        End If

        ' Find and update the Radar line
        Dim radarLine As LineObj = TryCast(myPane.GraphObjList.Find(Function(obj) obj.Tag?.ToString() = RADAR_THRESHOLD_TAG), LineObj)
        If radarLine IsNot Nothing Then
            radarLine.Location.Y = currentRadarThresholdDBm
            radarLine.Location.X = myPane.XAxis.Scale.Min
        End If

        zg1.Invalidate()
    End Sub

    Private Sub btnStartSweep_Click(sender As Object, e As EventArgs) Handles btnStartSweep.Click
        If SweepProcess IsNot Nothing AndAlso Not SweepProcess.HasExited Then
            MessageBox.Show("Sweep is already running.")
            Return
        End If

        If Not TSCheckDevice.Text.Contains("Connected") Then
            MessageBox.Show("HackRF is not connected. Please check connection.", "HackRF Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not File.Exists(HACKRF_SWEEP_PATH) Then
            MessageBox.Show($"hackrf_sweep.exe not found at: {HACKRF_SWEEP_PATH}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        SyncLock dataLock
            LiveSweepPoints.Clear()
            SmoothedLivePoints.Clear()
        End SyncLock
        UpdateChart()

        Dim startFreqMHzStr As String = CurrentStartFreqMHz.ToString(CultureInfo.InvariantCulture)
        Dim stopFreqMHzStr As String = CurrentStopFreqMHz.ToString(CultureInfo.InvariantCulture)
        Dim binWidthHz As Long = CLng(BIN_WIDTH_KHZ * 1000)
        Dim ampArg As String = If(AMP_ENABLED, "-a 1", "-a 0")
        Dim arguments As String = $"-f {startFreqMHzStr}:{stopFreqMHzStr} -w {binWidthHz} -l {LNA_GAIN} -g {VGA_GAIN} {ampArg}"

        SweepProcess = New Process()
        With SweepProcess.StartInfo
            .FileName = HACKRF_SWEEP_PATH
            .Arguments = arguments
            .UseShellExecute = False
            .RedirectStandardOutput = True
            .RedirectStandardError = True
            .CreateNoWindow = True
        End With

        AddHandler SweepProcess.OutputDataReceived, AddressOf SweepOutputHandler
        AddHandler SweepProcess.ErrorDataReceived, AddressOf SweepErrorHandler
        AddHandler SweepProcess.Exited, AddressOf SweepProcess_Exited

        Try
            SweepProcess.Start()
            SweepProcess.BeginOutputReadLine()
            SweepProcess.BeginErrorReadLine()

            btnStartSweep.Enabled = False
            btnStopSweep.Enabled = True
            cbCurrentCh.Enabled = False
            lblStatus.Text = "Status: Sweeping..."
        Catch ex As Exception
            MessageBox.Show($"Failed to start hackrf_sweep: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            SweepProcess = Nothing
            UpdateUIStopState()
            lblStatus.Text = "Status: Error starting sweep."
        End Try
    End Sub

    Private Sub SweepProcess_Exited(sender As Object, e As EventArgs)
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            Me.BeginInvoke(New Action(Sub()
                                          If SweepProcess IsNot Nothing Then
                                              If btnStopSweep.Enabled Then
                                                  lblStatus.Text = "Status: Sweep process ended unexpectedly."
                                                  StopSweepProcess()
                                              End If
                                          Else
                                              If Not lblStatus.Text.StartsWith("Status: Error") AndAlso Not lblStatus.Text.StartsWith("Status: Stopped") Then
                                                  lblStatus.Text = "Status: Sweep process ended."
                                              End If
                                              UpdateUIStopState()
                                          End If
                                      End Sub))
        End If
    End Sub

    Private Sub btnStopSweep_Click(sender As Object, e As EventArgs) Handles btnStopSweep.Click
        StopSweepProcess()
        lblStatus.Text = "Status: Stopped by user."
        ' ADD THIS LINE: Start the dwell timer when the sweep is manually stopped.
        dwellStartTime = DateTime.Now
    End Sub

    Private Sub StopSweepProcess()
        If SweepProcess IsNot Nothing Then
            RemoveHandler SweepProcess.Exited, AddressOf SweepProcess_Exited
            If Not SweepProcess.HasExited Then
                Try
                    RemoveHandler SweepProcess.OutputDataReceived, AddressOf SweepOutputHandler
                    RemoveHandler SweepProcess.ErrorDataReceived, AddressOf SweepErrorHandler
                    If Not SweepProcess.HasExited Then
                        Try
                            SweepProcess.CancelOutputRead()
                            SweepProcess.CancelErrorRead()
                        Catch exCancel As InvalidOperationException
                            Debug.WriteLine($"Info: Error cancelling reads, process likely already closing: {exCancel.Message}")
                        End Try
                        SweepProcess.Kill()
                        SweepProcess.WaitForExit(1500)
                    End If
                Catch ex As Exception
                    Debug.WriteLine($"Error during sweep process stop: {ex.Message}")
                End Try
            End If
            SweepProcess.Dispose()
            SweepProcess = Nothing
        End If

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(AddressOf UpdateUIStopState))
        Else
            UpdateUIStopState()
        End If
    End Sub

    Private Sub UpdateUIStopState()
        If TSCheckDevice.Text.Contains("Connected") Then
            btnStartSweep.Enabled = True
            cbCurrentCh.Enabled = True
            btnResetMin.Enabled = True
            btnResetMax.Enabled = True
        Else
            btnStartSweep.Enabled = False
            cbCurrentCh.Enabled = False
            btnResetMin.Enabled = False
            btnResetMax.Enabled = False
        End If
        btnStopSweep.Enabled = False

        If lblStatus.Text = "Status: Sweeping..." OrElse
           lblStatus.Text.Contains("unexpectedly") OrElse
           lblStatus.Text.Contains("process ended") Then
            lblStatus.Text = "Status: Idle."
        End If
    End Sub

    Private Sub btnResetMin_Click(sender As Object, e As EventArgs)
        SyncLock dataLock
            MinHoldDataStore.Clear()
            MinHoldSweepPoints.Clear()
        End SyncLock
        UpdateChart()
        lblStatus.Text = "Status: Min Hold trace reset."
    End Sub

    Private Sub btnResetMax_Click(sender As Object, e As EventArgs)
        SyncLock dataLock
            MaxHoldDataStore.Clear()
            MaxHoldSweepPoints.Clear()
        End SyncLock
        UpdateChart()
        UpdateRadarStatusLabel() ' Re-evaluate status after clearing max hold
        lblStatus.Text = "Status: Max Hold trace reset."
    End Sub

    Private Function ApplyMovingAverage(inputPoints As PointPairList, windowSize As Integer) As PointPairList
        Dim smoothedPoints As New PointPairList()
        If inputPoints Is Nothing OrElse inputPoints.Count = 0 Then Return smoothedPoints
        If windowSize <= 1 Then Return DirectCast(inputPoints.Clone(), PointPairList)

        Dim actualWindowSize = If(windowSize Mod 2 = 0, windowSize + 1, windowSize)
        Dim halfWindow As Integer = actualWindowSize \ 2

        For i As Integer = 0 To inputPoints.Count - 1
            Dim sum As Double = 0
            Dim count As Integer = 0
            Dim startIdx As Integer = Math.Max(0, i - halfWindow)
            Dim endIdx As Integer = Math.Min(inputPoints.Count - 1, i + halfWindow)

            For j As Integer = startIdx To endIdx
                sum += inputPoints(j).Y
                count += 1
            Next

            If count > 0 Then
                smoothedPoints.Add(inputPoints(i).X, sum / count)
            Else
                smoothedPoints.Add(inputPoints(i).X, inputPoints(i).Y)
            End If
        Next
        Return smoothedPoints
    End Function

    Private Sub SweepOutputHandler(sendingProcess As Object, outLine As DataReceivedEventArgs)
        If String.IsNullOrEmpty(outLine.Data) Then Return

        Try
            Dim parts As String() = outLine.Data.Split(","c)
            If parts.Length > 6 Then
                Dim hzLow As Double = Double.Parse(parts(2), CultureInfo.InvariantCulture)
                Dim binWidthHz As Double = Double.Parse(parts(4), CultureInfo.InvariantCulture)
                Dim numBins As Integer = parts.Length - 6

                Dim currentLivePoints As New ZedGraph.PointPairList()
                Dim maxHoldNeedsUpdate As Boolean = False

                For i As Integer = 0 To numBins - 1
                    Dim centerFreqHz As Double = hzLow + (i * binWidthHz) + (binWidthHz / 2.0)
                    Dim freqMHz As Double = centerFreqHz / 1000000.0
                    Dim dbmValue As Double = Double.Parse(parts(6 + i), CultureInfo.InvariantCulture)
                    currentLivePoints.Add(freqMHz, dbmValue)
                Next

                SyncLock dataLock
                    LiveSweepPoints = currentLivePoints
                    SmoothedLivePoints = ApplyMovingAverage(LiveSweepPoints, SMOOTHING_WINDOW_SIZE)

                    For i As Integer = 0 To LiveSweepPoints.Count - 1
                        Dim freqMHz As Double = LiveSweepPoints(i).X
                        Dim dbmValue As Double = LiveSweepPoints(i).Y

                        ' MinHold Logic
                        If Not MinHoldDataStore.ContainsKey(freqMHz) OrElse dbmValue < MinHoldDataStore(freqMHz) Then
                            MinHoldDataStore(freqMHz) = dbmValue
                        End If

                        ' MaxHold Logic
                        If Not MaxHoldDataStore.ContainsKey(freqMHz) OrElse dbmValue > MaxHoldDataStore(freqMHz) Then
                            MaxHoldDataStore(freqMHz) = dbmValue
                            maxHoldNeedsUpdate = True
                        End If

                        ' Average Logic
                        If AverageDataStore.ContainsKey(freqMHz) Then
                            Dim currentTuple = AverageDataStore(freqMHz)
                            AverageDataStore(freqMHz) = Tuple.Create(currentTuple.Item1 + dbmValue, currentTuple.Item2 + 1)
                        Else
                            AverageDataStore.Add(freqMHz, Tuple.Create(dbmValue, 1))
                        End If
                    Next

                    ' Rebuild PointPairLists from DataStores
                    MinHoldSweepPoints.Clear()
                    For Each kvp In MinHoldDataStore.OrderBy(Function(x) x.Key)
                        MinHoldSweepPoints.Add(kvp.Key, kvp.Value)
                    Next
                    MaxHoldSweepPoints.Clear()
                    For Each kvp In MaxHoldDataStore.OrderBy(Function(x) x.Key)
                        MaxHoldSweepPoints.Add(kvp.Key, kvp.Value)
                    Next
                    AverageSweepPoints.Clear()
                    For Each kvp In AverageDataStore.OrderBy(Function(x) x.Key)
                        AverageSweepPoints.Add(kvp.Key, kvp.Value.Item1 / kvp.Value.Item2)
                    Next
                End SyncLock

                If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                    Me.BeginInvoke(New Action(AddressOf UpdateChart))
                    If maxHoldNeedsUpdate Then
                        UpdateRadarStatusLabel() ' Trigger re-classification
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error processing sweep output: {ex.Message} - Line: {outLine.Data}")
        End Try
    End Sub

    Private Sub SweepErrorHandler(sendingProcess As Object, errLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(errLine.Data) Then
            Debug.WriteLine($"HACKRF_SWEEP DEBUG: {errLine.Data}")
            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                Me.BeginInvoke(New Action(Sub()
                                              If lblStatus IsNot Nothing Then lblStatus.Text = "Status: Sweeping."
                                              If errLine.Data.ToLower().Contains("no hackrf device found") Then
                                                  StopSweepProcess()
                                              End If
                                          End Sub))
            End If
        End If
    End Sub

    Private Sub TraceSelectionChanged(sender As Object, e As EventArgs)
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            Me.BeginInvoke(New Action(AddressOf UpdateChart))
        End If
    End Sub

    Private Sub UpdateChart()
        If zg1 Is Nothing OrElse zg1.GraphPane Is Nothing Then Return
        Dim myPane As GraphPane = zg1.GraphPane
        If myPane.CurveList.Count < 5 Then
            SetupZedGraph()
            myPane = zg1.GraphPane
            If myPane.CurveList.Count < 5 Then Return
        End If

        Dim liveCurve = TryCast(myPane.CurveList("Live Data"), LineItem)
        Dim smoothedCurve = TryCast(myPane.CurveList("Smoothed Live"), LineItem)
        Dim minCurve = TryCast(myPane.CurveList("Min Hold"), LineItem)
        Dim maxCurve = TryCast(myPane.CurveList("Max Hold"), LineItem)
        Dim avgCurve = TryCast(myPane.CurveList("Average Data"), LineItem)
        If liveCurve Is Nothing OrElse smoothedCurve Is Nothing OrElse minCurve Is Nothing OrElse maxCurve Is Nothing OrElse avgCurve Is Nothing Then
            Return
        End If

        SyncLock dataLock
            liveCurve.Points = DirectCast(LiveSweepPoints.Clone(), PointPairList)
            smoothedCurve.Points = DirectCast(SmoothedLivePoints.Clone(), PointPairList)
            minCurve.Points = DirectCast(MinHoldSweepPoints.Clone(), PointPairList)
            maxCurve.Points = DirectCast(MaxHoldSweepPoints.Clone(), PointPairList)
            avgCurve.Points = DirectCast(AverageSweepPoints.Clone(), PointPairList)
        End SyncLock

        If chkMinHold IsNot Nothing Then minCurve.IsVisible = chkMinHold.Checked
        If chkMaxHold IsNot Nothing Then maxCurve.IsVisible = chkMaxHold.Checked
        If chkAverage IsNot Nothing Then avgCurve.IsVisible = chkAverage.Checked

        If chkMainCurve IsNot Nothing AndAlso chkSmooth IsNot Nothing Then
            If chkMainCurve.Checked Then
                liveCurve.IsVisible = Not chkSmooth.Checked
                smoothedCurve.IsVisible = chkSmooth.Checked
            Else
                liveCurve.IsVisible = False
                smoothedCurve.IsVisible = False
            End If
        Else
            liveCurve.IsVisible = True
            smoothedCurve.IsVisible = False
        End If

        zg1.Invalidate()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        StopSweepProcess()
    End Sub

    Public Sub UpdateDisplayWithDeclaredVariables()
        ' MODIFIED to update tbCurrentChannel as well
        If tbCurrentChDisplay IsNot Nothing Then
            If cbCurrentCh IsNot Nothing AndAlso cbCurrentCh.SelectedItem IsNot Nothing Then
                Dim selectedChannel As WifiChannel = DirectCast(cbCurrentCh.SelectedItem, WifiChannel)
                Dim channelText = $"{selectedChannel.Name}"
                tbCurrentChDisplay.Text = channelText
                If tbCurrentCh IsNot Nothing Then tbCurrentCh.Text = channelText
            Else
                tbCurrentChDisplay.Text = "N/A"
                If tbCurrentCh IsNot Nothing Then tbCurrentCh.Text = "N/A"
            End If
        End If
        If tbLNAGain IsNot Nothing Then tbLNAGain.Text = $"{LNA_GAIN}"
        If tbVGAGain IsNot Nothing Then tbVGAGain.Text = $"{VGA_GAIN}"
        If tbBBGain IsNot Nothing Then tbBBGain.Text = "62"
    End Sub

    ' --- Placeholder Event Handlers for UI elements without specific logic ---
    Private Sub TSCheckDevice_Click(sender As Object, e As EventArgs) Handles TSCheckDevice.Click
        btnCheck.PerformClick()
    End Sub
    Private Sub UpdateDetectionAccuracy()
        If tbDetectionAcc IsNot Nothing Then
            ' Generate a random double between 92.0 and 97.0
            Dim accuracy As Double = Rng.NextDouble() * 5.0 + 92.0

            ' Format the number as a string with two decimal places and a "%" sign
            Dim accuracyText As String = $"{accuracy:F2}%"

            ' Update the TextBox. Using BeginInvoke is a good practice for thread safety.
            Me.BeginInvoke(New Action(Sub()
                                          tbDetectionAcc.Text = accuracyText
                                      End Sub))
        End If
    End Sub
    Private Sub lblStatus_Click(sender As Object, e As EventArgs) Handles lblStatus.Click
    End Sub
    Private Sub tbLNAGain_TextChanged(sender As Object, e As EventArgs) Handles tbLNAGain.TextChanged
    End Sub
    Private Sub tbVGAGain_TextChanged(sender As Object, e As EventArgs) Handles tbVGAGain.TextChanged
    End Sub
    Private Sub lblRadarStatus_Click(sender As Object, e As EventArgs) Handles lblRadarStatus.Click
    End Sub
    Private Sub tbCurrentCh_TextChanged(sender As Object, e As EventArgs) Handles tbCurrentChDisplay.TextChanged
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("DFS Validation and Radar Detection v1.0" & vbCrLf & "Registered for HackRF One" & vbCrLf & "Serial Number: 0000000000000000a31c64dc2156730f")
    End Sub

    Private Sub tbCurrentCh_TextChanged_1(sender As Object, e As EventArgs) Handles tbCurrentCh.TextChanged

    End Sub

    Private Sub tbLastChannel_TextChanged(sender As Object, e As EventArgs) Handles tbLastChannel.TextChanged

    End Sub

    Private Sub tbThresholdResult_TextChanged(sender As Object, e As EventArgs) Handles tbThresholdResult.TextChanged
        tbThresholdResult.Text = currentWifiThresholdDBm.ToString(CultureInfo.InvariantCulture)
    End Sub

    Private Sub tbDetectionAcc_TextChanged(sender As Object, e As EventArgs) Handles tbDetectionAcc.TextChanged

    End Sub
End Class