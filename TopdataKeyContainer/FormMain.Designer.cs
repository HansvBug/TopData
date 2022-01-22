namespace TopdataKeyContainer
{
    partial class MainFrm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonCreateOrRetrieveKeyContainer = new System.Windows.Forms.Button();
            this.TextBoxNameKeyContainer = new System.Windows.Forms.TextBox();
            this.LabelKeyContainer = new System.Windows.Forms.Label();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonGetKey = new System.Windows.Forms.Button();
            this.ButtonSaveToXml = new System.Windows.Forms.Button();
            this.RichTextBoxKey = new System.Windows.Forms.RichTextBox();
            this.TextBoxSaved = new System.Windows.Forms.TextBox();
            this.ButtonSaveKeyToCurrentMachine = new System.Windows.Forms.Button();
            this.ButtonDeltekeyContainer = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ButtonCreateOrRetrieveKeyContainer
            // 
            this.ButtonCreateOrRetrieveKeyContainer.Location = new System.Drawing.Point(330, 21);
            this.ButtonCreateOrRetrieveKeyContainer.Name = "ButtonCreateOrRetrieveKeyContainer";
            this.ButtonCreateOrRetrieveKeyContainer.Size = new System.Drawing.Size(169, 23);
            this.ButtonCreateOrRetrieveKeyContainer.TabIndex = 2;
            this.ButtonCreateOrRetrieveKeyContainer.Text = "Create or Get Key contanier";
            this.ButtonCreateOrRetrieveKeyContainer.UseVisualStyleBackColor = true;
            this.ButtonCreateOrRetrieveKeyContainer.Click += new System.EventHandler(this.ButtonCreateOrRetrieveKeyContainer_Click);
            // 
            // TextBoxNameKeyContainer
            // 
            this.TextBoxNameKeyContainer.Location = new System.Drawing.Point(143, 21);
            this.TextBoxNameKeyContainer.Name = "TextBoxNameKeyContainer";
            this.TextBoxNameKeyContainer.Size = new System.Drawing.Size(181, 23);
            this.TextBoxNameKeyContainer.TabIndex = 3;
            // 
            // LabelKeyContainer
            // 
            this.LabelKeyContainer.AutoSize = true;
            this.LabelKeyContainer.Location = new System.Drawing.Point(12, 24);
            this.LabelKeyContainer.Name = "LabelKeyContainer";
            this.LabelKeyContainer.Size = new System.Drawing.Size(116, 15);
            this.LabelKeyContainer.TabIndex = 4;
            this.LabelKeyContainer.Text = "Name key container:";
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(713, 415);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 9;
            this.ButtonClose.Text = "Close";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonGetKey
            // 
            this.ButtonGetKey.Location = new System.Drawing.Point(12, 49);
            this.ButtonGetKey.Name = "ButtonGetKey";
            this.ButtonGetKey.Size = new System.Drawing.Size(116, 23);
            this.ButtonGetKey.TabIndex = 10;
            this.ButtonGetKey.Text = "Get key";
            this.ButtonGetKey.UseVisualStyleBackColor = true;
            this.ButtonGetKey.Click += new System.EventHandler(this.ButtonGetKey_Click);
            // 
            // ButtonSaveToXml
            // 
            this.ButtonSaveToXml.Location = new System.Drawing.Point(12, 198);
            this.ButtonSaveToXml.Name = "ButtonSaveToXml";
            this.ButtonSaveToXml.Size = new System.Drawing.Size(116, 23);
            this.ButtonSaveToXml.TabIndex = 11;
            this.ButtonSaveToXml.Text = "Save key as xml file";
            this.ButtonSaveToXml.UseVisualStyleBackColor = true;
            this.ButtonSaveToXml.Click += new System.EventHandler(this.ButtonSaveToXml_Click);
            // 
            // RichTextBoxKey
            // 
            this.RichTextBoxKey.Location = new System.Drawing.Point(143, 50);
            this.RichTextBoxKey.Name = "RichTextBoxKey";
            this.RichTextBoxKey.Size = new System.Drawing.Size(645, 104);
            this.RichTextBoxKey.TabIndex = 12;
            this.RichTextBoxKey.Text = "";
            // 
            // TextBoxSaved
            // 
            this.TextBoxSaved.Location = new System.Drawing.Point(143, 198);
            this.TextBoxSaved.Name = "TextBoxSaved";
            this.TextBoxSaved.Size = new System.Drawing.Size(645, 23);
            this.TextBoxSaved.TabIndex = 13;
            // 
            // ButtonSaveKeyToCurrentMachine
            // 
            this.ButtonSaveKeyToCurrentMachine.Location = new System.Drawing.Point(12, 251);
            this.ButtonSaveKeyToCurrentMachine.Name = "ButtonSaveKeyToCurrentMachine";
            this.ButtonSaveKeyToCurrentMachine.Size = new System.Drawing.Size(185, 23);
            this.ButtonSaveKeyToCurrentMachine.TabIndex = 14;
            this.ButtonSaveKeyToCurrentMachine.Text = "Copy the XML to the machine keycontainer";
            this.ButtonSaveKeyToCurrentMachine.UseVisualStyleBackColor = true;
            this.ButtonSaveKeyToCurrentMachine.Click += new System.EventHandler(this.ButtonSaveKeyToCurrentMachine_Click);
            // 
            // ButtonDeltekeyContainer
            // 
            this.ButtonDeltekeyContainer.Location = new System.Drawing.Point(12, 341);
            this.ButtonDeltekeyContainer.Name = "ButtonDeltekeyContainer";
            this.ButtonDeltekeyContainer.Size = new System.Drawing.Size(185, 23);
            this.ButtonDeltekeyContainer.TabIndex = 15;
            this.ButtonDeltekeyContainer.Text = "Delete key container";
            this.ButtonDeltekeyContainer.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(242, 251);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(445, 187);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = "TE DOEN\n\nkijk of een key bestaat:\nhttps://www.codeproject.com/Tips/887978/Create-" +
    "RSA-Key-Container-and-Encrypt-Decrypt-Confi\n\n\ndelete a key container... hoe";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.ButtonDeltekeyContainer);
            this.Controls.Add(this.ButtonSaveKeyToCurrentMachine);
            this.Controls.Add(this.TextBoxSaved);
            this.Controls.Add(this.RichTextBoxKey);
            this.Controls.Add(this.ButtonSaveToXml);
            this.Controls.Add(this.ButtonGetKey);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.LabelKeyContainer);
            this.Controls.Add(this.TextBoxNameKeyContainer);
            this.Controls.Add(this.ButtonCreateOrRetrieveKeyContainer);
            this.Name = "MainFrm";
            this.Text = "MainFrm";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button ButtonCreateOrRetrieveKeyContainer;
        private TextBox TextBoxNameKeyContainer;
        private Label LabelKeyContainer;
        private Button ButtonClose;
        private Button ButtonGetKey;
        private Button ButtonSaveToXml;
        private RichTextBox RichTextBoxKey;
        private TextBox TextBoxSaved;
        private Button ButtonSaveKeyToCurrentMachine;
        private Button ButtonDeltekeyContainer;
        private RichTextBox richTextBox1;
    }
}