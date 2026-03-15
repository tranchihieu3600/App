namespace App
{
    partial class fChatPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbChatHistory = new System.Windows.Forms.RichTextBox();
            this.txtChatInput = new System.Windows.Forms.TextBox();
            this.btnSendChat = new System.Windows.Forms.Button();
            this.btnVoice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbChatHistory
            // 
            this.rtbChatHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbChatHistory.Location = new System.Drawing.Point(12, 12);
            this.rtbChatHistory.Name = "rtbChatHistory";
            this.rtbChatHistory.ReadOnly = true;
            this.rtbChatHistory.Size = new System.Drawing.Size(1095, 522);
            this.rtbChatHistory.TabIndex = 1;
            this.rtbChatHistory.Text = "";
            // 
            // txtChatInput
            // 
            this.txtChatInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChatInput.Location = new System.Drawing.Point(12, 562);
            this.txtChatInput.Name = "txtChatInput";
            this.txtChatInput.Size = new System.Drawing.Size(882, 49);
            this.txtChatInput.TabIndex = 0;
            // 
            // btnSendChat
            // 
            this.btnSendChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendChat.Location = new System.Drawing.Point(900, 562);
            this.btnSendChat.Name = "btnSendChat";
            this.btnSendChat.Size = new System.Drawing.Size(77, 49);
            this.btnSendChat.TabIndex = 3;
            this.btnSendChat.Text = "Gửi";
            this.btnSendChat.UseVisualStyleBackColor = true;
            // 
            // btnVoice
            // 
            this.btnVoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoice.Location = new System.Drawing.Point(983, 560);
            this.btnVoice.Name = "btnVoice";
            this.btnVoice.Size = new System.Drawing.Size(124, 51);
            this.btnVoice.TabIndex = 2;
            this.btnVoice.Text = "Ghi âm";
            this.btnVoice.UseVisualStyleBackColor = true;
            // 
            // fChatPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 639);
            this.Controls.Add(this.btnVoice);
            this.Controls.Add(this.btnSendChat);
            this.Controls.Add(this.txtChatInput);
            this.Controls.Add(this.rtbChatHistory);
            this.Name = "fChatPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gọi món bằng văn bản - lời nói";
            this.Load += new System.EventHandler(this.fChatPopup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbChatHistory;
        private System.Windows.Forms.TextBox txtChatInput;
        private System.Windows.Forms.Button btnSendChat;
        private System.Windows.Forms.Button btnVoice;
    }
}