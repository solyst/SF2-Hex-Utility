using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public partial class EditHexLocationsList : Form
    {
        public HexLocations HexLocations { get; set; }

        private readonly bool hexLocationsDirtyFlag = false;
        private List<HexLocation> addedHexLocations = new List<HexLocation>();
        private List<HexLocationEdit> editStack = new List<HexLocationEdit>();
        public Rom unchangedRom = null;

        public EditHexLocationsList(HexLocations hexLocations, Rom unchangedRom)
        {
            this.HexLocations = hexLocations;
            this.unchangedRom = unchangedRom;
            hexLocationsDirtyFlag = hexLocations.Dirty;
            InitializeComponent();
            refreshTreeView();
        }


        //------------------------------------------------------------
        // Tree View Node Helper Functions
        //------------------------------------------------------------
        #region TreeViewNodeHelper_Functions

        private int nodeLevel(TreeNode treeNode)
        {
            return treeNode.FullPath.Count(x => x == '\\');
        }


        /// <summary>
        /// Return the node corresponding to the given text. Does not search child nodes.
        /// </summary>
        /// <param name="nodes">Nodes to search for text.</param>
        /// <param name="text">Text to find.</param>
        /// <returns>The matching node, or NULL if no match is found.</returns>
        private TreeNode findNodeText(TreeNodeCollection nodes, string text)
        {
            TreeNode returnNode = null;
            foreach (TreeNode n in nodes)
            {
                if (n.Text == text)
                {
                    returnNode = n;
                    break;
                }
            }
            return returnNode;
        }


        #endregion



        //------------------------------------------------------------
        // Tree View Functions
        //------------------------------------------------------------
        #region TreeView_Functions

        /// <summary>
        /// Clear the Tree View and build all nodes based upon the hex locations in hices.
        /// </summary>
        private void refreshTreeView()
        {
            //Create new nodes based upon loaded hex locations in hices
            treeFolders.Nodes.Clear();
            {
                TreeNode treeNode = new TreeNode(HexLocations.Name);
                if (HexLocations.Dirty)
                {
                    treeNode.ImageIndex = 4;
                    treeNode.SelectedImageIndex = 4;
                }
                else
                {
                    treeNode.ImageIndex = 0;
                    treeNode.SelectedImageIndex = 0;
                }
                treeNode.Expand();
                treeFolders.Nodes.Add(treeNode);
                foreach (string folder in HexLocations.GetFolders())
                {
                    TreeNode folderNode = new TreeNode(folder);
                    folderNode.ImageIndex = 1;
                    folderNode.SelectedImageIndex = 1;
                    treeNode.Nodes.Add(folderNode);
                    foreach (string group in HexLocations.GetGroups(folder))
                    {
                        TreeNode groupNode = new TreeNode(group);
                        groupNode.ImageIndex = 2;
                        groupNode.SelectedImageIndex = 2;
                        folderNode.Nodes.Add(groupNode);
                        foreach (HexLocation hexLocation in HexLocations.GetLocations(folder, group))
                        {
                            TreeNode hexLocationNode = new TreeNode(hexLocation.Name);
                            hexLocationNode.Tag = hexLocation;
                            groupNode.Nodes.Add(hexLocationNode);
                        }
                    }
                }
            }

            //Expand the full TreeView.
            treeFolders.ExpandAll();
        }


        //Add a new node
        private void addNodeByFullPath(TreeNode nodeToAdd, string fullPath, bool expandFullPath = false)
        {
            string[] fullPathNodes = fullPath.Split(new char[] { '\\' }, StringSplitOptions.None);
            TreeNode folderNode = findNodeText(treeFolders.Nodes[0].Nodes, fullPathNodes[1]);
            if (folderNode == null)
            {
                folderNode = new TreeNode(fullPathNodes[1]);
                folderNode.ImageIndex = 1;
                folderNode.SelectedImageIndex = 1;
                treeFolders.Nodes[0].Nodes.Add(folderNode);
            }
            TreeNode groupNode = findNodeText(folderNode.Nodes, fullPathNodes[2]);
            if (groupNode == null)
            {
                groupNode = new TreeNode(fullPathNodes[2]);
                groupNode.ImageIndex = 2;
                groupNode.SelectedImageIndex = 2;
                folderNode.Nodes.Add(groupNode);
            }

            groupNode.Nodes.Add(nodeToAdd);
            if (expandFullPath)
            {
                folderNode.Expand();
                groupNode.Expand();
            }
        }


        //Move the panel of node actions and control button enables.
        private void treeFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeFolders.SelectedNode.Parent == null)
            {
                panButtons.Hide();

            }
            else
            {
                panButtons.Top = e.Node.Bounds.Top + e.Node.Bounds.Height / 2 - panButtons.Height / 2 + treeFolders.Top;
                btnEdit.Enabled = nodeLevel(e.Node) == 3;
                panButtons.Show();
            }
        }


        #endregion



        //------------------------------------------------------------
        // Tree View Buttons (moev, add, remove, etc.)
        //------------------------------------------------------------
        #region TreeView_Buttons

        /// <summary>
        /// Moe the node up one position on the same level, if possible.
        /// </summary>
        private void btnUp_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeFolders.SelectedNode;
            if (treeNode != null && treeNode.Parent != null)
            {
                TreeNode parentNode = null;
                TreeNode prevNode = prevNodeLocation(treeNode, ref parentNode);
                int index = -1;
                if (prevNode == null && parentNode != null)
                {
                    index = 0;
                }
                else if (prevNode != null)
                {
                    if (parentNode == treeNode.Parent)
                    {
                        index = prevNode.Index;
                    }
                    else
                    {
                        index = prevNode.Index + 1;
                    }
                }
                if (index != -1)
                {
                    treeFolders.Nodes.Remove(treeNode);
                    parentNode.Nodes.Insert(index, treeNode);
                    treeFolders.SelectedNode = treeNode;
                }
            }
        }


        /// <summary>
        /// Move the node down one position on the same level, if possible.
        /// </summary>
        private void btnDown_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeFolders.SelectedNode;
            if (treeNode != null && treeNode.Parent != null)
            {
                TreeNode parentNode = null;
                TreeNode nextNode = nextNodeLocation(treeNode, ref parentNode);
                int index = -1;
                if (nextNode == null && parentNode != null)
                {
                    index = 0;
                }
                else if (nextNode != null)
                {
                    index = nextNode.Index;
                }
                if (index != -1)
                {
                    treeFolders.Nodes.Remove(treeNode);
                    parentNode.Nodes.Insert(index, treeNode);
                    treeFolders.SelectedNode = treeNode;
                }
            }
        }


        /// <summary>
        /// Find the prev node location for the same level in the Tree View, if one exists.
        /// </summary>
        /// <param name="searchNode">Starting node from which to search for the prev ndoe. Determines the original node level.</param>
        /// <param name="parentNode"></param>
        /// <param name="originalNodeLevel">Do not set this value.</param>
        /// <returns>The prev node location for the same level in the Tree View, if one exists.</returns>
        private TreeNode prevNodeLocation(TreeNode searchNode, ref TreeNode parentNode, int originalNodeLevel = -1)
        {
            //If the current node level <> original node level, then extra work needs to be done.
            int currentNodeLevel = nodeLevel(searchNode);

            //First call does not need to set a node level.
            //Node level is the level of the original searchNode, not the one at the current level of recursion, and must be preserved throughout subsequent recursive calls.
            if (originalNodeLevel == -1)
                originalNodeLevel = currentNodeLevel;

            //Find the prev node.
            if (searchNode.PrevNode == null)
            {
                if (searchNode.Parent == null)
                {
                    parentNode = null;
                    return null;
                }
                else
                {
                    //If there is no PrevNode, search the parent for the PrevNode.
                    TreeNode testNode = prevNodeLocation(searchNode.Parent, ref parentNode, originalNodeLevel);
                    parentNode = testNode;
                    if (testNode == null)
                        return null;
                    else
                        return testNode.LastNode;
                }
            }
            else if (currentNodeLevel + 1 < originalNodeLevel && searchNode.PrevNode.Nodes.Count == 0)
            {
                //If the found PrevNode has no children, look continue the search.
                TreeNode testNode = prevNodeLocation(searchNode.PrevNode, ref parentNode, originalNodeLevel);
                parentNode = testNode.Parent;
                return testNode;
            }
            else
            {
                parentNode = searchNode.PrevNode.Parent;
                return searchNode.PrevNode;
            }
        }



        /// <summary>
        /// Find the next node location for the same level in the Tree View, if one exists.
        /// </summary>
        /// <param name="searchNode">Starting node from which to search for the next ndoe. Determines the original node level.</param>
        /// <param name="parentNode"></param>
        /// <param name="originalNodeLevel">Do not set this value.</param>
        /// <returns>The next node location for the same level in the Tree View, if one exists.</returns>
        private TreeNode nextNodeLocation(TreeNode searchNode, ref TreeNode parentNode, int originalNodeLevel = -1)
        {
            //If the current node level <> original node level, then extra work needs to be done.
            int currentNodeLevel = nodeLevel(searchNode);

            //First call does not need to set a node level.
            //Node level is the level of the original searchNode, not the one at the current level of recursion, and must be preserved throughout subsequent recursive calls.
            if (originalNodeLevel == -1)
                originalNodeLevel = currentNodeLevel;


            //Find the next node.
            if (searchNode.NextNode == null)
            {
                if (searchNode.Parent == null)
                {
                    parentNode = null;
                    return null;
                }
                else
                {
                    //If there is no NextNode, search the parent for the NextNode.
                    TreeNode testNode = nextNodeLocation(searchNode.Parent, ref parentNode, originalNodeLevel);
                    parentNode = testNode;
                    if (testNode == null)
                        return null;
                    else
                        return testNode.FirstNode;
                }
            }
            else if (currentNodeLevel + 1 < originalNodeLevel && searchNode.NextNode.Nodes.Count == 0)
            {
                //If the found NextNode has no children, look continue the search.
                TreeNode testNode = nextNodeLocation(searchNode.NextNode, ref parentNode, originalNodeLevel);
                parentNode = testNode.Parent;
                return testNode;
            }
            else
            {
                parentNode = searchNode.NextNode.Parent;
                return searchNode.NextNode;
            }
        }


        /// <summary>
        /// Edit the Hex Location corresponding to the selected node. Only works on the lowest level nodes.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Only the lowest node level (the one representing an actual hex location) can be edited.
            if (treeFolders.SelectedNode != null && nodeLevel(treeFolders.SelectedNode) == 3)
            {
                HexLocation oldHexLocation = (HexLocation)treeFolders.SelectedNode.Tag;
                using (EditHexLocation editHexLocation = new EditHexLocation((HexLocation)treeFolders.SelectedNode.Tag, HexLocations, unchangedRom))
                {
                    if (editHexLocation.ShowDialog() == DialogResult.OK)
                    {
                        if (editHexLocation.newHexLocation.Folder != oldHexLocation.Folder || editHexLocation.newHexLocation.Group != oldHexLocation.Group)
                        {
                            TreeNode newHexLocationNode = new TreeNode(editHexLocation.newHexLocation.Name);
                            newHexLocationNode.Tag = editHexLocation.newHexLocation;
                            addNodeByFullPath(newHexLocationNode, treeFolders.Nodes[0].Text + "\\" + editHexLocation.newHexLocation.Folder + "\\" + editHexLocation.newHexLocation.Group, true);
                            treeFolders.Nodes.Remove(treeFolders.SelectedNode);
                            treeFolders.SelectedNode = newHexLocationNode;
                        }
                        else
                        {
                            treeFolders.SelectedNode.Text = editHexLocation.newHexLocation.Name;
                            treeFolders.SelectedNode.Tag = editHexLocation.newHexLocation;
                        }

                        editStack.Add(new HexLocationEdit(oldHexLocation, editHexLocation.newHexLocation));
                    }
                }
            }
        }


        /// <summary>
        /// Add a new Hex Location using the specified Folder and Group, if applicable. This is a "temporary" addition and will be deleted if the dialog is canceled.
        /// </summary>
        private void btnNew_Click(object sender, EventArgs e)
        {
            //Only the lowest node level (the one representing an actual hex location) can be edited.
            if (treeFolders.SelectedNode != null)
            {
                HexLocation newHexLocation = new HexLocation();
                switch (nodeLevel(treeFolders.SelectedNode))
                {
                    case 1:
                        newHexLocation.Folder = treeFolders.SelectedNode.Text;
                        break;
                    case 2:
                        newHexLocation.Folder = treeFolders.SelectedNode.Parent.Text;
                        newHexLocation.Group = treeFolders.SelectedNode.Text;
                        break;
                    case 3:
                        newHexLocation.Folder = ((HexLocation)treeFolders.SelectedNode.Tag).Folder;
                        newHexLocation.Group = ((HexLocation)treeFolders.SelectedNode.Tag).Group;
                        break;
                    default:
                        break;
                }
                using (EditHexLocation editHexLocation = new EditHexLocation(newHexLocation, HexLocations, unchangedRom))
                {
                    if (editHexLocation.ShowDialog() == DialogResult.OK)
                    {
                        TreeNode newHexLocationNode = new TreeNode(editHexLocation.newHexLocation.Name);
                        newHexLocationNode.Tag = editHexLocation.newHexLocation;
                        addNodeByFullPath(newHexLocationNode, treeFolders.Nodes[0].Text + "\\" + editHexLocation.newHexLocation.Folder + "\\" + editHexLocation.newHexLocation.Group, true);
                        
                        addedHexLocations.Add(editHexLocation.newHexLocation);
                    }
                }
            }
        }


        /// <summary>
        /// Remove the selected node. This does not delete the underlying hex location until changes are saved.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeFolders.SelectedNode != null)
            {
                if (MessageBox.Show("Are you sure you want to delete " + treeFolders.SelectedNode.Text + "?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    treeFolders.Nodes.Remove(treeFolders.SelectedNode);
                }
            }
        }


        #endregion



        //------------------------------------------------------------
        // Dialog Buttons
        //------------------------------------------------------------
        #region Dialog_Buttons

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Undo any edits made to the Hex Locations.
            for (int n = editStack.Count - 1; n >= 0; n--)
            {
                HexLocations.Items.Insert(HexLocations.Items.IndexOf(editStack[n].NewHexLocation), editStack[n].OldHexLocation); 
                HexLocations.Items.Remove(editStack[n].NewHexLocation);
            }

            //Remove anything that was added, since the user indicated they didn't want to save those changes.
            foreach (HexLocation h in addedHexLocations)
            {
                HexLocations.Items.Remove(h);
            }

            //Reset the dirty flag
            HexLocations.Dirty = hexLocationsDirtyFlag;
            
            //Return result
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            List<HexLocation> reorderedItems = new List<HexLocation>();
            foreach (TreeNode n in treeFolders.Nodes.Find("",true))
            {
                if (nodeLevel(n) == 3)
                {
                    reorderedItems.Add((HexLocation)n.Tag);
                }
            }
            HexLocations.SetItems(reorderedItems);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        #endregion



    }


    /// <summary>
    /// Stores the change to the hex location by recording the old and new versions.
    /// </summary>
    class HexLocationEdit
    {
        public HexLocation OldHexLocation { get; set; }
        public HexLocation NewHexLocation { get; set; }

        public HexLocationEdit(HexLocation oldHexLocation, HexLocation newHexLocation)
        {
            OldHexLocation = oldHexLocation;
            NewHexLocation = newHexLocation;
        }
    }
}
