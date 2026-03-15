using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace App.Utils
{
    // Lớp EventArgs tùy chỉnh để gửi đường dẫn file
    public class RecordingStoppedEventArgs : EventArgs
    {
        public string FilePath { get; set; }
    }

    public class VoiceHelper
    {
        private Button voiceButton;
        private bool isRecording = false;

        private WaveInEvent waveIn;
        private WaveFileWriter writer;
        private string outputFile = "record.wav";

        // Event để thông báo cho Form chính khi ghi âm xong
        public event EventHandler<RecordingStoppedEventArgs> RecordingFinished;

        // Constructor đã được rút gọn
        public VoiceHelper(Button button)
        {
            this.voiceButton = button;
            this.voiceButton.Text = "🎤 Ghi âm";
            this.voiceButton.BackColor = System.Drawing.Color.LightGreen;
            this.voiceButton.Click += VoiceButton_Click;
        }

        private void VoiceButton_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                try
                {
                    waveIn = new WaveInEvent();
                    waveIn.WaveFormat = new WaveFormat(16000, 1);
                    waveIn.DataAvailable += OnDataAvailable;
                    waveIn.RecordingStopped += OnRecordingStopped;

                    writer = new WaveFileWriter(outputFile, waveIn.WaveFormat);
                    waveIn.StartRecording();

                    isRecording = true;
                    voiceButton.BackColor = System.Drawing.Color.Red;
                    voiceButton.Text = "🛑 Dừng ghi";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi bắt đầu ghi: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    waveIn?.StopRecording();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi dừng ghi: " + ex.Message);
                }
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            writer?.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            // 1. Giải phóng tài nguyên
            writer?.Dispose();
            writer = null;
            waveIn?.Dispose();
            waveIn = null;

            // 2. Cập nhật UI nút bấm
            voiceButton.FindForm()?.BeginInvoke(new Action(() =>
            {
                isRecording = false;
                voiceButton.BackColor = System.Drawing.Color.LightGreen;
                voiceButton.Text = "🎤 Ghi âm";
            }));

            // 3. Gửi sự kiện (event) về cho form (fChatPopup)
            RecordingFinished?.Invoke(this, new RecordingStoppedEventArgs { FilePath = outputFile });
        }
    }
}