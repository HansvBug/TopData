
namespace TopData
{
    partial class FormMaintainQueryDefaultText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMaintainQueryDefaultText));
            this.RichTextBoxDefaultQueryText = new System.Windows.Forms.RichTextBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonSaveAndClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RichTextBoxDefaultQueryText
            // 
            this.RichTextBoxDefaultQueryText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxDefaultQueryText.Location = new System.Drawing.Point(12, 12);
            this.RichTextBoxDefaultQueryText.Name = "RichTextBoxDefaultQueryText";
            this.RichTextBoxDefaultQueryText.Size = new System.Drawing.Size(278, 303);
            this.RichTextBoxDefaultQueryText.TabIndex = 0;
            this.RichTextBoxDefaultQueryText.Text = resources.GetString("RichTextBoxDefaultQueryText.Text");
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(215, 321);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 1;
            this.ButtonClose.Text = "Sl&uiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSave.Location = new System.Drawing.Point(134, 321);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 23);
            this.ButtonSave.TabIndex = 2;
            this.ButtonSave.Text = "Op&slaan";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonSaveAndClose
            // 
            this.ButtonSaveAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSaveAndClose.Location = new System.Drawing.Point(12, 321);
            this.ButtonSaveAndClose.Name = "ButtonSaveAndClose";
            this.ButtonSaveAndClose.Size = new System.Drawing.Size(116, 23);
            this.ButtonSaveAndClose.TabIndex = 3;
            this.ButtonSaveAndClose.Text = "&Opslaan en sluiten";
            this.ButtonSaveAndClose.UseVisualStyleBackColor = true;
            this.ButtonSaveAndClose.Click += new System.EventHandler(this.ButtonSaveAndClose_Click);
            // 
            // FormMaintainQueryDefaultText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(303, 351);
            this.Controls.Add(this.ButtonSaveAndClose);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.RichTextBoxDefaultQueryText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormMaintainQueryDefaultText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormMaintainQueryDefaultText";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaintainQueryDefaultText_FormClosing);
            this.Load += new System.EventHandler(this.FormMaintainQueryDefaultText_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RichTextBoxDefaultQueryText;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonSaveAndClose;
    }
}