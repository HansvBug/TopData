
namespace TopData
{
    partial class FormFilter
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonFiltertype = new System.Windows.Forms.Button();
            this.TextBoxFilter = new System.Windows.Forms.TextBox();
            this.GroupBoxOperator = new System.Windows.Forms.GroupBox();
            this.RadioButtonOR = new System.Windows.Forms.RadioButton();
            this.RadioButtonAND = new System.Windows.Forms.RadioButton();
            this.ButtonAnnuleer = new System.Windows.Forms.Button();
            this.ButtonFilter = new System.Windows.Forms.Button();
            this.ButtonShowSelected = new System.Windows.Forms.Button();
            this.ButtonSelectNone = new System.Windows.Forms.Button();
            this.ButtonInvertSelection = new System.Windows.Forms.Button();
            this.ButtonSelectAll = new System.Windows.Forms.Button();
            this.TreeViewFilter = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.GroupBoxOperator.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.ButtonFiltertype);
            this.panel1.Controls.Add(this.TextBoxFilter);
            this.panel1.Controls.Add(this.GroupBoxOperator);
            this.panel1.Controls.Add(this.ButtonAnnuleer);
            this.panel1.Controls.Add(this.ButtonFilter);
            this.panel1.Controls.Add(this.ButtonShowSelected);
            this.panel1.Controls.Add(this.ButtonSelectNone);
            this.panel1.Controls.Add(this.ButtonInvertSelection);
            this.panel1.Controls.Add(this.ButtonSelectAll);
            this.panel1.Controls.Add(this.TreeViewFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 423);
            this.panel1.TabIndex = 0;
            // 
            // ButtonFiltertype
            // 
            this.ButtonFiltertype.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonFiltertype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFiltertype.Location = new System.Drawing.Point(3, 3);
            this.ButtonFiltertype.Name = "ButtonFiltertype";
            this.ButtonFiltertype.Size = new System.Drawing.Size(204, 23);
            this.ButtonFiltertype.TabIndex = 9;
            this.ButtonFiltertype.Text = "ButtonFiltertype";
            this.ButtonFiltertype.UseVisualStyleBackColor = true;
            this.ButtonFiltertype.Click += new System.EventHandler(this.ButtonFiltertype_Click);
            // 
            // TextBoxFilter
            // 
            this.TextBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxFilter.Location = new System.Drawing.Point(3, 32);
            this.TextBoxFilter.Name = "TextBoxFilter";
            this.TextBoxFilter.Size = new System.Drawing.Size(204, 23);
            this.TextBoxFilter.TabIndex = 8;
            this.TextBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // GroupBoxOperator
            // 
            this.GroupBoxOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxOperator.Controls.Add(this.RadioButtonOR);
            this.GroupBoxOperator.Controls.Add(this.RadioButtonAND);
            this.GroupBoxOperator.Location = new System.Drawing.Point(3, 260);
            this.GroupBoxOperator.Name = "GroupBoxOperator";
            this.GroupBoxOperator.Size = new System.Drawing.Size(204, 48);
            this.GroupBoxOperator.TabIndex = 7;
            this.GroupBoxOperator.TabStop = false;
            this.GroupBoxOperator.Text = "Operator";
            // 
            // RadioButtonOR
            // 
            this.RadioButtonOR.AutoSize = true;
            this.RadioButtonOR.Location = new System.Drawing.Point(64, 19);
            this.RadioButtonOR.Name = "RadioButtonOR";
            this.RadioButtonOR.Size = new System.Drawing.Size(40, 19);
            this.RadioButtonOR.TabIndex = 1;
            this.RadioButtonOR.TabStop = true;
            this.RadioButtonOR.Text = "OF";
            this.RadioButtonOR.UseVisualStyleBackColor = true;
            // 
            // RadioButtonAND
            // 
            this.RadioButtonAND.AutoSize = true;
            this.RadioButtonAND.Location = new System.Drawing.Point(3, 19);
            this.RadioButtonAND.Name = "RadioButtonAND";
            this.RadioButtonAND.Size = new System.Drawing.Size(40, 19);
            this.RadioButtonAND.TabIndex = 0;
            this.RadioButtonAND.TabStop = true;
            this.RadioButtonAND.Text = "EN";
            this.RadioButtonAND.UseVisualStyleBackColor = true;
            // 
            // ButtonAnnuleer
            // 
            this.ButtonAnnuleer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAnnuleer.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonAnnuleer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonAnnuleer.Location = new System.Drawing.Point(114, 372);
            this.ButtonAnnuleer.Name = "ButtonAnnuleer";
            this.ButtonAnnuleer.Size = new System.Drawing.Size(93, 23);
            this.ButtonAnnuleer.TabIndex = 6;
            this.ButtonAnnuleer.Text = "&Annuleer";
            this.ButtonAnnuleer.UseVisualStyleBackColor = true;
            this.ButtonAnnuleer.Click += new System.EventHandler(this.ButtonAnnuleer_Click);
            // 
            // ButtonFilter
            // 
            this.ButtonFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonFilter.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFilter.Location = new System.Drawing.Point(3, 372);
            this.ButtonFilter.Name = "ButtonFilter";
            this.ButtonFilter.Size = new System.Drawing.Size(93, 23);
            this.ButtonFilter.TabIndex = 5;
            this.ButtonFilter.Text = "&Filter";
            this.ButtonFilter.UseVisualStyleBackColor = true;
            this.ButtonFilter.Click += new System.EventHandler(this.ButtonFilter_Click);
            // 
            // ButtonShowSelected
            // 
            this.ButtonShowSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonShowSelected.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonShowSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonShowSelected.Location = new System.Drawing.Point(114, 343);
            this.ButtonShowSelected.Name = "ButtonShowSelected";
            this.ButtonShowSelected.Size = new System.Drawing.Size(93, 23);
            this.ButtonShowSelected.TabIndex = 4;
            this.ButtonShowSelected.Text = "&Geselecteerd";
            this.ButtonShowSelected.UseVisualStyleBackColor = true;
            this.ButtonShowSelected.Click += new System.EventHandler(this.ButtonShowSelected_Click);
            // 
            // ButtonSelectNone
            // 
            this.ButtonSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectNone.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonSelectNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSelectNone.Location = new System.Drawing.Point(3, 343);
            this.ButtonSelectNone.Name = "ButtonSelectNone";
            this.ButtonSelectNone.Size = new System.Drawing.Size(93, 23);
            this.ButtonSelectNone.TabIndex = 3;
            this.ButtonSelectNone.Text = "Selecteer &geen";
            this.ButtonSelectNone.UseVisualStyleBackColor = true;
            this.ButtonSelectNone.Click += new System.EventHandler(this.ButtonSelectNone_Click);
            // 
            // ButtonInvertSelection
            // 
            this.ButtonInvertSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonInvertSelection.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonInvertSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonInvertSelection.Location = new System.Drawing.Point(114, 314);
            this.ButtonInvertSelection.Name = "ButtonInvertSelection";
            this.ButtonInvertSelection.Size = new System.Drawing.Size(93, 23);
            this.ButtonInvertSelection.TabIndex = 2;
            this.ButtonInvertSelection.Text = "&Invert";
            this.ButtonInvertSelection.UseVisualStyleBackColor = true;
            this.ButtonInvertSelection.Click += new System.EventHandler(this.ButtonInvertSelection_Click);
            // 
            // ButtonSelectAll
            // 
            this.ButtonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectAll.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSelectAll.Location = new System.Drawing.Point(3, 314);
            this.ButtonSelectAll.Name = "ButtonSelectAll";
            this.ButtonSelectAll.Size = new System.Drawing.Size(93, 23);
            this.ButtonSelectAll.TabIndex = 1;
            this.ButtonSelectAll.Text = "Selecteer &alles";
            this.ButtonSelectAll.UseVisualStyleBackColor = true;
            this.ButtonSelectAll.Click += new System.EventHandler(this.ButtonSelectAll_Click);
            // 
            // TreeViewFilter
            // 
            this.TreeViewFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewFilter.CheckBoxes = true;
            this.TreeViewFilter.HotTracking = true;
            this.TreeViewFilter.Location = new System.Drawing.Point(3, 61);
            this.TreeViewFilter.Name = "TreeViewFilter";
            this.TreeViewFilter.Size = new System.Drawing.Size(204, 193);
            this.TreeViewFilter.TabIndex = 0;
            this.TreeViewFilter.Click += new System.EventHandler(this.TreeViewFilter_Click);
            this.TreeViewFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TreeViewFilter_KeyPress);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(2, 403);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(210, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.ContextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenuStrip1_ItemClicked);
            // 
            // FormFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(214, 427);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormFilter";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.Text = "FormFilter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFilter_FormClosing);
            this.Load += new System.EventHandler(this.FormFilter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.GroupBoxOperator.ResumeLayout(false);
            this.GroupBoxOperator.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView TreeViewFilter;
        public System.Windows.Forms.Button ButtonInvertSelection;
        public System.Windows.Forms.Button ButtonSelectAll;
        private System.Windows.Forms.GroupBox GroupBoxOperator;
        private System.Windows.Forms.RadioButton RadioButtonOR;
        private System.Windows.Forms.RadioButton RadioButtonAND;
        private System.Windows.Forms.Button ButtonAnnuleer;
        private System.Windows.Forms.Button ButtonFilter;
        private System.Windows.Forms.Button ButtonShowSelected;
        private System.Windows.Forms.Button ButtonSelectNone;
        private System.Windows.Forms.Button ButtonFiltertype;
        private System.Windows.Forms.TextBox TextBoxFilter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
    }
}