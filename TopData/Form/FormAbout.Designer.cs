
namespace TopData
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelCompany = new System.Windows.Forms.Label();
            this.LabelBuilddate = new System.Windows.Forms.Label();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.LabelAppName = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.LabelCompany);
            this.panel1.Controls.Add(this.LabelBuilddate);
            this.panel1.Controls.Add(this.LabelVersion);
            this.panel1.Controls.Add(this.LabelAppName);
            this.panel1.Controls.Add(this.PictureBox1);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(837, 362);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // LabelCompany
            // 
            this.LabelCompany.AutoSize = true;
            this.LabelCompany.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelCompany.Location = new System.Drawing.Point(550, 333);
            this.LabelCompany.Name = "LabelCompany";
            this.LabelCompany.Size = new System.Drawing.Size(50, 19);
            this.LabelCompany.TabIndex = 4;
            this.LabelCompany.Text = "HvB ©";
            this.LabelCompany.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // LabelBuilddate
            // 
            this.LabelBuilddate.AutoSize = true;
            this.LabelBuilddate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelBuilddate.Location = new System.Drawing.Point(550, 314);
            this.LabelBuilddate.Name = "LabelBuilddate";
            this.LabelBuilddate.Size = new System.Drawing.Size(81, 19);
            this.LabelBuilddate.TabIndex = 3;
            this.LabelBuilddate.Text = "Build date : ";
            this.LabelBuilddate.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // LabelVersion
            // 
            this.LabelVersion.AutoSize = true;
            this.LabelVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LabelVersion.Location = new System.Drawing.Point(550, 61);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(74, 19);
            this.LabelVersion.TabIndex = 2;
            this.LabelVersion.Text = "Version : 0";
            this.LabelVersion.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // LabelAppName
            // 
            this.LabelAppName.AutoSize = true;
            this.LabelAppName.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelAppName.Location = new System.Drawing.Point(550, 16);
            this.LabelAppName.Name = "LabelAppName";
            this.LabelAppName.Size = new System.Drawing.Size(110, 45);
            this.LabelAppName.TabIndex = 1;
            this.LabelAppName.Text = "label1";
            this.LabelAppName.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.InitialImage = null;
            this.PictureBox1.Location = new System.Drawing.Point(10, 10);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(577, 342);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            this.PictureBox1.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(841, 366);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormAbout";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.Click += new System.EventHandler(this.FormAbout_Click);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.Label LabelCompany;
        private System.Windows.Forms.Label LabelAppName;
        internal System.Windows.Forms.Label LabelVersion;
        internal System.Windows.Forms.Label LabelBuilddate;
    }
}