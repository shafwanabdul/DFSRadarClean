# DFS-Radar Smart Classifications

A Software-Defined Radio (SDR)-based system for dynamic frequency selection (DFS) and radar signal detection in the 5GHz Wi-Fi band. This project integrates HackRF hardware, radar pulse simulation, and Machine Learning (1D CNN) for real-time Wi-Fi channel switching and radar classification.

## 📌 Features
- Detects radar pulses using HackRF SDR
- Implements DFS logic for Wi-Fi coexistence
- Simulates radar pulses (FCC Type 4) using GNU Radio
- Classifies radar signals with CNN (1D Convolutional Neural Network)
- Visualizes spectrum data and classification results in a GUI (VB.NET)

## 🛠 Technologies Used
- HackRF (SDR Hardware)
- GNU Radio (Radar Pulse Simulation)
- Python (Signal Processing, ML)
- VB.NET (GUI for visualization and control)
- ZedGraph (for plotting in VB.NET)
- Wireshark / CommView (Wi-Fi packet analysis)

## 🚀 How to Run

1. Clone this repository:
   ```bash
   git clone https://github.com/shafwanabdul/DFSRadarClean.git
2. Install Python Dependencies
   ```bash
    pip install -r numpy, scipy, matplotlib, keras, tensorflow, scikit-learn
3. Set Up GNU Radio
   - Visit the [GNU Radio official website](https://www.gnuradio.org/) for more information.
   - Install GNU Radio
   - Open the .grc file from gnu_radio/ and run the radar pulse simulation
     
4. Launch SDR Capture + GUI
   - Connect your HackRF device.
   - Open the Visual Basic project in vbnet_ui/ using Visual Studio.
   - Run the DFS Radar Clean GUI to begin spectrum monitoring and ML-based classification.
  
## 🤖 Machine Learning Model
- Architecture: 1D Convolutional Neural Network (CNN)
- Input: Power-time signal samples (radar & non-radar)
- Training Data: Synthetic radar pulses + Wi-Fi packets
- Accuracy: ~92% on FCC Type 4 radar detection
- Framework: Keras with TensorFlow backend

## 🧠 DFS Decision Logic
1. SDR captures real-time signal using HackRF
2. Signal is processed and threshold-checked
3. Classified using trained CNN model
4. If radar detected → Channel switch is triggered
5. Log data: Frequency center, TG level, sweep time

## 👨‍💻 Author
Muhamad Shafwan Abdul Aziz
📍 Bandung, Indonesia
📧 muhamadshafwan10@gmail.com
🔗 [LinkedIn](https://www.linkedin.com/in/shafwanabdul/)
🐙 [GitHub](https://github.com/shafwanabdul/)

## 🙏 Acknowledgements
- GNU Radio open-source community
- FCC radar test waveform documentation
- OpenWRT firmware developers
- Politeknik Negeri Bandung (POLBAN)
