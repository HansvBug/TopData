namespace TopData
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// Treeview extension class.
    /// Extension class is alway static.
    /// </summary>
    public static class TdTreeViewExtensions
    {
        /// <summary>
        /// Get the expansion state from all treeview nodes.
        /// </summary>
        /// <param name="nodes">The treenode collection of the selected treeview.</param>
        /// <returns>List with treenodes.</returns>
        public static List<string> GetExpansionState(this TreeNodeCollection nodes)
        {
            return nodes.Descendants()
                        .Where(n => n.IsExpanded)
                        .Select(n => n.FullPath)
                        .ToList();
        }

        /// <summary>
        /// Save the expansion state from all treeview nodes.
        /// </summary>
        /// <param name="nodes">The treenode collection of the selected treeview.</param>
        /// <param name="savedExpansionState">The saved treenode collection of the selected treeview.</param>
        public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants().Where(n => savedExpansionState.Contains(n.FullPath)))
            {
                node.Expand();
            }
        }

        private static void SaveTrvState(List<string> savedExpansionState, string userName, int userId, string databaseFileName, string treeViewName, int connectionId)
        {
            if (savedExpansionState == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(userName) &&
                    userId > 0 &&
                    !string.IsNullOrEmpty(databaseFileName))
            {
                using TdMaintainTreeviewState saveState = new (userName, userId, connectionId);
                saveState.DeleteTreeViewState(treeViewName);  // Delete the old treeview state settings

                foreach (string trv_node_state in savedExpansionState)
                {
                    saveState.SaveTreeViewState(treeViewName, trv_node_state);
                }
            }
            else
            {
                TdLogging.WriteToLogInformation("De treeview state voor '" + treeViewName + "' is niet opgeslagen.");
                TdLogging.WriteToLogInformation("UserName = " + userName);
                TdLogging.WriteToLogInformation("User_id = " + Convert.ToString(userId, CultureInfo.InvariantCulture));
                TdLogging.WriteToLogInformation("DatabaseFileName : " + databaseFileName);
                TdLogging.WriteToLogInformation("ConnectionId = " + Convert.ToString(connectionId, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Save the treeview nodes expansion state.
        /// </summary>
        /// <param name="savedExpansionState">A list with the expansion states of all treeview nodes per user per database connection.</param>
        /// <param name="userName">The name of the current user.</param>
        /// <param name="userId">Id of the currrent user.</param>
        /// <param name="treeViewName">Name of the selcted treeview.</param>
        /// <param name="connectionId">Current databse connection id.</param>
        public static void SaveTreeviewState(List<string> savedExpansionState, string userName, int userId, string treeViewName, int connectionId)
        {
            if (savedExpansionState == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(userName) &&
                !string.IsNullOrEmpty(Convert.ToString(userId, CultureInfo.InvariantCulture)) &&
                !string.IsNullOrEmpty(treeViewName))
            {
                using TdMaintainTreeviewState saveState = new (userName, userId, connectionId);
                saveState.DeleteTreeViewState(treeViewName);

                foreach (string trv_node_state in savedExpansionState)
                {
                    saveState.SaveTreeViewState(treeViewName, trv_node_state);
                }
            }
            else
            {
                TdLogging.WriteToLogError("De treeview state is niet opgeslagen. Betreft treeview : " + treeViewName);
            }
        }

        /// <summary>
        /// Delete the treeviw state records in the settngs table.
        /// </summary>
        /// <param name="userName">Current user.</param>
        /// <param name="userId">Current user id.</param>
        /// <param name="treeViewName">Selected treeview.</param>
        /// <param name="connectionId">Current database connection id.</param>
        public static void DeleteTreeViewStateReset(string userName, int userId, string treeViewName, int connectionId)
        {
            if (!string.IsNullOrEmpty(userName) &&
                    userId > 0 &&
                    !string.IsNullOrEmpty(treeViewName))
            {
                using TdMaintainTreeviewState deleteState = new (userName, userId, connectionId);
                deleteState.DeleteTreeViewState(treeViewName);
            }
            else
            {
                TdLogging.WriteToLogError("De treeview state gegevens zijn verwijderd. Betreft treeview : " + treeViewName);
            }
        }

        private static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Get the treeview state from the settings table.
        /// </summary>
        /// <param name="trv">The treeview.</param>
        /// <param name="userName">The current user.</param>
        /// <param name="userId">Current user id.</param>
        /// <param name="connectionId">Current connection id.</param>
        public static void ReadtreeviewState(TreeView trv, string userName, int userId, int connectionId)
        {
            if (trv == null)
            {
                return;
            }

            if (trv != null &&
                    !string.IsNullOrEmpty(userName) &&
                    userId > 0)
            {
                List<string> savedExpansionState = new ();

                using TdMaintainTreeviewState listTreeviewNodes = new (userName, userId, connectionId);
                listTreeviewNodes.ReadTreeViewState(trv);
                savedExpansionState = listTreeviewNodes.TreeviewNodes;

                trv.Nodes.SetExpansionState(savedExpansionState);
            }
            else
            {
                TdLogging.WriteToLogError("De treeview state gegevens kunnen niet worden ingelezen. Betreft treeview : " + trv.Name);
            }
        }
    }
}
