
namespace TopData
{
    partial class FormUserLogin
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonAgain = new System.Windows.Forms.Button();
            this.ButtonLogIn = new System.Windows.Forms.Button();
            this.LabelUserPassword = new System.Windows.Forms.Label();
            this.LabelUserName = new System.Windows.Forms.Label();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.TextBoxUser = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonClose);
            this.groupBox1.Controls.Add(this.ButtonAgain);
            this.groupBox1.Controls.Add(this.ButtonLogIn);
            this.groupBox1.Controls.Add(this.LabelUserPassword);
            this.groupBox1.Controls.Add(this.LabelUserName);
            this.groupBox1.Controls.Add(this.TextBoxPassword);
            this.groupBox1.Controls.Add(this.TextBoxUser);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(279, 119);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 7;
            this.ButtonClose.Text = "&Sluiten";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonAgain
            // 
            this.ButtonAgain.Location = new System.Drawing.Point(9, 119);
            this.ButtonAgain.Name = "ButtonAgain";
            this.ButtonAgain.Size = new System.Drawing.Size(75, 23);
            this.ButtonAgain.TabIndex = 6;
            this.ButtonAgain.Text = "&Opnieuw";
            this.ButtonAgain.UseVisualStyleBackColor = true;
            this.ButtonAgain.Click += new System.EventHandler(this.ButtonAgain_Click);
            // 
            // ButtonLogIn
            // 
            this.ButtonLogIn.Location = new System.Drawing.Point(9, 90);
            this.ButtonLogIn.Name = "ButtonLogIn";
            this.ButtonLogIn.Size = new System.Drawing.Size(345, 23);
            this.ButtonLogIn.TabIndex = 5;
            this.ButtonLogIn.Text = "&Log in";
            this.ButtonLogIn.UseVisualStyleBackColor = true;
            this.ButtonLogIn.Click += new System.EventHandler(this.ButtonLogIn_Click);
            // 
            // LabelUserPassword
            // 
            this.LabelUserPassword.AutoSize = true;
            this.LabelUserPassword.Location = new System.Drawing.Point(6, 54);
            this.LabelUserPassword.Name = "LabelUserPassword";
            this.LabelUserPassword.Size = new System.Drawing.Size(78, 15);
            this.LabelUserPassword.TabIndex = 4;
            this.LabelUserPassword.Text = "Wachtwoord:";
            // 
            // LabelUserName
            // 
            this.LabelUserName.AutoSize = true;
            this.LabelUserName.Location = new System.Drawing.Point(6, 25);
            this.LabelUserName.Name = "LabelUserName";
            this.LabelUserName.Size = new System.Drawing.Size(64, 15);
            this.LabelUserName.TabIndex = 3;
            this.LabelUserName.Text = "Gebruiker :";
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(99, 51);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(255, 23);
            this.TextBoxPassword.TabIndex = 2;
            this.TextBoxPassword.TextChanged += new System.EventHandler(this.TextBoxUser_TextChanged);
            this.TextBoxPassword.Enter += new System.EventHandler(this.TextBoxUser_Enter);
            this.TextBoxPassword.Leave += new System.EventHandler(this.TextBoxUser_Leave);
            // 
            // TextBoxUser
            // 
            this.TextBoxUser.Location = new System.Drawing.Point(99, 22);
            this.TextBoxUser.Name = "TextBoxUser";
            this.TextBoxUser.PlaceholderText = "Uw naam";
            this.TextBoxUser.Size = new System.Drawing.Size(255, 23);
            this.TextBoxUser.TabIndex = 1;
            this.TextBoxUser.TextChanged += new System.EventHandler(this.TextBoxUser_TextChanged);
            this.TextBoxUser.Enter += new System.EventHandler(this.TextBoxUser_Enter);
            this.TextBoxUser.Leave += new System.EventHandler(this.TextBoxUser_Leave);
            // 
            // FormUserLogin
            // 
            this.AcceptButton = this.ButtonLogIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.ButtonClose;
            this.ClientSize = new System.Drawing.Size(384, 167);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormUserLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormUserLogin";
            this.Load += new System.EventHandler(this.FormUserLogin_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LabelUserPassword;
        private System.Windows.Forms.Label LabelUserName;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.TextBox TextBoxUser;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button ButtonAgain;
        private System.Windows.Forms.Button ButtonLogIn;
    }
}