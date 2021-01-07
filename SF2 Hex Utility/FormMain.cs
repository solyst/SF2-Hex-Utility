using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SF2_Hex_Utility
{
    public partial class FormMain : Form
    {
        public Dictionary<string, HexLocations> hices = new Dictionary<string, HexLocations>();
        public Rom rom = new Rom();
        public Rom unchangedRom = null;

        private readonly string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly string hexPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Hex Locations");
        private readonly string romPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Unchanged Roms");
        const string HEX_LOCATIONS_FILE_EXTENSION = "hxl";

        public FormMain()
        {
            InitializeComponent();

#if DEBUG
            tsmHelpTestScreen.Visible = true;
#else
            tsmHelpTestScreen.Visible = false;
#endif

            //Create Hex Path if it doens't already exist
            Directory.CreateDirectory(hexPath);
            Directory.CreateDirectory(romPath);

            //Create an initial HexLocations file, if one does not already exist
            if (getHexLocationsFiles().Count() == 0)
            {
                HexLocations h = new HexLocations(Path.Combine(hexPath, @"Default." + HEX_LOCATIONS_FILE_EXTENSION));
                h.Save();
            }

            //Load all Hex Locations file in the hexPath directory
            loadAllHexLocationsFiles();

            //Find all unchanged roms
            populateUnchangedRomsMenu();

            //Display all the hex locations
            refreshTreeView();

            //Apply events
            editFieldList1.FieldFocusChanged += editFieldList_FieldFocusChanged;
            editFieldList1.FieldChanged += editFieldList_FieldChanged;
            editFieldList1.NoneSelected += editFieldList_NoneSelected;
            editFieldList1.SelectionChanged += editFieldList_SelectionChanged;
            rom.RomEdited += rom_RomEdited;
            rom.RomSaved += rom_RomSaved;
            rom.RomOpened += rom_RomOpened;

            //Events for hex locations handled in loadAllHexLocationsFiles();
        }






        //---------------------------------------------------------
        // Hices Functions
        //---------------------------------------------------------
        // Hices is the name recommended by the IDE. It's not a real word. It is not the plural for Hex. But I'm using it anyway.
        #region Hices_Functions

        private string[] getHexLocationsFiles()
        {
            return Directory.GetFiles(hexPath, "*." + HEX_LOCATIONS_FILE_EXTENSION);
        }


        private void loadAllHexLocationsFiles()
        {
            foreach (string s in getHexLocationsFiles())
            {
                addHexLocationsToHices(s);
            }
        }


        private bool saveAllHexLocationsFiles()
        {
            bool error = false;
            foreach (HexLocations h in hices.Values)
            {
                if (h.Save() == false)
                    error = true;
            }
            return !error;
        }


        private void addHexLocationsToHices(string filePath)
        {
            HexLocations h = new HexLocations(filePath);
            h.Load();
            h.ListHexLocationChanged += hexLocations_ListHexLocationChanged;
            h.DirtyFlagChanged += hexLocations_DirtyFlagChanged;
            hices.Add(h.Name, h);
        }


        #endregion



        //---------------------------------------------------------
        // Tools Menu and Unchaged Rom Functions
        //---------------------------------------------------------
        #region Tools_Menu_UnchangedRom_Functions

        private void populateUnchangedRomsMenu()
        {
            List<string> unchangedRomsList = new List<string>();
            unchangedRomsList.AddRange(Directory.GetFiles(romPath, "*.bin"));
            List<ToolStripItem> items = new List<ToolStripItem>();
            foreach (string s in unchangedRomsList)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(Path.GetFileNameWithoutExtension(s));
                menuItem.Click += tsmToolsUnchangedRoms_Click;
                menuItem.Tag = s;
                items.Add(menuItem);
            }
            tsmToolsUnchangedRoms.DropDownItems.AddRange(items.ToArray());
        }


        private void tsmToolsUnchangedRoms_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            uncheckAllUnchangedRomMenuItems();
            unchangedRom = new Rom();
            if (unchangedRom.Open((string)menuItem.Tag) == true)
            {
                menuItem.Checked = true;
                editFieldList1.UnchangedRom = unchangedRom;
                editFieldDetails1.UnchangedRom = unchangedRom;
            }

        }


        private void uncheckAllUnchangedRomMenuItems()
        {
            foreach (ToolStripMenuItem item in tsmToolsUnchangedRoms.DropDownItems)
            {
                item.Checked = false;
            }
            editFieldList1.UnchangedRom = null;
            editFieldDetails1.UnchangedRom = null;
        }


        private void tsmAlwaysGenerateContext_Click(object sender, EventArgs e)
        {
            tsmAlwaysGenerateContext.Checked = !tsmAlwaysGenerateContext.Checked;
            EditFieldDetails.AlwaysGenerateContextFromRom = tsmAlwaysGenerateContext.Checked;
        }


        #endregion



        //---------------------------------------------------------
        // File Menu Functions
        //---------------------------------------------------------
        #region FileMenu_Functions

        private void tsmFileExit_Click(object sender, EventArgs e)
        {
            if (rom.Dirty)
            {
                if (MessageBox.Show("There are unsaved changes to rom " + rom.Name + ". Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                bool changedFile = false;
                foreach (HexLocations h in hices.Values)
                {
                    if (h.Dirty)
                    {
                        changedFile = true;
                        break;
                    }
                }
                if (changedFile)
                {
                    if (MessageBox.Show("There are unsaved changes to one or more Hex Location List. Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }


        #endregion



        //---------------------------------------------------------
        // Rom Menu Functions
        //---------------------------------------------------------
        #region RomMenu_Functions

        private void tsmRomOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialogRom.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialogRom.FileName) != ".bin")
                {
                    if (MessageBox.Show("This file may not be a valid rom. Are you sure you wish to open " + openFileDialogRom.FileName + "?", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                //Prevents certain updates so that the rom loads faster
                //editFieldList1.SelectNone();
                //editFieldDetails1.Visible = false;
                //Load the rom
                if (rom.Open(openFileDialogRom.FileName))
                {
                    tsmRomSave.Enabled = true;
                    tsmRomSaveAs.Enabled = true;
                    tsmRomReload.Enabled = true;

                    lblRom.Text = rom.Name;
                    editFieldList1.Rom = rom;
                }
                else
                {
                    rom = new Rom();
                    tsmRomSave.Enabled = false;
                    tsmRomSaveAs.Enabled = false;
                    tsmRomReload.Enabled = false;

                    lblRom.Text = "No Rom Loaded";
                    editFieldList1.Rom = null;
                }
            }
        }

        private void tsmRomSave_Click(object sender, EventArgs e)
        {
            rom.Save();
        }

        private void tsmRomSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialogRom.InitialDirectory = Path.GetDirectoryName(rom.FilePath);
            saveFileDialogRom.FileName = rom.Name;
            if (saveFileDialogRom.ShowDialog() == DialogResult.OK)
            {
                if (rom.Save(saveFileDialogRom.FileName))
                {
                    lblRom.Text = rom.Name;
                }
            }
        }

        private void tsmRomReload_Click(object sender, EventArgs e)
        {
            if (rom.Dirty &&
                MessageBox.Show("You have unsaved changes. All changes will be lost if the rom is reloaded. Are you sure you want to reload the rom?", "Reload Rom", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                rom.Reload();
            }
        }

        #endregion



        //---------------------------------------------------------
        // Locations Menu Functions
        //---------------------------------------------------------
        #region LocationsMenu_Functions


        private void tsmLocationsNew_Click(object sender, EventArgs e)
        {
            using (NewHexLocations newHexLocations = new NewHexLocations(hexPath, HEX_LOCATIONS_FILE_EXTENSION))
            {
                if (newHexLocations.ShowDialog() == DialogResult.OK)
                {
                    addHexLocationsToHices(newHexLocations.FilePath);
                    refreshTreeView();
                }
            }
        }


        /// <summary>
        /// Add a new Location to the Hex Locations selected in the TreeView.
        /// </summary>
        private void tsmLocationsNewLocation_Click(object sender, EventArgs e)
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

            HexLocations selectedHexLocations = getSelectedHexLocations();
            using (EditHexLocation editHexLocation = new EditHexLocation(newHexLocation, selectedHexLocations, unchangedRom))
            {
                if (editHexLocation.ShowDialog() == DialogResult.OK)
                {
                    //refreshTreeView();
                    //No need to add the location to selectedHexLocations. The dialog will do that automatically.
                    //No need to refresh the TreeView. That will happen automatically with an event.
                }
            }
        }


        /// <summary>
        /// Append the locations of the opened file to the hex locations currently selected in the Tree View.
        /// </summary>
        private void tsmLocationsImportList_Click(object sender, EventArgs e)
        {
            if (openFileDialogHexList.ShowDialog() == DialogResult.OK)
            {
                HexLocations h = new HexLocations(openFileDialogHexList.FileName);
                h.ReadItemsFromFile();
                getSelectedHexLocations().AddRange(h.Items.ToList());
                //No need to refresh the TreeView. That will happen automatically with an event.
            }
        }


        private void tsmLocationsSave_Click(object sender, EventArgs e)
        {
            getSelectedHexLocations().Save();
        }


        private void tsmLocationsSaveAll_Click(object sender, EventArgs e)
        {
            saveAllHexLocationsFiles();
        }


        private void tsmLocationsEditList_Click(object sender, EventArgs e)
        {
            using (EditHexLocationsList editHexLocationsList = new EditHexLocationsList(getSelectedHexLocations(), unchangedRom))
            {
                if (editHexLocationsList.ShowDialog() == DialogResult.OK)
                {
                    //refreshTreeView();
                }
            }
        }


        private void tsmLocationsEditLocation_Click(object sender, EventArgs e)
        {
            if (editFieldList1.SelectedItems.Count > 0)
            {
                EditField editField = editFieldList1.SelectedItems[0];
                using (EditHexLocation editHexLocation = new EditHexLocation(editField.HexLocation, editFieldList1.ParentHexLocations, unchangedRom))
                {
                    if (editHexLocation.ShowDialog() == DialogResult.OK)
                    {
                        editField.HexLocation = editHexLocation.newHexLocation;
                    }
                }
            }
        }


        #endregion



        //---------------------------------------------------------
        // Help Menu Functions
        //---------------------------------------------------------
        #region HelpMenu_Functions

        private void tsmShowToolTips_Click(object sender, EventArgs e)
        {
            tsmShowToolTips.Checked = !tsmShowToolTips.Checked;
            EditHexLocation.ShowToolTips = tsmShowToolTips.Checked;
            editFieldDetails1.ShowToolTips = tsmShowToolTips.Checked;
        }


        private void tsmHelpTestScreen_Click(object sender, EventArgs e)
        {
            FormTesting formTesting = new FormTesting()
            {
                TestRom = rom
            };
            formTesting.Show();
        }


        private void tsmHelpAbout_Click(object sender, EventArgs e)
        {
            using (FormAbout formAbout = new FormAbout())
            {
                formAbout.ShowDialog();
            }
        }


        #endregion



        //---------------------------------------------------------
        // Edit Field Detail Functions
        //---------------------------------------------------------
        #region EditFieldDetail_Functions

        /// <summary>
        /// Update the hex location details view with the selected hex location from the edit field list.
        /// </summary>
        private void updateEditFieldDetails()
        {
            updateEditFieldDetails(editFieldList1.SelectedItems);
        }


        /// <summary>
        /// Update the hex location details view if editFields contains only a single entry. Otherwise, clear the hex location view details.
        /// </summary>
        /// <param name="editFields">Usually a list of selected fields.</param>
        private void updateEditFieldDetails(List<EditField> editFields)
        {
            if (editFields.Count > 1 || editFields.Count == 0)
                editFieldDetails1.UpdateDetails(null);
            else
                editFieldDetails1.UpdateDetails(editFields[0]);
        }



        #endregion



        //---------------------------------------------------------
        // Tree View Functions
        //---------------------------------------------------------
        #region TreeView_Functions

        private string oldSelectionPath = "";

        /// <summary>
        /// Return the zero-indexed node level for treeNode.
        /// </summary>
        /// <param name="treeNode">The node for which to find the node level.</param>
        /// <returns>The node level for treeNode.</returns>
        private int nodeLevel(TreeNode treeNode)
        {
            return treeNode.FullPath.Count(x => x == '\\');
        }


        /// <summary>
        /// Get the Hex Locations object associated with the Tree View selection. If no selection is made, return the first hex location in the Tree View.
        /// </summary>
        /// <returns>Return the Hex Locations object associated with the Tree View selection. If no selection, return NULL.</returns>
        private HexLocations getSelectedHexLocations()
        {
            if (treeFolders.Nodes.Count == 0)
                return null;

            TreeNode n = treeFolders.SelectedNode;
            if (n == null)
                return hices[treeFolders.Nodes[0].Text];

            switch (nodeLevel(n))
            {
                case 0:
                    return hices[treeFolders.SelectedNode.Text];
                case 1:
                    return hices[treeFolders.SelectedNode.Parent.Text];
                case 2:
                    return hices[treeFolders.SelectedNode.Parent.Parent.Text];
                default:
                    return null;
            }
        }


        /// <summary>
        /// Clear the Tree View and build all nodes based upon the hex locations in hices.
        /// </summary>
        private void refreshTreeView()
        {
            //Record the current expanded state for all nodes, based upon their FullPath
            Dictionary<string, bool> nodeExpandedState = new Dictionary<string, bool>();
            getTreeNodesExpandedState(treeFolders.Nodes, ref nodeExpandedState);

            //Record the currently selected node
            string selectedNodeFullPath = "";
            TreeNode selectedNode = treeFolders.SelectedNode;
            if (selectedNode != null)
                selectedNodeFullPath = selectedNode.FullPath;

            //Create new nodes based upon loaded hex locations in hices
            treeFolders.Nodes.Clear();
            foreach (HexLocations hexLocations in hices.Values)
            {
                TreeNode treeNode = new TreeNode(hexLocations.Name);
                if (hexLocations.Dirty)
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
                foreach (string folder in hexLocations.GetFolders())
                {
                    TreeNode subNode = new TreeNode(folder);
                    subNode.ImageIndex = 1;
                    subNode.SelectedImageIndex = 1;
                    treeNode.Nodes.Add(subNode);
                    foreach (string group in hexLocations.GetGroups(folder))
                    {
                        TreeNode groupNode = new TreeNode(group);
                        groupNode.ImageIndex = 2;
                        groupNode.SelectedImageIndex = 2;
                        subNode.Nodes.Add(groupNode);
                    }
                }
            }

            //Re-expand out the tree view
            setTreeNodesExpandedState(treeFolders.Nodes, nodeExpandedState);

            //Select the appropriate node
            if (selectedNodeFullPath != "")
            {
                TreeNode nodeToSelect = findTreeNodeFromFullPath(treeFolders.Nodes, selectedNodeFullPath);
                if (nodeToSelect != null)
                    treeFolders.SelectedNode = nodeToSelect;
                else
                    treeFolders.SelectedNode = treeFolders.Nodes[0];
            }
        }


        /// <summary>
        /// Populate a dictionary of key Node FullPath and Bool IsExpanded for each node, on any level, in treeNodes.
        /// </summary>
        /// <param name="treeNodes">Nodes for which their IsExpanded state is to be recorded.</param>
        /// <param name="nodeExpandedState">Dictionary in which the nodes' IsExpanded state is to be recorded.</param>
        private void getTreeNodesExpandedState(TreeNodeCollection treeNodes, ref Dictionary<string, bool> nodeExpandedState)
        {
            foreach (TreeNode n in treeNodes)
            {
                nodeExpandedState.Add(n.FullPath, n.IsExpanded);
                getTreeNodesExpandedState(n.Nodes, ref nodeExpandedState);
            }
        }


        /// <summary>
        /// Set the expanded state for treeNodes based upon the dictionary nodeExpandedState.
        /// </summary>
        /// <param name="treeNodes">Nodes for which their IsExpanded state is to be set.</param>
        /// <param name="nodeExpandedState">Dictionary in which the nodes' IsExpanded state is recorded.</param>
        private void setTreeNodesExpandedState(TreeNodeCollection treeNodes, Dictionary<string, bool> nodeExpandedState)
        {
            foreach (TreeNode n in treeNodes)
            {
                if (nodeExpandedState.ContainsKey(n.FullPath))
                {
                    if (nodeExpandedState[n.FullPath] == true)
                    {
                        n.Expand();
                    }
                }
                setTreeNodesExpandedState(n.Nodes, nodeExpandedState);
            }
        }


        /// <summary>
        /// Return the TreeNode with a FullPath matching that of fullPath. Will search children of treeNodes.
        /// </summary>
        /// <param name="treeNodes">Nodes to search.</param>
        /// <param name="fullPath">FullPath to find.</param>
        /// <returns>Node with matching FullPath.</returns>
        private TreeNode findTreeNodeFromFullPath(TreeNodeCollection treeNodes, string fullPath)
        {
            foreach (TreeNode n in treeNodes)
            {
                if (n.FullPath == fullPath)
                {
                    return n;
                }
                else
                {
                    TreeNode temp = findTreeNodeFromFullPath(n.Nodes, fullPath);
                    if (temp != null)
                    {
                        return temp;
                    }
                }
            }
            return null;
        }


        private void treeFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (oldSelectionPath != treeFolders.SelectedNode.FullPath)
            {
                //Only make updates if something has changed
                oldSelectionPath = treeFolders.SelectedNode.FullPath;

                //Manage Menu Item - Edit List
                tsmLocationsEditList.Enabled = treeFolders.SelectedNode != null;
                tsmLocationsImportList.Enabled = treeFolders.SelectedNode != null;
                tsmLocationsNew.Enabled = treeFolders.SelectedNode != null;
                tsmLocationsNewLocation.Enabled = treeFolders.SelectedNode != null;
                tsmLocationsSave.Enabled = treeFolders.SelectedNode != null;

                //Show Item List only when groups are selected
                if (treeFolders.SelectedNode.FullPath.Count(x => x == '\\') == 2)
                {
                    string folder = treeFolders.SelectedNode.Parent.Text;
                    string group = treeFolders.SelectedNode.Text;

                    HexLocations selectedHexLocations = getSelectedHexLocations();
                    editFieldList1.SetListContents(selectedHexLocations, folder, group);
                }
                else
                {
                    editFieldList1.SetListContents(null, null, null);
                }
            }
        }


        #endregion



        //---------------------------------------------------------
        // Custom Events
        //---------------------------------------------------------
        #region CustomEvents

        private void editFieldList_FieldFocusChanged(object sender, EditFieldList.FieldChangedEventArgs e)
        {
            //editFieldDetails1.UpdateDetails(e.EditField);
        }

        private void editFieldList_FieldChanged(object sender, EditFieldList.FieldChangedEventArgs e)
        {
            editFieldDetails1.UpdateDetails(e.EditField);
            tsmLocationsEditLocation.Enabled = editFieldList1.SelectedItems.Count > 0;
        }

        private void editFieldList_NoneSelected(object sender, EventArgs e)
        {
            editFieldDetails1.UpdateDetails(null);
            tsmLocationsEditLocation.Enabled = false;
        }

        private void editFieldList_SelectionChanged(object sender, EditFieldList.SelectionChangedEventArgs e)
        {
            updateEditFieldDetails(e.SelectedItems);
            tsmLocationsEditLocation.Enabled = e.SelectedItems.Count > 0;
        }

        private void hexLocations_ListHexLocationChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //This is triggered with the DirtyFlagChanaged automatically
            //refreshTreeView();
        }

        private void hexLocations_DirtyFlagChanged(object sender, EventArgs e)
        {
            refreshTreeView();
        }


        //Color the rom text red when it is edited but not saved; color black otherwise
        private void rom_RomOpened(object sender, EventArgs e)
        {
            lblRom.ForeColor = Color.Black;
            lblRom.Font = new Font(lblRom.Font, FontStyle.Regular);
        }

        private void rom_RomSaved(object sender, EventArgs e)
        {
            lblRom.ForeColor = Color.Black;
            lblRom.Font = new Font(lblRom.Font, FontStyle.Regular);
        }

        private void rom_RomEdited(object sender, EventArgs e)
        {
            lblRom.ForeColor = Color.DarkRed;
            //lblRom.Font = new Font(lblRom.Font, FontStyle.Bold);
        }




        #endregion

        
    }
}
