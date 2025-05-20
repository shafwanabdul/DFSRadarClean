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

Public Class MLPredictionService
    Private Shared ReadOnly client As New HttpClient()
    Private Const ML_API_URL As String = "http://127.0.0.1:5001/predict" ' Matches Python server

    Public Structure PredictionResponse
        Public Property predicted_class As String
        Public Property confidence As Double
        Public Property probabilities As List(Of Double)
        Public Property error_message As String ' For handling API errors
    End Structure

    Public Structure SpectrumRequest
        Public Property spectrum_dbm As List(Of Double)
    End Structure

    Public Shared Async Function GetInterferencePrediction(dbmValues As List(Of Double)) As Task(Of PredictionResponse)
        Dim response As New PredictionResponse()
        Try
            Dim requestPayload As New SpectrumRequest With {.spectrum_dbm = dbmValues}
            Dim jsonPayload As String = JsonConvert.SerializeObject(requestPayload)
            Dim content As New StringContent(jsonPayload, Encoding.UTF8, "application/json")

            Dim httpResponse As HttpResponseMessage = Await client.PostAsync(ML_API_URL, content)

            If httpResponse.IsSuccessStatusCode Then
                Dim jsonResponse As String = Await httpResponse.Content.ReadAsStringAsync()
                response = JsonConvert.DeserializeObject(Of PredictionResponse)(jsonResponse)
            Else
                Dim errorContent As String = Await httpResponse.Content.ReadAsStringAsync()
                Debug.WriteLine($"ML API Error: {httpResponse.StatusCode} - {errorContent}")
                response.error_message = $"API Error: {httpResponse.StatusCode}"
                ' Attempt to deserialize error from API if it sends structured errors
                Try
                    Dim errorObj = JsonConvert.DeserializeObject(Of Object)(errorContent) ' Or a specific error structure
                    response.error_message &= " - " & errorObj.ToString()
                Catch exJson As JsonException
                    ' Could not parse error, use raw content
                    response.error_message &= " - " & errorContent
                End Try
            End If
        Catch ex As HttpRequestException
            Debug.WriteLine($"HTTP Request Exception for ML API: {ex.Message}")
            response.error_message = "Network error calling ML service."
        Catch ex As JsonException
            Debug.WriteLine($"JSON Exception processing ML API response: {ex.Message}")
            response.error_message = "Invalid JSON response from ML service."
        Catch ex As Exception
            Debug.WriteLine($"Generic Exception calling ML API: {ex.ToString()}")
            response.error_message = "Unexpected error calling ML service."
        End Try
        Return response
    End Function
End Class

