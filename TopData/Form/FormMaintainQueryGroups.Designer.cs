
namespace TopData
{
    partial class FormMaintainQueryGroups
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
            this.ButtonClose = new System.Windows.Forms.Button();
            this.DataGridViewQgroups = new System.Windows.Forms.DataGridView();
            this.ButtonNewQueryGroup = new System.Windows.Forms.Button();
            this.ButtonSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewQgroups)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Location = new System.Drawing.Point(713, 318);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.Text = "C&lose";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // DataGridViewQgroups
            // 
            this.DataGridViewQgroups.AllowUserToAddRows = false;
            this.DataGridViewQgroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridViewQgroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DataGridViewQgroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewQgroups.Location = new System.Drawing.Point(12, 12);
            this.DataGridViewQgroups.Name = "DataGridViewQgroups";
            this.DataGridViewQgroups.RowTemplate.Height = 25;
            this.DataGridViewQgroups.Size = new System.Drawing.Size(776, 269);
            this.DataGridViewQgroups.TabIndex = 1;
            this.DataGridViewQgroups.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewQgroups_CellMouseDown);
            this.DataGridViewQgroups.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewQgroups_CellValidated);
            this.DataGridViewQgroups.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridViewQgroups_DataError);
            // 
            // ButtonNewQueryGroup
            // 
            this.ButtonNewQueryGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonNewQueryGroup.Location = new System.Drawing.Point(12, 287);
            this.ButtonNewQueryGroup.Name = "ButtonNewQueryGroup";
            this.ButtonNewQueryGroup.Size = new System.Drawing.Size(75, 23);
            this.ButtonNewQueryGroup.TabIndex = 2;
            this.ButtonNewQueryGroup.Text = "&New";
            this.ButtonNewQueryGroup.UseVisualStyleBackColor = true;
            this.ButtonNewQueryGroup.Click += new System.EventHandler(this.ButtonNewQueryGroup_Click);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSave.Location = new System.Drawing.Point(713, 287);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 23);
            this.ButtonSave.TabIndex = 5;
            this.ButtonSave.Text = "&Save";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // FormMaintainQueryGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 353);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.ButtonNewQueryGroup);
            this.Controls.Add(this.DataGridViewQgroups);
            this.Controls.Add(this.ButtonClose);
            this.Name = "FormMaintainQueryGroups";
            this.Text = "FormMaintainQueryGroups";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMaintainQueryGroups_FormClosing);
            this.Load += new System.EventHandler(this.FormMaintainQueryGroups_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewQgroups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView DataGridViewQgroups;
        public System.Windows.Forms.Button ButtonClose;
        public System.Windows.Forms.Button ButtonNewQueryGroup;
        public System.Windows.Forms.Button ButtonSave;
    }
}