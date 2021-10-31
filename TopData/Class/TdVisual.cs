namespace TopData
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Change visual effects of components.
    /// </summary>
    public static class TdVisual
    {
        /// <summary>
        /// Gets or sets a value indicating whether an active textbox or combobox will get a diffentent background color.
        /// </summary>
        public static bool ActivateHighlightEntryBoxes { get; set; }

        /// <summary>
        /// Gets or sets the cel highlight style of a datagridview.
        /// </summary>
        public static DataGridViewCellStyle HighlightStyle { get; set; }

        private static int highlightedRowIndex = -1;

        /// <summary>
        /// Gets or sets the highlighted rowindex of a datagridview where the mouse hoovers over.
        /// </summary>
        public static int HighlightedRowIndex
        {
            get
            {
                return highlightedRowIndex;
            }

            set
            {
                highlightedRowIndex = value;
            }
        }

        /// <summary>
        /// Set the back color of a textbox on enter.
        /// </summary>
        /// <param name="sender">The textbox.</param>
        /// <param name="e">Sender arguments.</param>
        public static void TxtEnter(object sender, EventArgs e)
        {
            if (ActivateHighlightEntryBoxes)
            {
                if (sender is TextBox tb)
                {
                    tb.BackColor = Color.AliceBlue;
                }
                else if (sender is ComboBox cb)
                {
                    cb.BackColor = Color.AliceBlue;
                }
            }
        }

        /// <summary>
        /// Set the back color of a textbox on leave.
        /// </summary>
        /// <param name="sender">The textbox.</param>
        /// <param name="e">Sender arguments.</param>
        public static void TxtLeave(object sender, EventArgs e)
        {
            if (ActivateHighlightEntryBoxes)
            {
                if (sender is TextBox tb)
                {
                    tb.BackColor = Color.White;
                }
                else if (sender is ComboBox cb)
                {
                    cb.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Check if the entry string is not to large.
        /// </summary>
        /// <param name="sender">The text component.</param>
        /// <param name="length">The maximum length allowed for the text string.</param>
        public static void TxtLengthTolarge(object sender, int length)
        {
            if (sender is TextBox tb)
            {
                Color curColor = tb.BackColor;
                if (tb.Text.Length > length)
                {
                    tb.BackColor = Color.Tomato;
                }
                else
                {
                    tb.BackColor = curColor;
                }
            }
        }

        /// <summary>
        /// Draws 3 dots on the splitcontainer.
        /// </summary>
        /// <param name="sender">The split container.</param>
        /// <param name="e">Sender arguments.</param>
        public static void Splitcontainerhandle(this SplitContainer sender, PaintEventArgs e)
        {
            if (e == null || sender == null)
            {
                return;
            }

            // Paint the three dots'
            if (sender is SplitContainer control)
            {
                Point[] points = new Point[3];
                var w = control.Width;
                var h = control.Height;
                var d = control.SplitterDistance;
                var sW = control.SplitterWidth;

                // Calculate the position of the points
                if (control.Orientation == Orientation.Horizontal)
                {
                    points[0] = new Point(w / 2, d + (sW / 2));
                    points[1] = new Point(points[0].X - 4, points[0].Y);
                    points[2] = new Point(points[0].X + 4, points[0].Y);
                }
                else
                {
                    points[0] = new Point(d + (sW / 2), h / 2);
                    points[1] = new Point(points[0].X, points[0].Y - 4);
                    points[2] = new Point(points[0].X, points[0].Y + 4);
                }

                foreach (Point p in points)
                {
                    p.Offset(-2, -2);
                    e.Graphics.FillEllipse(SystemBrushes.ControlDark, new Rectangle(p, new Size(3, 3)));

                    p.Offset(1, 1);
                    e.Graphics.FillEllipse(SystemBrushes.ControlLight, new Rectangle(p, new Size(3, 3)));
                }
            }
        }

        #region Highlight datagrid row

        /// <summary>
        /// Set the datagridview highlight style.
        /// </summary>
        public static void DataGridHighlightStyle()
        {
            HighlightStyle = new DataGridViewCellStyle
            {
                // this.highlightStyle.ForeColor = Color.DarkBlue;
                BackColor = Color.AliceBlue,  // TODO; make optional.
            };

            // this.highlightStyle.Font = new Font(this.DataGridViewUsers.Font, FontStyle.Bold);
        }

        /// <summary>
        /// Datagridview Cell Mouse enter event handler.
        /// </summary>
        /// <param name="sender">The sender which should be a datagridview.</param>
        /// <param name="e">Datagridview cell event argument.</param>
        public static void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is DataGridView dgv)
            {
                if (e.RowIndex == HighlightedRowIndex)
                {
                    return;
                }

                // Unhighlight the previously highlighted row.
                if (HighlightedRowIndex >= 0)
                {
                    SetRowStyle(dgv.Rows[HighlightedRowIndex], null);
                }

                // Highlight the row holding the mouse.
                HighlightedRowIndex = e.RowIndex;
                if (HighlightedRowIndex >= 0)
                {
                    SetRowStyle(
                        dgv.Rows[HighlightedRowIndex],
                        HighlightStyle);
                }
            }
        }

        private static void SetRowStyle(DataGridViewRow row, DataGridViewCellStyle style)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style = style;
            }
        }

        /// <summary>
        /// Datagridview Cell Mouse leave event handler.
        /// </summary>
        /// <param name="sender">The sender which should be a datagridview.</param>
        /// <param name="e">Datagridview cell event argument.</param>
        public static void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is DataGridView dgv)
            {
                if (HighlightedRowIndex >= 0)
                {
                    SetRowStyle(dgv.Rows[HighlightedRowIndex], null);
                    HighlightedRowIndex = -1;
                }
            }
        }
        #endregion Highlight datagrid row
    }
}
