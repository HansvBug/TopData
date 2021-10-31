
namespace TopData
{
    partial class FormQueryParameter
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
            this.LabelParameter = new System.Windows.Forms.Label();
            this.TextBoxParameter = new System.Windows.Forms.TextBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelParameter
            // 
            this.LabelParameter.AutoSize = true;
            this.LabelParameter.Location = new System.Drawing.Point(12, 9);
            this.LabelParameter.Name = "LabelParameter";
            this.LabelParameter.Size = new System.Drawing.Size(64, 15);
            this.LabelParameter.TabIndex = 0;
            this.LabelParameter.Text = "Parameter:";
            // 
            // TextBoxParameter
            // 
            this.TextBoxParameter.Location = new System.Drawing.Point(12, 27);
            this.TextBoxParameter.Name = "TextBoxParameter";
            this.TextBoxParameter.Size = new System.Drawing.Size(332, 23);
            this.TextBoxParameter.TabIndex = 1;
            this.TextBoxParameter.TextChanged += new System.EventHandler(this.TextBoxParameter_TextChanged);
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(269, 56);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ButtonOk.TabIndex = 2;
            this.ButtonOk.Text = "&OK";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // FormQueryParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 91);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.TextBoxParameter);
            this.Controls.Add(this.LabelParameter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormQueryParameter";
            this.Text = "FormQueryParameter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQueryParameter_FormClosing);
            this.Load += new System.EventHandler(this.FormQueryParameter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelParameter;
        private System.Windows.Forms.TextBox TextBoxParameter;
        private System.Windows.Forms.Button ButtonOk;
    }
}