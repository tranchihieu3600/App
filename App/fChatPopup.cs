using App.Utils; // <- Namespace của VoiceHelper và RasaClient
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace App
{
    public partial class fChatPopup : Form
    {
        // === CÁC BIẾN CHO CHAT & VOICE ===
        private VoiceHelper voiceHelper; // Lớp ghi âm
        private RasaClient rasaClient;   // Lớp gọi Rasa
        private string openAiApiKey;     // Key Whisper

        public fChatPopup()
        {
            InitializeComponent();

            // === KHỞI TẠO LOGIC CHAT & VOICE ===

            // 1. Khởi tạo VoiceHelper (truyền nút btnVoice trên form NÀY)
            voiceHelper = new VoiceHelper(btnVoice);
            voiceHelper.RecordingFinished += VoiceHelper_RecordingFinished; // Đăng ký sự kiện

            

            // 3. Gắn sự kiện cho các control chat
            this.btnSendChat.Click += new System.EventHandler(this.btnSendChat_Click);
            this.txtChatInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtChatInput_KeyPress);

            // 4. Gửi lời chào ban đầu
            AppendMessageToChat("Bot:", "Xin chào, tôi được xây dựng cho chức năng gọi món bằng văn bản và lời nói, bạn muốn ăn gì hôm nay?", Color.RoyalBlue);
        }

        // Hàm này được file Designer yêu cầu
        private void fChatPopup_Load(object sender, EventArgs e)
        {
            // Bạn có thể để trống hàm này.
        }
        // ===================================

        // === CÁC HÀM CHO LOGIC CHAT ===

        // 1. Khi nút Gửi (btnSendChat) được nhấn
        private async void btnSendChat_Click(object sender, EventArgs e)
        {
            string message = txtChatInput.Text;
            if (string.IsNullOrWhiteSpace(message)) return;

            AppendMessageToChat("Bạn:", message, Color.DarkGreen);
            txtChatInput.Clear();
            await SendMessageToRasa(message);
        }

        // 2. Khi ghi âm xong (Sự kiện từ VoiceHelper)
        private async void VoiceHelper_RecordingFinished(object sender, RecordingStoppedEventArgs e)
        {
            try
            {
                AppendMessageToChat("Bot:", "[Đang xử lý giọng nói...]", Color.Gray);
                string transcript = await ConvertSpeechToText_OpenAI(this.openAiApiKey, e.FilePath);
                AppendMessageToChat("Bạn (Giọng nói):", transcript, Color.DarkGreen);
                await SendMessageToRasa(transcript);
            }
            catch (Exception ex)
            {
                AppendMessageToChat("Lỗi Whisper:", ex.Message, Color.Red);
            }
        }

        // 3. Hàm Lõi: Gửi tin nhắn tới Rasa và hiển thị phản hồi
        private async Task SendMessageToRasa(string message)
        {
            try
            {
                var rasaResponse = await rasaClient.SendMessageAsync(message);
                List<string> botReplies = new List<string>();
                foreach (var msg in rasaResponse)
                {
                    if (msg["text"] != null)
                    {
                        botReplies.Add(msg["text"].ToString());
                    }
                }

                if (botReplies.Any())
                {
                    string combinedReply = string.Join("\n", botReplies);
                    AppendMessageToChat("Bot:", combinedReply, Color.RoyalBlue);
                }
                else
                {
                    AppendMessageToChat("Bot:", "[Không nhận được phản hồi]", Color.Gray);
                }
            }
            catch (Exception ex)
            {
                AppendMessageToChat("Lỗi Rasa:", ex.Message, Color.Red);
            }
        }

        // 4. Hàm trợ giúp: Thêm tin nhắn vào RichTextBox (rtbChatHistory)
        private void AppendMessageToChat(string sender, string message, Color color)
        {
            if (rtbChatHistory.InvokeRequired)
            {
                rtbChatHistory.BeginInvoke(new Action(() => AppendMessageToChat(sender, message, color)));
                return;
            }

            rtbChatHistory.SelectionStart = rtbChatHistory.TextLength;
            rtbChatHistory.SelectionLength = 0;
            rtbChatHistory.SelectionFont = new Font(rtbChatHistory.Font, FontStyle.Bold);
            rtbChatHistory.SelectionColor = color;
            rtbChatHistory.AppendText(sender + " ");
            rtbChatHistory.SelectionFont = new Font(rtbChatHistory.Font, FontStyle.Regular);
            rtbChatHistory.SelectionColor = Color.Black;
            rtbChatHistory.AppendText(message + "\n\n");
            rtbChatHistory.ScrollToCaret();
        }

        // 5. Hàm trợ giúp: Gõ Enter để gửi
        private void txtChatInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                btnSendChat_Click(sender, e);
            }
        }

        // 6. Hàm trợ giúp: Gọi API Whisper
        private async Task<string> ConvertSpeechToText_OpenAI(string apiKey, string audioFile)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                using (var content = new MultipartFormDataContent())
                {
                    var audioBytes = File.ReadAllBytes(audioFile);
                    var byteContent = new ByteArrayContent(audioBytes);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
                    content.Add(byteContent, "file", Path.GetFileName(audioFile));
                    content.Add(new StringContent("whisper-1"), "model");
                    content.Add(new StringContent("vi"), "language");

                    var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", content);
                    string result = await response.Content.ReadAsStringAsync();

                    var jsonResult = JObject.Parse(result);
                    var textToken = jsonResult["text"];
                    if (textToken != null)
                    {
                        return textToken.ToString();
                    }
                    else
                    {
                        throw new Exception("Không phân tích được text từ Whisper: " + result);
                    }
                }
            }
        }
    }
}