Public Class Form1
    ' Check Device
    Const HackRFPath As String = "C:\Program Files (x86)\HackRF\bin\hackrf_info.exe"
    Private mlUpdateTimer As System.Windows.Forms.Timer
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
            ' UpdateRadarStatusLabel() ' Could call here if you want to re-evaluate with existing data
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
    Private Const AMP_ENABLED As Boolean = True ' Set to True to try enabling amp (-a 1)
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

    ' --- Threshold Line ---
    Private thresholdLineObject As ZedGraph.LineObj ' The graphical object for the threshold line
    Private currentThresholdDBm As Double = -70.0 ' Default threshold value in dBm
    Private Const THRESHOLD_LINE_TAG As String = "ThresholdLine" ' Tag to easily find the line object

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

        SetupZedGraph() ' This will now also setup the initial threshold line

        cbCurrentCh.Enabled = False
        btnStartSweep.Enabled = False
        btnStopSweep.Enabled = False
        btnResetMin.Enabled = False ' Initially disable reset buttons
        btnResetMax.Enabled = False ' Initially disable reset buttons
        lblStatus.Text = "Status: Idle. Please check HackRF device."
        TSCheckDevice.Text = "HackRF Status: Unknown (Click Check HackRF)"

        ' Initialize lblRadarStatus
        If lblRadarStatus IsNot Nothing Then
            lblRadarStatus.Text = "Status: Initializing..." ' Or "No Interference Detected"
            lblRadarStatus.ForeColor = SystemColors.ControlText ' Default color
        End If

        AddHandler chkMainCurve.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkMinHold.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkMaxHold.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkAverage.CheckedChanged, AddressOf TraceSelectionChanged
        AddHandler chkSmooth.CheckedChanged, AddressOf TraceSelectionChanged

        AddHandler cbCurrentCh.SelectedIndexChanged, AddressOf CbCurrentCh_SelectedIndexChanged_Main

        ' --- Add handlers for new reset buttons ---
        AddHandler btnResetMin.Click, AddressOf btnResetMin_Click
        AddHandler btnResetMax.Click, AddressOf btnResetMax_Click
        ' --- End ---

        Debug.WriteLine("Form Loaded. ZedGraph Initialized. Channels Populated.")

        Dim defaultChannelName As String = "Ch 149 (5745 MHz)"
        Dim initialChannelIndex As Integer = -1
        For i As Integer = 0 To cbCurrentCh.Items.Count - 1
            If DirectCast(cbCurrentCh.Items(i), WifiChannel).Name.Contains("Ch 149") Then
                initialChannelIndex = i
                Exit For
            End If
        Next
        If initialChannelIndex <> -1 Then
            cbCurrentCh.SelectedIndex = initialChannelIndex
        ElseIf cbCurrentCh.Items.Count > 0 Then
            cbCurrentCh.SelectedIndex = 0
        End If

        If cbCurrentCh.SelectedItem IsNot Nothing Then
            UpdateFrequenciesFromChannel(DirectCast(cbCurrentCh.SelectedItem, WifiChannel))
            UpdateZedGraphXAxis()
        End If

        ' --- Threshold Line Initialization on Load ---
        If tbThreshold IsNot Nothing Then
            tbThreshold.Text = currentThresholdDBm.ToString(CultureInfo.InvariantCulture)
        End If
        If btnThreshold IsNot Nothing Then
            AddHandler btnThreshold.Click, AddressOf btnThreshold_Click
        End If
        ' --- End Threshold Line Initialization ---

        mlUpdateTimer = New System.Windows.Forms.Timer()
        mlUpdateTimer.Interval = 2000 ' Predict every 2 seconds
        AddHandler mlUpdateTimer.Tick, AddressOf M LUpdateTimer_Tick
        mlUpdateTimer.Start() ' Start it when HackRF is connected and sweeping

   
        UpdateDisplayWithDeclaredVariables()
        UpdateRadarStatusLabel() ' Set initial radar status
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
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 104 (5520 MHz, DFS)", .CenterFreqMHz = 5520, .IsDFS = True, .StartFreqMHz = 5520 - halfSpan, .StopFreqMHz = 5520 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 108 (5540 MHz, DFS)", .CenterFreqMHz = 5540, .IsDFS = True, .StartFreqMHz = 5540 - halfSpan, .StopFreqMHz = 5540 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 112 (5560 MHz, DFS)", .CenterFreqMHz = 5560, .IsDFS = True, .StartFreqMHz = 5560 - halfSpan, .StopFreqMHz = 5560 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 116 (5580 MHz, DFS)", .CenterFreqMHz = 5580, .IsDFS = True, .StartFreqMHz = 5580 - halfSpan, .StopFreqMHz = 5580 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 132 (5660 MHz, DFS)", .CenterFreqMHz = 5660, .IsDFS = True, .StartFreqMHz = 5660 - halfSpan, .StopFreqMHz = 5660 + halfSpan})
        AllWifiChannels.Add(New WifiChannel With {.Name = "Channel 136 (5680 MHz, DFS)", .CenterFreqMHz = 5680, .IsDFS = True, .StartFreqMHz = 5680 - halfSpan, .StopFreqMHz = 5680 + halfSpan})
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
                                          If lblRadarStatus Is Nothing Then
                                              Debug.WriteLine("UpdateRadarStatusLabel: lblRadarStatus is Nothing.")
                                              Return
                                          End If

                                          Dim interferenceDetected As Boolean = False
                                          SyncLock dataLock ' Ensure thread-safe access to MaxHoldSweepPoints
                                              If MaxHoldSweepPoints IsNot Nothing AndAlso MaxHoldSweepPoints.Count > 0 Then
                                                  For i As Integer = 0 To MaxHoldSweepPoints.Count - 1
                                                      If MaxHoldSweepPoints(i).Y > currentThresholdDBm Then
                                                          interferenceDetected = True
                                                          Exit For
                                                      End If
                                                  Next
                                              End If
                                          End SyncLock

                                          If interferenceDetected Then
                                              lblRadarStatus.Text = "Wi-Fi detected"
                                              lblRadarStatus.ForeColor = Color.Red ' Set color to red for warning
                                          Else
                                              lblRadarStatus.Text = "No Interference Detected"
                                              lblRadarStatus.ForeColor = Color.Green ' Set color to green for normal
                                          End If
                                      End Sub))
        End If
    End Sub

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
        Debug.WriteLine($"Channel selected: {selectedChannel.Name}, Start: {CurrentStartFreqMHz} MHz, Stop: {CurrentStopFreqMHz} MHz")
    End Sub

    Private Sub UpdateZedGraphXAxis()
        Dim myPane As GraphPane = zg1.GraphPane
        myPane.XAxis.Scale.Min = CurrentStartFreqMHz
        myPane.XAxis.Scale.Max = CurrentStopFreqMHz

        UpdateThresholdLineOnGraph() ' Update threshold line span

        zg1.AxisChange()
        zg1.Invalidate()
        Debug.WriteLine("ZedGraph X-Axis Updated.")
    End Sub

    Private Sub CbCurrentCh_SelectedIndexChanged_Main(sender As Object, e As EventArgs) Handles cbCurrentCh.SelectedIndexChanged
        If cbCurrentCh.SelectedItem IsNot Nothing Then
            Dim selectedChannel As WifiChannel = DirectCast(cbCurrentCh.SelectedItem, WifiChannel)
            UpdateFrequenciesFromChannel(selectedChannel)

            If SweepProcess IsNot Nothing AndAlso Not SweepProcess.HasExited Then
                StopSweepProcess()
                lblStatus.Text = "Status: Stopped (Channel Changed)."
            Else
                If TSCheckDevice.Text.Contains("Connected") Then
                    btnStartSweep.Enabled = True
                    btnResetMin.Enabled = True ' Ensure enabled if connected
                    btnResetMax.Enabled = True ' Ensure enabled if connected
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
                MaxHoldSweepPoints.Clear() ' MaxHold is cleared
                AverageSweepPoints.Clear()
                MinHoldDataStore.Clear()
                MaxHoldDataStore.Clear() ' MaxHoldDataStore is cleared
                AverageDataStore.Clear()
            End SyncLock
            UpdateChart()
            UpdateRadarStatusLabel() ' Update after channel change and data clear

            UpdateZedGraphXAxis()
            UpdateDisplayWithDeclaredVariables()
        End If
    End Sub

    Private Sub SetupZedGraph()
        If zg1 Is Nothing Then Return ' Should not happen if control is on form
        Dim myPane As GraphPane = zg1.GraphPane
        myPane.CurveList.Clear()
        myPane.GraphObjList.Clear() ' Clear previous graph objects, including old threshold line

        myPane.Title.Text = "HackRF Sweep Spectrum"
        myPane.Title.FontSpec.FontColor = Color.Black
        myPane.Title.FontSpec.IsBold = True

        myPane.XAxis.Title.Text = "Frequency (MHz)"
        myPane.XAxis.Title.FontSpec.FontColor = Color.Black
        myPane.XAxis.Scale.FontSpec.FontColor = Color.Black
        myPane.XAxis.MajorTic.Color = Color.DarkGray
        myPane.XAxis.MinorTic.Color = Color.LightGray

        myPane.YAxis.Title.Text = "Power (dBm)"
        myPane.YAxis.Title.FontSpec.FontColor = Color.Black
        myPane.YAxis.Scale.FontSpec.FontColor = Color.Black
        myPane.YAxis.MajorTic.Color = Color.DarkGray
        myPane.YAxis.MinorTic.Color = Color.LightGray

        myPane.Chart.Fill = New Fill(Color.White)
        myPane.Fill = New Fill(Color.FromArgb(230, 230, 230))

        myPane.XAxis.MajorGrid.IsVisible = True
        myPane.XAxis.MajorGrid.Color = Color.LightGray
        myPane.YAxis.MajorGrid.IsVisible = True
        myPane.YAxis.MajorGrid.Color = Color.LightGray

        myPane.XAxis.Scale.Min = CurrentStartFreqMHz
        myPane.XAxis.Scale.Max = CurrentStopFreqMHz
        myPane.YAxis.Scale.Min = -90
        myPane.YAxis.Scale.Max = 0

        Dim liveCurve As LineItem = myPane.AddCurve("Live Data", LiveSweepPoints, Color.Red, SymbolType.None)
        liveCurve.Line.Width = 1.5F
        liveCurve.IsVisible = True

        Dim smoothedCurve As LineItem = myPane.AddCurve("Smoothed Live", SmoothedLivePoints, Color.OrangeRed, SymbolType.None)
        smoothedCurve.Line.Width = 1.5F
        smoothedCurve.IsVisible = False

        Dim minCurve As LineItem = myPane.AddCurve("Min Hold", MinHoldSweepPoints, Color.DarkCyan, SymbolType.None)
        minCurve.Line.Width = 1.0F
        minCurve.IsVisible = False

        Dim maxCurve As LineItem = myPane.AddCurve("Max Hold", MaxHoldSweepPoints, Color.Goldenrod, SymbolType.None)
        maxCurve.Line.Width = 1.5F
        maxCurve.IsVisible = False

        Dim avgCurve As LineItem = myPane.AddCurve("Average Data", AverageSweepPoints, Color.Blue, SymbolType.None)
        avgCurve.Line.Width = 2.0F
        avgCurve.IsVisible = False

        ' --- Threshold Curve is no longer added directly here as a LineItem from thresholdLineObject ---
        ' It's a GraphObj added separately.
        ' We need to ensure that our CurveList indexing in UpdateChart is correct.
        ' The LineItem for "Threshold" was likely a misunderstanding. The LineObj is what matters.
        ' Let's remove the placeholder thresholdCurve LineItem if it's not actually used for points.
        ' If "Threshold" was meant to be a checkbox-toggleable GraphObj, that's different.
        ' For now, assuming only 5 data curves.
        ' Dim thresholdCurve As LineItem = myPane.AddCurve("Threshold", thresholdLineObject, Color.Magenta, SymbolType.None) ' This was problematic
        ' thresholdCurve.Line.Width = 2.0F
        ' thresholdCurve.IsVisible = False


        ' --- Add Threshold Line ---
        thresholdLineObject = New LineObj(Color.Magenta, myPane.XAxis.Scale.Min, currentThresholdDBm, myPane.XAxis.Scale.Max, currentThresholdDBm)
        'thresholdLineObject.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash ' This line can be uncommented if you want dashed style
        thresholdLineObject.Line.Width = 2.0F
        thresholdLineObject.IsVisible = True ' Controlled by a checkbox later if needed, or always on
        thresholdLineObject.ZOrder = ZOrder.A_InFront ' Draw it in front of grid
        thresholdLineObject.Tag = THRESHOLD_LINE_TAG
        myPane.GraphObjList.Add(thresholdLineObject)
        ' --- End Threshold Line ---

        myPane.Legend.IsVisible = True
        myPane.Legend.FontSpec.FontColor = Color.Black
        myPane.Legend.Fill = New Fill(Color.FromArgb(240, 240, 240, 200))
        myPane.Legend.Border.IsVisible = True
        myPane.Legend.Border.Color = Color.DarkGray

        zg1.AxisChange()
        zg1.Invalidate()
        Debug.WriteLine("ZedGraph Setup Complete with all curves and threshold line.")
    End Sub

    Private Sub btnThreshold_Click(sender As Object, e As EventArgs) Handles btnThreshold.Click
        Dim newThreshold As Double
        If tbThreshold Is Nothing Then
            Debug.WriteLine("tbThreshold is Nothing in btnThreshold_Click. Ensure it's on the form.")
            Return
        End If

        If Double.TryParse(tbThreshold.Text, NumberStyles.Float, CultureInfo.InvariantCulture, newThreshold) Then
            If zg1 IsNot Nothing AndAlso zg1.GraphPane IsNot Nothing Then
                currentThresholdDBm = newThreshold
                UpdateThresholdLineOnGraph()
                UpdateRadarStatusLabel() ' << ADD THIS CALL
                If lblStatus IsNot Nothing Then lblStatus.Text = $"Status: Threshold set to {currentThresholdDBm} dBm."
                Debug.WriteLine($"Threshold updated to: {currentThresholdDBm} dBm")
            Else
                Debug.WriteLine("ZedGraph control or Pane is not initialized in btnThreshold_Click.")
                currentThresholdDBm = newThreshold
                UpdateRadarStatusLabel() ' << ADD THIS CALL (if you want to update even if graph isn't ready)
                tbThreshold.Text = currentThresholdDBm.ToString(CultureInfo.InvariantCulture)
            End If
        Else
            MessageBox.Show("Invalid threshold value. Please enter a numeric value (e.g., -75.5).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            tbThreshold.Text = currentThresholdDBm.ToString(CultureInfo.InvariantCulture) ' Revert to old value
        End If
    End Sub

    Private Sub UpdateThresholdLineOnGraph()
        If zg1 Is Nothing OrElse zg1.GraphPane Is Nothing Then Return

        Dim myPane As GraphPane = zg1.GraphPane

        Dim foundObj As Object = myPane.GraphObjList.Find(Function(obj) obj.Tag IsNot Nothing AndAlso obj.Tag.ToString() = THRESHOLD_LINE_TAG)
        Dim existingLine As LineObj = TryCast(foundObj, LineObj)

        If existingLine IsNot Nothing Then
            thresholdLineObject = existingLine ' Update our class-level reference
        Else
            Debug.WriteLine("Threshold line object not found in GraphObjList, re-creating.")
            thresholdLineObject = New LineObj(Color.Magenta, myPane.XAxis.Scale.Min, currentThresholdDBm, myPane.XAxis.Scale.Max, currentThresholdDBm)
            'thresholdLineObject.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash ' Uncomment for dashed style
            thresholdLineObject.Line.Width = 2.0F
            thresholdLineObject.IsVisible = True
            thresholdLineObject.ZOrder = ZOrder.A_InFront
            thresholdLineObject.Tag = THRESHOLD_LINE_TAG
            myPane.GraphObjList.Add(thresholdLineObject)
        End If

        thresholdLineObject.Location.Y = currentThresholdDBm ' Set Y position for both points

        thresholdLineObject.Location.X = myPane.XAxis.Scale.Min ' Set X start

        thresholdLineObject.IsVisible = True ' Ensure it's visible (can be controlled by a checkbox too)

        zg1.Invalidate() ' Redraw the chart
    End Sub

    ' Add this field to your Form1 class
    Private mlModelPrediction As MLPredictionService.PredictionResponse

    Private Async Sub UpdateRadarStatusWithML()
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            Dim dataToPredict As List(Of Double) = Nothing
            SyncLock dataLock ' Ensure thread-safe access
                ' Decide what data to send: Live, MaxHold, or Smoothed.
                ' MaxHold is probably good for persistent interference.
                ' LiveSmoothed for more immediate, less noisy reaction.
                If MaxHoldSweepPoints IsNot Nothing AndAlso MaxHoldSweepPoints.Count > 0 Then
                    dataToPredict = MaxHoldSweepPoints.Select(Function(p) p.Y).ToList()
                    ' ElseIf SmoothedLivePoints IsNot Nothing AndAlso SmoothedLivePoints.Count > 0 Then
                    '    dataToPredict = SmoothedLivePoints.Select(Function(p) p.Y).ToList()
                Else
                    ' No data to predict
                    If lblRadarStatus IsNot Nothing Then
                        lblRadarStatus.Text = "Status: No Data for ML"
                        lblRadarStatus.ForeColor = SystemColors.ControlText
                    End If
                    Return
                End If
            End SyncLock

            If dataToPredict IsNot Nothing AndAlso dataToPredict.Any() Then
                ' Asynchronously get prediction
                mlModelPrediction = Await MLPredictionService.GetInterferencePrediction(dataToPredict)

                ' Update UI on the UI thread
                Me.BeginInvoke(New Action(Sub()
                                              If lblRadarStatus Is Nothing Then
                                                  Debug.WriteLine("UpdateRadarStatusWithML: lblRadarStatus is Nothing.")
                                                  Return
                                              End If

                                              If Not String.IsNullOrEmpty(mlModelPrediction.error_message) Then
                                                  lblRadarStatus.Text = $"ML: {mlModelPrediction.error_message}"
                                                  lblRadarStatus.ForeColor = Color.OrangeRed
                                              ElseIf Not String.IsNullOrEmpty(mlModelPrediction.predicted_class) Then
                                                  lblRadarStatus.Text = $"ML: {mlModelPrediction.predicted_class} ({mlModelPrediction.confidence:P1})"
                                                  ' Customize color based on prediction
                                                  Select Case mlModelPrediction.predicted_class.ToLower()
                                                      Case "wifi", "wi-fi", "wifi+radar", "wi-fi+radar", "radar"
                                                          lblRadarStatus.ForeColor = Color.Red
                                                      Case "no interference", "clear"
                                                          lblRadarStatus.ForeColor = Color.Green
                                                      Case Else
                                                          lblRadarStatus.ForeColor = Color.DarkOrange
                                                  End Select
                                              Else
                                                  lblRadarStatus.Text = "ML: No prediction"
                                                  lblRadarStatus.ForeColor = SystemColors.ControlText
                                              End If
                                          End Sub))
            Else
                Me.BeginInvoke(New Action(Sub()
                                              If lblRadarStatus IsNot Nothing Then
                                                  lblRadarStatus.Text = "Status: No Data"
                                                  lblRadarStatus.ForeColor = SystemColors.ControlText
                                              End If
                                          End Sub))
            End If
        End If
    End Sub
    Private Sub btnStartSweep_Click(sender As Object, e As EventArgs) Handles btnStartSweep.Click
        Debug.WriteLine("Start Sweep Button Clicked.")
        If SweepProcess IsNot Nothing AndAlso Not SweepProcess.HasExited Then
            MessageBox.Show("Sweep is already running.")
            Debug.WriteLine("Sweep already running, returning.")
            Return
        End If

        If Not TSCheckDevice.Text.Contains("Connected") Then
            MessageBox.Show("HackRF is not connected. Please click 'Check HackRF' and ensure it's connected.", "HackRF Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Debug.WriteLine("Attempted to start sweep, but HackRF not reported as connected.")
            Return
        End If

        If Not File.Exists(HACKRF_SWEEP_PATH) Then
            MessageBox.Show($"hackrf_sweep.exe not found at: {HACKRF_SWEEP_PATH}{vbCrLf}" &
                            "Please update the path in the code or ensure radioconda environment is active if using relative paths.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"hackrf_sweep.exe not found at: {HACKRF_SWEEP_PATH}")
            Return
        End If

        SyncLock dataLock
            LiveSweepPoints.Clear()
            SmoothedLivePoints.Clear()
            ' Min/Max/Average are NOT cleared here, they persist across sweeps unless manually reset or channel changes
        End SyncLock
        UpdateChart() ' This also refreshes ZedGraph with empty live points
        Debug.WriteLine("Live trace data reset for new sweep. Min/Max/Avg persist.")

        Dim startFreqMHzStr As String = CurrentStartFreqMHz.ToString(CultureInfo.InvariantCulture)
        Dim stopFreqMHzStr As String = CurrentStopFreqMHz.ToString(CultureInfo.InvariantCulture)
        Dim binWidthHz As Long = CLng(BIN_WIDTH_KHZ * 1000)
        Dim ampArg As String = If(AMP_ENABLED, "-a 1", "-a 0")

        Dim HACKRF_SERIAL_FOR_SWEEP As String = "0000000000000000a31c64dc2156730f" ' IMPORTANT: Set this to your HackRF's serial if you have multiple or specific one
        Dim arguments As String
        If String.IsNullOrWhiteSpace(HACKRF_SERIAL_FOR_SWEEP) Then
            arguments = $"-f {startFreqMHzStr}:{stopFreqMHzStr} -w {binWidthHz} -l {LNA_GAIN} -g {VGA_GAIN} {ampArg}"
        Else
            arguments = $"-d {HACKRF_SERIAL_FOR_SWEEP} -f {startFreqMHzStr}:{stopFreqMHzStr} -w {binWidthHz} -l {LNA_GAIN} -g {VGA_GAIN} {ampArg}"
        End If
        Debug.WriteLine($"hackrf_sweep arguments: {arguments}")


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
            Debug.WriteLine("SweepProcess Started.")
            SweepProcess.BeginOutputReadLine()
            SweepProcess.BeginErrorReadLine()
            Debug.WriteLine("BeginOutputReadLine and BeginErrorReadLine called.")

            btnStartSweep.Enabled = False
            btnStopSweep.Enabled = True
            cbCurrentCh.Enabled = False
            ' Reset buttons remain enabled as per HackRF connection status
            lblStatus.Text = "Status: Sweeping..."

        Catch ex As Exception
            MessageBox.Show($"Failed to start hackrf_sweep: {ex.Message}{vbCrLf}Ensure HackRF tools are correctly installed and in PATH, or HACKRF_SWEEP_PATH is absolute.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Debug.WriteLine($"EXCEPTION starting process: {ex.ToString()}")
            SweepProcess = Nothing
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
            lblStatus.Text = "Status: Error starting sweep."
        End Try
    End Sub

    Private Sub SweepProcess_Exited(sender As Object, e As EventArgs)
        Debug.WriteLine("SweepProcess_Exited event fired.")
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            Me.BeginInvoke(New Action(Sub()
                                          If SweepProcess IsNot Nothing Then
                                              If btnStopSweep.Enabled Then ' Process exited but user didn't click stop
                                                  lblStatus.Text = "Status: Sweep process ended unexpectedly."
                                                  StopSweepProcess() ' Ensure cleanup
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
        Debug.WriteLine("Stop Sweep Button Clicked.")
        StopSweepProcess()
        lblStatus.Text = "Status: Stopped by user."
    End Sub

    Private Sub StopSweepProcess()
        Debug.WriteLine("Attempting to stop sweep process...")

        If SweepProcess IsNot Nothing Then
            RemoveHandler SweepProcess.Exited, AddressOf SweepProcess_Exited

            If Not SweepProcess.HasExited Then
                Try
                    Debug.WriteLine("Removing event handlers for Output/Error...")
                    RemoveHandler SweepProcess.OutputDataReceived, AddressOf SweepOutputHandler
                    RemoveHandler SweepProcess.ErrorDataReceived, AddressOf SweepErrorHandler

                    If Not SweepProcess.HasExited Then ' Double check before trying to cancel/kill
                        Debug.WriteLine("Cancelling output read...")
                        Try
                            SweepProcess.CancelOutputRead()
                            SweepProcess.CancelErrorRead()
                        Catch exCancel As InvalidOperationException
                            Debug.WriteLine($"Info: Error cancelling reads, process likely already closing: {exCancel.Message}")
                        End Try

                        Debug.WriteLine("Killing process...")
                        SweepProcess.Kill()
                        Debug.WriteLine("Waiting for exit (short)...")
                        If Not SweepProcess.WaitForExit(1500) Then ' Short timeout
                            Debug.WriteLine("Process did not exit gracefully after kill signal within 1.5s.")
                        Else
                            Debug.WriteLine("Process exited after kill signal.")
                        End If
                    End If
                Catch exIO As InvalidOperationException
                    Debug.WriteLine($"Info during stop: Process likely already exited or streams closed: {exIO.Message}")
                Catch ex As Exception
                    Debug.WriteLine($"Error during sweep process stop: {ex.Message}")
                End Try
            Else
                Debug.WriteLine("SweepProcess had already exited before explicit stop measures.")
            End If
            SweepProcess.Dispose()
            SweepProcess = Nothing
        Else
            Debug.WriteLine("SweepProcess is already Nothing.")
        End If

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(AddressOf UpdateUIStopState))
        Else
            UpdateUIStopState()
        End If
        Debug.WriteLine("StopSweepProcess finished.")
    End Sub

    Private Sub UpdateUIStopState()
        If TSCheckDevice.Text.Contains("Connected") Then
            btnStartSweep.Enabled = True
            cbCurrentCh.Enabled = True
            btnResetMin.Enabled = True ' Keep enabled if connected
            btnResetMax.Enabled = True ' Keep enabled if connected
        Else
            btnStartSweep.Enabled = False
            cbCurrentCh.Enabled = False
            btnResetMin.Enabled = False
            btnResetMax.Enabled = False
        End If
        btnStopSweep.Enabled = False

        If lblStatus.Text = "Status: Sweeping..." OrElse
           lblStatus.Text = "Status: Sweep process ended unexpectedly." OrElse
           lblStatus.Text = "Status: Sweep process ended." Then
            lblStatus.Text = "Status: Idle."
        End If
    End Sub

    ' --- New Reset Button Handlers ---
    Private Sub btnResetMin_Click(sender As Object, e As EventArgs) Handles btnResetMin.Click
        Debug.WriteLine("btnResetMin Clicked")
        SyncLock dataLock
            MinHoldDataStore.Clear()
            MinHoldSweepPoints.Clear()
        End SyncLock
        UpdateChart()
        lblStatus.Text = "Status: Min Hold trace reset."
    End Sub

    Private Sub btnResetMax_Click(sender As Object, e As EventArgs) Handles btnResetMax.Click
        Debug.WriteLine("btnResetMax Clicked")
        SyncLock dataLock
            MaxHoldDataStore.Clear()
            MaxHoldSweepPoints.Clear()
        End SyncLock
        UpdateChart()
        UpdateRadarStatusLabel() ' << ADD THIS CALL
        lblStatus.Text = "Status: Max Hold trace reset."
    End Sub
    ' --- End New Reset Button Handlers ---


    Private Function ApplyMovingAverage(inputPoints As PointPairList, windowSize As Integer) As PointPairList
        Dim smoothedPoints As New PointPairList()
        If inputPoints Is Nothing OrElse inputPoints.Count = 0 Then
            Return smoothedPoints
        End If
        If windowSize <= 1 Then ' No smoothing or invalid window, return clone
            Return DirectCast(inputPoints.Clone(), PointPairList)
        End If

        Dim actualWindowSize = windowSize
        If actualWindowSize Mod 2 = 0 Then actualWindowSize += 1 ' Ensure odd
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
                smoothedPoints.Add(inputPoints(i).X, inputPoints(i).Y) ' Should not happen
            End If
        Next
        Return smoothedPoints
    End Function


    Private Sub SweepOutputHandler(sendingProcess As Object, outLine As DataReceivedEventArgs)
        If outLine.Data Is Nothing Then
            Return
        End If

        If Not String.IsNullOrEmpty(outLine.Data) Then
            Try
                Dim parts As String() = outLine.Data.Split(","c)

                If parts.Length > 6 Then ' Basic check for valid data line format
                    Dim hzLow As Double = Double.Parse(parts(2), CultureInfo.InvariantCulture)
                    Dim binWidthHz As Double = Double.Parse(parts(4), CultureInfo.InvariantCulture)
                    Dim numBins As Integer = parts.Length - 6 ' Number of dBm values

                    Dim currentLivePoints As New ZedGraph.PointPairList()

                    Dim minHoldNeedsUpdate As Boolean = False
                    Dim maxHoldNeedsUpdateLocal As Boolean = False ' Use a local flag for clarity
                    Dim averageNeedsUpdate As Boolean = False

                    For i As Integer = 0 To numBins - 1
                        Dim centerFreqHz As Double = hzLow + (i * binWidthHz) + (binWidthHz / 2.0)
                        Dim freqMHz As Double = centerFreqHz / 1000000.0
                        Dim dbmValue As Double = Double.Parse(parts(6 + i), CultureInfo.InvariantCulture)
                        currentLivePoints.Add(freqMHz, dbmValue)
                    Next

                    SyncLock dataLock
                        LiveSweepPoints = currentLivePoints ' Assign the newly read full sweep
                        SmoothedLivePoints = ApplyMovingAverage(LiveSweepPoints, SMOOTHING_WINDOW_SIZE)

                        For i As Integer = 0 To LiveSweepPoints.Count - 1 ' Use LiveSweepPoints for calculations
                            Dim freqMHz As Double = LiveSweepPoints(i).X
                            Dim dbmValue As Double = LiveSweepPoints(i).Y

                            ' MinHold Logic
                            If MinHoldDataStore.ContainsKey(freqMHz) Then
                                If dbmValue < MinHoldDataStore(freqMHz) Then
                                    MinHoldDataStore(freqMHz) = dbmValue
                                    minHoldNeedsUpdate = True
                                End If
                            Else
                                MinHoldDataStore.Add(freqMHz, dbmValue)
                                minHoldNeedsUpdate = True
                            End If

                            ' MaxHold Logic
                            If MaxHoldDataStore.ContainsKey(freqMHz) Then
                                If dbmValue > MaxHoldDataStore(freqMHz) Then
                                    MaxHoldDataStore(freqMHz) = dbmValue
                                    maxHoldNeedsUpdateLocal = True ' Set local flag
                                End If
                            Else
                                MaxHoldDataStore.Add(freqMHz, dbmValue)
                                maxHoldNeedsUpdateLocal = True ' Set local flag
                            End If

                            ' Average Logic
                            If AverageDataStore.ContainsKey(freqMHz) Then
                                Dim currentTuple As Tuple(Of Double, Integer) = AverageDataStore(freqMHz)
                                AverageDataStore(freqMHz) = Tuple.Create(currentTuple.Item1 + dbmValue, currentTuple.Item2 + 1)
                            Else
                                AverageDataStore.Add(freqMHz, Tuple.Create(dbmValue, 1))
                            End If
                            averageNeedsUpdate = True ' Always true as we add a new sample
                        Next

                        If minHoldNeedsUpdate Then
                            MinHoldSweepPoints.Clear()
                            Dim sortedMinKeys = MinHoldDataStore.Keys.OrderBy(Function(k) k)
                            For Each keyFreqMHz As Double In sortedMinKeys
                                MinHoldSweepPoints.Add(keyFreqMHz, MinHoldDataStore(keyFreqMHz))
                            Next
                        End If
                        If maxHoldNeedsUpdateLocal Then ' Use the local flag to decide if MaxHoldSweepPoints needs rebuild
                            MaxHoldSweepPoints.Clear()
                            Dim sortedMaxKeys = MaxHoldDataStore.Keys.OrderBy(Function(k) k)
                            For Each keyFreqMHz As Double In sortedMaxKeys
                                MaxHoldSweepPoints.Add(keyFreqMHz, MaxHoldDataStore(keyFreqMHz))
                            Next
                        End If
                        If averageNeedsUpdate Then ' Always true, so always rebuild
                            AverageSweepPoints.Clear()
                            Dim sortedAvgKeys = AverageDataStore.Keys.OrderBy(Function(k) k)
                            For Each keyFreqMHz As Double In sortedAvgKeys
                                Dim avgDb As Double = AverageDataStore(keyFreqMHz).Item1 / AverageDataStore(keyFreqMHz).Item2
                                AverageSweepPoints.Add(keyFreqMHz, avgDb)
                            Next
                        End If
                    End SyncLock

                    If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                        Me.BeginInvoke(New Action(AddressOf UpdateChart))
                        If maxHoldNeedsUpdateLocal Then ' If max hold data was updated in this batch
                            UpdateRadarStatusLabel() ' << ADD THIS CALL (conditionally)
                        End If
                    End If
                End If
            Catch exFE As FormatException
                Debug.WriteLine($"Format error parsing sweep output: {exFE.Message} - Line: {outLine.Data}")
            Catch ex As Exception
                Debug.WriteLine($"Error processing sweep output: {ex.Message} - Line: {outLine.Data}")
            End Try
        End If
    End Sub

    Private Sub SweepErrorHandler(sendingProcess As Object, errLine As DataReceivedEventArgs)
        If Not String.IsNullOrEmpty(errLine.Data) Then
            Debug.WriteLine($"HACKRF_SWEEP ERROR: {errLine.Data}")
            If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
                Me.BeginInvoke(New Action(Sub()
                                              If lblStatus IsNot Nothing Then lblStatus.Text = "Status: Sweep Error."
                                              If errLine.Data.ToLower().Contains("no hackrf device found") OrElse errLine.Data.ToLower().Contains("hackrf_error_not_found") Then
                                                  Debug.WriteLine("Critical error from hackrf_sweep, stopping process.")
                                                  StopSweepProcess() ' Attempt to stop and reset UI
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
        If zg1 Is Nothing OrElse zg1.GraphPane Is Nothing Then
            Debug.WriteLine("UpdateChart: zg1 or GraphPane is Nothing. Aborting.")
            Return
        End If

        Dim myPane As GraphPane = zg1.GraphPane

        ' Ensure curves exist, if not, SetupZedGraph (which also re-acquires pane)
        If myPane.CurveList.Count < 5 Then ' Live, Smoothed, Min, Max, Avg
            Debug.WriteLine($"UpdateChart: CurveList count is {myPane.CurveList.Count}, expected at least 5. Re-setting up ZedGraph.")
            SetupZedGraph()
            myPane = zg1.GraphPane ' Re-acquire pane after setup
            If myPane.CurveList.Count < 5 Then
                Debug.WriteLine("UpdateChart: Still not enough curves after SetupZedGraph. Aborting.")
                Return
            End If
        End If

        Dim liveCurve As LineItem = TryCast(myPane.CurveList("Live Data"), LineItem)
        Dim smoothedCurve As LineItem = TryCast(myPane.CurveList("Smoothed Live"), LineItem)
        Dim minCurve As LineItem = TryCast(myPane.CurveList("Min Hold"), LineItem)
        Dim maxCurve As LineItem = TryCast(myPane.CurveList("Max Hold"), LineItem)
        Dim avgCurve As LineItem = TryCast(myPane.CurveList("Average Data"), LineItem)

        If liveCurve Is Nothing OrElse smoothedCurve Is Nothing OrElse minCurve Is Nothing OrElse maxCurve Is Nothing OrElse avgCurve Is Nothing Then
            Debug.WriteLine("UpdateChart: One or more curves not found by name. Re-setting up ZedGraph.")
            SetupZedGraph() ' Attempt to fix by re-initializing graph components
            myPane = zg1.GraphPane ' Re-acquire pane
            liveCurve = TryCast(myPane.CurveList("Live Data"), LineItem)
            smoothedCurve = TryCast(myPane.CurveList("Smoothed Live"), LineItem)
            minCurve = TryCast(myPane.CurveList("Min Hold"), LineItem)
            maxCurve = TryCast(myPane.CurveList("Max Hold"), LineItem)
            avgCurve = TryCast(myPane.CurveList("Average Data"), LineItem)
            If liveCurve Is Nothing OrElse smoothedCurve Is Nothing OrElse minCurve Is Nothing OrElse maxCurve Is Nothing OrElse avgCurve Is Nothing Then
                Debug.WriteLine("UpdateChart: Failed to get curves by name even after re-setup. Aborting.")
                Return
            End If
        End If

        SyncLock dataLock
            liveCurve.Points = DirectCast(LiveSweepPoints.Clone(), PointPairList)
            smoothedCurve.Points = DirectCast(SmoothedLivePoints.Clone(), PointPairList)
            minCurve.Points = DirectCast(MinHoldSweepPoints.Clone(), PointPairList)
            maxCurve.Points = DirectCast(MaxHoldSweepPoints.Clone(), PointPairList)
            avgCurve.Points = DirectCast(AverageSweepPoints.Clone(), PointPairList)
        End SyncLock

        ' Checkbox controls visibility
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
        Else ' Fallback if checkboxes are somehow null
            liveCurve.IsVisible = True ' Default visibility
            smoothedCurve.IsVisible = False
        End If

        zg1.Invalidate()
    End Sub


    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Debug.WriteLine("Form Closing event.")
        StopSweepProcess() ' Ensure sweep process is terminated
    End Sub

    Public Sub UpdateDisplayWithDeclaredVariables()
        If tbCurrentChDisplay IsNot Nothing Then
            If cbCurrentCh IsNot Nothing AndAlso cbCurrentCh.SelectedItem IsNot Nothing Then
                Dim selectedChannel As WifiChannel = DirectCast(cbCurrentCh.SelectedItem, WifiChannel)
                tbCurrentChDisplay.Text = $"{selectedChannel.Name}"
            Else
                tbCurrentChDisplay.Text = "N/A"
            End If
        End If

        If tbLNAGain IsNot Nothing Then
            tbLNAGain.Text = $"{LNA_GAIN} dB"
        End If

        If tbVGAGain IsNot Nothing Then
            tbVGAGain.Text = $"{VGA_GAIN} dB"
        End If

        If tbAMPStatus IsNot Nothing Then
            tbAMPStatus.Text = If(AMP_ENABLED, "Enabled", "Disabled")
        End If
    End Sub

    Private Sub ToolStripStatusLabel2_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel2.Click
        ' Placeholder for ToolStripStatusLabel2 if it exists and has this handler.
    End Sub

    Private Sub TSCheckDevice_Click(sender As Object, e As EventArgs) Handles TSCheckDevice.Click
        ' If you want this to trigger the button click:
        btnCheck.PerformClick()
    End Sub

    Private Sub lblStatus_Click(sender As Object, e As EventArgs) Handles lblStatus.Click
        ' Placeholder
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles tbLNAGain.TextChanged
        ' Placeholder - TextChanged event for display-only LNA gain textbox
    End Sub

    Private Sub tbIFGain_TextChanged(sender As Object, e As EventArgs) Handles tbVGAGain.TextChanged
        ' Placeholder - TextChanged event for display-only VGA gain textbox (assuming tbIFGain was tbVGAGain)
    End Sub

    Private Sub lblRadarStatus_Click(sender As Object, e As EventArgs) Handles lblRadarStatus.Click

    End Sub
End Class