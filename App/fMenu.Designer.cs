using System.Windows.Forms;

namespace App
{
    partial class fMenu
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flpMenuContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.dtgvIngredient = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvIngredient)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1037, 547);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flpMenuContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dtgvIngredient);
            this.splitContainer1.Size = new System.Drawing.Size(1037, 547);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 0;
            // 
            // flpMenuContainer
            // 
            this.flpMenuContainer.AutoScroll = true;
            this.flpMenuContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMenuContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenuContainer.Location = new System.Drawing.Point(0, 0);
            this.flpMenuContainer.Name = "flpMenuContainer";
            this.flpMenuContainer.Size = new System.Drawing.Size(420, 547);
            this.flpMenuContainer.TabIndex = 0;
            this.flpMenuContainer.WrapContents = false;
            this.flpMenuContainer.Resize += new System.EventHandler(this.flpMenuContainer_Resize);
            // 
            // dtgvIngredient
            // 
            this.dtgvIngredient.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dtgvIngredient.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtgvIngredient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvIngredient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvIngredient.Location = new System.Drawing.Point(0, 0);
            this.dtgvIngredient.Name = "dtgvIngredient";
            this.dtgvIngredient.RowHeadersWidth = 51;
            this.dtgvIngredient.RowTemplate.Height = 24;
            this.dtgvIngredient.Size = new System.Drawing.Size(613, 547);
            this.dtgvIngredient.TabIndex = 1;
            // 
            // fMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 571);
            this.Controls.Add(this.panel1);
            this.Name = "fMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thực Đơn";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvIngredient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dtgvIngredient;
        private System.Windows.Forms.FlowLayoutPanel flpMenuContainer;
    }
}