
namespace TopData
{
    partial class FormSpecificFilter
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
            this.ButtonFilter = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ComboBoxFilterType = new System.Windows.Forms.ComboBox();
            this.ComboBoxItemsToFilter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ButtonFilter
            // 
            this.ButtonFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFilter.Location = new System.Drawing.Point(547, 49);
            this.ButtonFilter.Name = "ButtonFilter";
            this.ButtonFilter.Size = new System.Drawing.Size(75, 23);
            this.ButtonFilter.TabIndex = 0;
            this.ButtonFilter.Text = "&OK";
            this.ButtonFilter.UseVisualStyleBackColor = true;
            this.ButtonFilter.Click += new System.EventHandler(this.ButtonFilter_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(466, 49);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 1;
            this.ButtonCancel.Text = "&Annuleren";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ComboBoxFilterType
            // 
            this.ComboBoxFilterType.FormattingEnabled = true;
            this.ComboBoxFilterType.Location = new System.Drawing.Point(12, 12);
            this.ComboBoxFilterType.Name = "ComboBoxFilterType";
            this.ComboBoxFilterType.Size = new System.Drawing.Size(165, 23);
            this.ComboBoxFilterType.TabIndex = 2;
            // 
            // ComboBoxItemsToFilter
            // 
            this.ComboBoxItemsToFilter.FormattingEnabled = true;
            this.ComboBoxItemsToFilter.Location = new System.Drawing.Point(187, 12);
            this.ComboBoxItemsToFilter.Name = "ComboBoxItemsToFilter";
            this.ComboBoxItemsToFilter.Size = new System.Drawing.Size(435, 23);
            this.ComboBoxItemsToFilter.TabIndex = 3;
            this.ComboBoxItemsToFilter.TextChanged += new System.EventHandler(this.ComboBoxItemsToFilter_TextChanged);
            // 
            // FormSpecificFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(634, 84);
            this.Controls.Add(this.ComboBoxItemsToFilter);
            this.Controls.Add(this.ComboBoxFilterType);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSpecificFilter";
            this.Text = "FormSpecificFilter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSpecificFilter_FormClosing);
            this.Load += new System.EventHandler(this.FormSpecificFilter_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonFilter;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.ComboBox ComboBoxFilterType;
        private System.Windows.Forms.ComboBox ComboBoxItemsToFilter;
    }
}