namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Enable Treeview search.
    /// </summary>
    public class TdTreeViewSearch
    {
        // TODO; Can not be disposible otherwise find next does not work  --> Check, the code is changed...
        #region Properties

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; }

        private readonly List<TreeNode> currentNodeMatches = new ();
        private int lastNodeIndex;
        private string lastSearchText;

        /// <summary>
        /// Gets or sets the number of found treeview nodes.
        /// </summary>
        public int FoundTrvNodeSearch { get; set; }

        /// <summary>
        /// gets or sets the number of the found treeview texts.
        /// </summary>
        public int FoundWithTrvSearch { get; set; } // Treeview search

        #endregion Properties

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TdTreeViewSearch"/> class.
        /// Load the settings.
        /// </summary>
        public TdTreeViewSearch()
        {
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            using TdSettingsManager set = new ();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }
        #endregion constructor

        private int FoundTrvSearch { get => this.FoundWithTrvSearch; set => this.FoundWithTrvSearch = value; }

        /// <summary>
        /// Give the treeview node a different color.
        /// </summary>
        /// <param name="treeNode">The treeviewnode which is searched for.</param>
        /// <param name="tb">The textbox with the search text.</param>
        public void ColorTrvSearchNode(TreeNode treeNode, TextBox tb)
        {
            if (treeNode == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(tb.Text))
            {
                if (treeNode.Text.ToUpperInvariant().Contains(tb.Text.ToUpperInvariant()))
                {
                    treeNode.BackColor = Color.LightBlue;
                    this.FoundTrvSearch++;
                }
                else
                {
                    treeNode.BackColor = Color.White;
                }

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    this.ColorTrvSearchNode(tn, tb);
                }
            }
            else
            {
                treeNode.BackColor = Color.White;
                this.FoundTrvSearch = 0;

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    this.ColorTrvSearchNode(tn, tb);
                }
            }
        }

        /// <summary>
        /// Search a text in the treenodes names.
        /// </summary>
        /// <param name="trv">The treeview.</param>
        /// <param name="searchText">The search text.</param>
        public void SearchInTreeViewNodes(TreeView trv, string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || trv == null)
            {
                return;
            }

            if (this.lastSearchText != searchText)
            {
                if (trv.Nodes.Count > 0)
                {
                    // It's a new Search
                    try
                    {
                        this.currentNodeMatches.Clear();
                        this.lastSearchText = searchText;
                        this.lastNodeIndex = 0;
                        this.SearchNodes(searchText, trv.Nodes[0]);

                        TreeNode selectedNode = this.currentNodeMatches[this.lastNodeIndex];

                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        this.lastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives an exception
                    }
                }
            }
            else
            {
                if (this.lastNodeIndex >= 0 && this.currentNodeMatches.Count > 0 && this.lastNodeIndex < this.currentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = this.currentNodeMatches[this.lastNodeIndex];
                        this.lastNodeIndex++;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives anexception
                    }
                }
                else if (this.lastNodeIndex == this.currentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = this.currentNodeMatches[0];
                        this.lastNodeIndex = 0;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        this.lastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives anexception
                    }
                }
                else
                {
                    // It's a new Search
                    this.currentNodeMatches.Clear();
                    this.lastSearchText = searchText;
                    this.lastNodeIndex = 0;
                    this.SearchNodes(searchText, trv.Nodes[0]);
                }
            }
        }

        private void SearchNodes(string searchText, TreeNode startNode)
        {
            while (startNode != null)
            {
                if (startNode.Text.ToUpperInvariant().Contains(searchText.ToUpperInvariant()))
                {
                    this.currentNodeMatches.Add(startNode);
                }

                if (startNode.Nodes.Count != 0)
                {
                    this.SearchNodes(searchText, startNode.Nodes[0]); // Recursive Search
                }

                startNode = startNode.NextNode;
            }
        }
    }
}
