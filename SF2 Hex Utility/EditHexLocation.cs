using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Channels;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public partial class EditHexLocation : Form
    {
        static public bool SuppressBeeps { get; set; } = false;
        static public bool ShowToolTips { get; set; } = false;

        public HexLocation newHexLocation;
        private HexLocation oldHexLocation;
        private HexLocations existingHexLocations;

        private Rom unchangedRom = new Rom();


        public EditHexLocation(HexLocation hexLocation, HexLocations existingLocations, Rom unchangedRom)
        {
            InitializeComponent();

            toolTip1.Active = ShowToolTips;

            existingHexLocations = existingLocations;
            refreshFolders();
            refreshGroups();
            cboRefType.Items.AddRange(HexLocation.GetReferenceTypeNames());
            cboFormat.Items.AddRange(HexLocation.GetDataFormatNames());
            this.unchangedRom = unchangedRom;
            oldHexLocation = hexLocation;
            newHexLocation = new HexLocation();
            resetNewHexLocationToOld();
            refreshFields();

            //Assign events
            cboFolder.Validating += cboFolder_Validating;
            cboFolder.Validated += cboFolder_Validated;
            cboGroup.Validating += cboGroup_Validating;
            cboGroup.Validated += cboGroup_Validated;
            txtName.Validating += txtName_Validating;
            txtName.Validated += txtName_Validated;
            txtLocation.Validating += txtLocation_Validating;
            txtLocation.Validated += txtLocation_Validated;
            numLength.Validating += numLength_Validating;
            numLength.Validated += numLength_Validated;
            cboRefType.Validating += cboRefType_Validating;
            cboRefType.Validated += cboRefType_Validated;
            txtSearchBytes.Validating += txtSearchBytes_Validating;
            txtSearchBytes.Validated += txtSearchBytes_Validated;

            txtDescription.Validating += txtDescription_Validating;
            txtDescription.Validated += txtDescription_Validated;

            txtBytes.Validating += txtBytes_Validating;
            txtBytes.Validated += txtBytes_Validated;
            txtContext.Validating += txtContext_Validating;
            txtContext.Validated += txtContext_Validated;
            numOriginalValueIndex.Validating += numOriginalValueIndex_Validating;
            numOriginalValueIndex.Validated += numOriginalValueIndex_Validated;
            cboFormat.Validating += cboFormat_Validating;
            cboFormat.Validated += cboFormat_Validated;


            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(ComboBox) || c.GetType() == typeof(TextBox))
                {
                    c.KeyDown += textControl_KeyDown;
                    c.Validated += textControl_Validated;
                    c.Tag = c.Text; //record the valid entries
                }
                if (c.GetType() == typeof(NumericUpDown))
                {
                    c.KeyDown += numControl_KeyDown;
                    c.Validated += numControl_Validated;
                    c.Tag = ((NumericUpDown)c).Value; //record the valid entries
                }
            }


        }



        //------------------------------------------------------------
        // Field Refresh Functions
        //------------------------------------------------------------
        #region FieldRefresh_Functions

        /// <summary>
        /// Replace the newHexLocation with the oldHexLocation data (ie: undo all changes).
        /// </summary>
        private void resetNewHexLocationToOld()
        {
            newHexLocation = oldHexLocation.Clone();
            refreshFields();
        }


        /// <summary>
        /// Update all fields with the value from the newHexLocation.
        /// </summary>
        private void refreshFields()
        {
            cboFolder.Text = newHexLocation.Folder;
            cboGroup.Text = newHexLocation.Group;
            txtName.Text = newHexLocation.Name;
            txtLocation.Text = HexRenderer.IntToHexString(newHexLocation.Location, 4);
            txtSearchBytes.Text = HexRenderer.BytesToHexStringWildcard(newHexLocation.SearchBytes);
            numLength.Value = newHexLocation.Length;
            cboRefType.Text = newHexLocation.RefType.ToString();
            cboFormat.Text = newHexLocation.Format.ToString();
            txtBytes.Text = HexRenderer.BytesToHexString(newHexLocation.Bytes, false);
            txtContext.Text = HexRenderer.BytesToHexString(newHexLocation.Context, true);
            numOriginalValueIndex.Value = newHexLocation.ByteOffsetInContext;
            txtDescription.Text = newHexLocation.Description;
        }


        /// <summary>
        /// Populate the folder combo box with all existing folder names.
        /// </summary>
        private void refreshFolders()
        {
            cboFolder.Items.Clear();
            List<string> folders = existingHexLocations.GetFolders().ToList();
            folders.Sort();
            cboFolder.Items.AddRange(folders.ToArray());

            if (cboFolder.Text != "" && !existingHexLocations.GetFolders().Contains(cboFolder.Text))
            {
                cboFolder.Items.Add(cboFolder.Text);
            }
            refreshGroups(); //since the available groups depend on the folder, update the group list as well
        }


        /// <summary>
        /// Populate the group combo box with all existing group names.
        /// </summary>
        private void refreshGroups()
        {
            cboGroup.Items.Clear();
            List<string> groups = existingHexLocations.GetGroups(cboFolder.Text).ToList();
            groups.Sort();
            cboGroup.Items.AddRange(groups.ToArray());

            if (cboGroup.Text != "" && !existingHexLocations.GetGroups(cboFolder.Text).Contains(cboGroup.Text))
            {
                cboGroup.Items.Add(cboGroup.Text);
            }
        }

        #endregion



        //------------------------------------------------------------
        // User Interface
        //------------------------------------------------------------
        #region UserInterface

        /// <summary>
        /// Find the byte in Context where the Original Value first occurs and insert that into numOriginalValueIndex.
        /// </summary>
        private void btnFindOriginalValueIndex_Click(object sender, EventArgs e)
        {
            updateByteOffsetInContextUsingOriginalBytes(false, true);
            picHex.Refresh();
        }


        /// <summary>
        /// Create context using the selected unchanged rom, if one exists.
        /// </summary>
        private void btnAutoContext_Click(object sender, EventArgs e)
        {
            if (unchangedRom == null)
            {
                MessageBox.Show("No unchanged rom selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                newHexLocation.SetContextFromRom(unchangedRom);
                //int startByte = Math.Max(0, HexRenderer.FindFirstByte(newHexLocation.Location) - HexRenderer.BytesPerLine);
                //int length = ((HexRenderer.BytesPerLine * 3 + newHexLocation.Length) / HexRenderer.BytesPerLine) * HexRenderer.BytesPerLine;
                //newHexLocation.Context = unchangedRom.GetData(startByte, length);
                txtContext.Text = HexRenderer.BytesToHexString(newHexLocation.Context, true);
                updateByteOffsetInContextUsingOriginalBytes(false, false);
                picHex.Refresh();
            }
        }


        /// <summary>
        /// Draw the visual representation of the hex.
        /// </summary>
        private void picHex_Paint(object sender, PaintEventArgs e)
        {
            int firstByteOffset = HexRenderer.FindFirstByteOffset(newHexLocation.GetContextLocation(unchangedRom));
            HexRenderer.DrawHexWithHighlight(e.Graphics, newHexLocation.Context, true, firstByteOffset, newHexLocation.ByteOffsetInContext, newHexLocation.Bytes.Length, new Point(10, 10));
        }


        /// <summary>
        /// Find the Original Value in the Context and update the ByteOffsetInContext accordingly.
        /// </summary>
        private void updateByteOffsetInContextUsingOriginalBytes(bool suppressMessages = false, bool startSearchAtCurrentIndex = false)
        {
            //Search
            int value = -1;
            if (startSearchAtCurrentIndex == true)
                value = newHexLocation.FindByteOffsetInContext(txtBytes.Text, (int)numOriginalValueIndex.Value + 1);
            if (value == -1)
                value = newHexLocation.FindByteOffsetInContext(txtBytes.Text); //this always searches from the beginning of the list

            if (value > -1)
            {
                //Match found
                newHexLocation.SetByteOffsetInContext(value);
                numOriginalValueIndex.Value = value;
            }
            else
            {   
                //No match
                if (suppressMessages == false)
                    MessageBox.Show("No match of the Original Value found in the Context. Index not changed.");
            }
        }


        /// <summary>
        /// Show the search bytes text box only if necessary.
        /// </summary>
        private void cboRefType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchBytes.Visible = (cboRefType.Text == Enum.GetName(typeof(ReferenceType), ReferenceType.WildcardMatch));
        }


        /// <summary>
        /// Submit the changes.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //this.ValidateChildren();
            if (allFieldsValidated())
            {
                //Update the ContextLocation
                newHexLocation.ContextLocation = newHexLocation.GetContextLocation(unchangedRom);
                //check if it is new or edit
                int indexOfOldHexLocation = existingHexLocations.Items.IndexOf(oldHexLocation);
                if (indexOfOldHexLocation == -1)
                {
                    //if this is a new hex location
                    existingHexLocations.Add(newHexLocation);
                }
                else                            
                {
                    //if this is an edit for an existing hex location, replace the old location with the new
                    existingHexLocations.Replace(indexOfOldHexLocation, newHexLocation);
                }
                newHexLocation.RaiseEventHexLocationValueChanged(); //IMPORTANT: this is the only line in the whole program that denotes when the Hex Location has changed!
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (SuppressBeeps == false)
                    SystemSounds.Beep.Play();
                MessageBox.Show("Unable to process result. There are data errors in the fields.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        /// <summary>
        /// Reset the newHexLocation back to the values of oldHexLocation.
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            resetNewHexLocationToOld();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        #endregion



        //------------------------------------------------------------
        // User entry validation
        //------------------------------------------------------------
        // Validate each field entry and update newHexLocation with the value.
        // If an error is encountered, do not update newHexLocation and flag the field as errored.
        // If no error is encountered, clear the ErrorProvider of errors and update the text to ensure it matches the value from the object.
        #region UserEntryValidation

        //Validate Folder
        private void cboFolder_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetFolder);
        }

        private void cboFolder_Validated(object sender, EventArgs e)
        {
            cboFolder.Text = newHexLocation.Folder;
            refreshFolders();
        }


        //Validate Group
        private void cboGroup_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetGroup);
        }

        private void cboGroup_Validated(object sender, EventArgs e)
        {
            cboGroup.Text = newHexLocation.Group;
            refreshGroups();
        }


        //Validate Name
        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetName);
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            txtName.Text = newHexLocation.Name;
        }


        //Validate Location
        private void txtLocation_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetLocationFromHex);
        }

        private void txtLocation_Validated(object sender, EventArgs e)
        {
            txtLocation.Text = HexRenderer.BytesToHexString(ByteConversion.ToBytes((UInt32)newHexLocation.Location, 4));
        }


        //Validate Length
        private void numLength_Validating(object sender, CancelEventArgs e)
        {
            numControl_Validating(sender, e, newHexLocation.SetLength);
        }

        private void numLength_Validated(object sender, EventArgs e)
        {
            numLength.Value = newHexLocation.Length;
            newHexLocation.SetBytesFromHex(txtBytes.Text);
            txtBytes.Text = HexRenderer.BytesToHexString(newHexLocation.Bytes);
        }


        //Validate Reference Type
        private void cboRefType_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetReferenceType);
        }

        private void cboRefType_Validated(object sender, EventArgs e)
        {
            cboRefType.Text = Enum.GetName(typeof(ReferenceType), newHexLocation.RefType);
        }


        //Validate Search Bytes
        private void txtSearchBytes_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetSearchBytesFromHex);
        }

        private void txtSearchBytes_Validated(object sender, EventArgs e)
        {
            txtSearchBytes.Text = HexRenderer.BytesToHexStringWildcard(newHexLocation.SearchBytes);
        }


        //Validate Description
        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetDescription);
        }

        private void txtDescription_Validated(object sender, EventArgs e)
        {
            txtDescription.Text = newHexLocation.Description;
        }


        //Validate Original Value (bytes)
        private void txtBytes_Validating(object sender, CancelEventArgs e)
        {
            if (newHexLocation.ValidateHex(txtBytes.Text, out byte[] bytes))
            {
                if (bytes.Length > 0 && bytes.Length < 100)
                {
                    numLength.Value = bytes.Length;
                    newHexLocation.Length = bytes.Length;
                }
            }
            //textControl_Validating(sender, e, setBytesFromHexRight);
            textControl_Validating(sender, e, newHexLocation.SetBytesFromHex);
        }
        //private bool setBytesFromHexRight(string hex) //necessary so that the function has the same parameters as expected
        //{
        //    return newHexLocation.SetBytesFromHex(hex, true);
        //}

        private void txtBytes_Validated(object sender, EventArgs e)
        {
            txtBytes.Text = HexRenderer.BytesToHexString(newHexLocation.Bytes);
            updateByteOffsetInContextUsingOriginalBytes(true, true);
        }


        //Validate Context
        private void txtContext_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetContextFromHex);
        }

        private void txtContext_Validated(object sender, EventArgs e)
        {
            txtContext.Text = HexRenderer.BytesToHexString(newHexLocation.Context);
            updateByteOffsetInContextUsingOriginalBytes(true, true);
        }


        //Validate OriginalLocationIndex
        private void numOriginalValueIndex_Validating(object sender, CancelEventArgs e)
        {
            numControl_Validating(sender, e, newHexLocation.SetByteOffsetInContext);
        }

        private void numOriginalValueIndex_Validated(object sender, EventArgs e)
        {
            numOriginalValueIndex.Value = newHexLocation.ByteOffsetInContext;
        }


        //Validate Data Format
        private void cboFormat_Validating(object sender, CancelEventArgs e)
        {
            textControl_Validating(sender, e, newHexLocation.SetFormat);
        }

        private void cboFormat_Validated(object sender, EventArgs e)
        {
            cboFormat.Text = Enum.GetName(typeof(DataFormat), newHexLocation.Format);
        }


        #endregion



        //------------------------------------------------------------
        // Generic Validation Methods
        //------------------------------------------------------------
        #region GenericValidation_Methods

        /// <summary>
        /// Generic handler for validating text based controls.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Cancel event args.</param>
        /// <param name="func">Function to call for validation and to set the value in the HexLocation.</param>
        private void textControl_Validating(object sender, CancelEventArgs e, Func<string, bool> func)
        {
            string errorMsg = "";
            string value = sender.GetType().Equals(typeof(TextBox)) ? ((TextBox)sender).Text : ((ComboBox)sender).Text;
            if (!func(value))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                errorMsg = "Invalid Entry";
                if (sender.GetType().Equals(typeof(TextBox)))
                    ((TextBox)sender).Select(0, ((TextBox)sender).Text.Length);
                else
                    ((ComboBox)sender).Select(0, ((ComboBox)sender).Text.Length);
                if (SuppressBeeps == false)
                    SystemSounds.Beep.Play();
            }
            this.errorProvider1.SetError((Control)sender, errorMsg);
        }


        /// <summary>
        /// Generic handler for validating number based controls.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Cancel event args.</param>
        /// <param name="func">Function to call for validation and to set the value in the HexLocation.</param>
        private void numControl_Validating(object sender, CancelEventArgs e, Func<int, bool> func)
        {
            string errorMsg = "";
            int value = (int)((NumericUpDown)sender).Value;
            if (!func(value))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                errorMsg = "Invalid Entry";
                ((NumericUpDown)sender).Select(0, value.ToString().Length);
                if (SuppressBeeps == false)
                    SystemSounds.Beep.Play();
            }
            this.errorProvider1.SetError((Control)sender, errorMsg);
        }


        //Track valid entries
        private void textControl_Validated(object sender, EventArgs e)
        {
            ((Control)sender).Tag = ((Control)sender).Text;
            errorProvider1.SetError((Control)sender, "");
            picHex.Refresh();
        }


        //Revert to last entry on escape key
        private void textControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                ((Control)sender).Text = (string)((Control)sender).Tag;
                errorProvider1.SetError((Control)sender, "");
            }
        }


        //Track valid entries
        private void numControl_Validated(object sender, EventArgs e)
        {
            ((Control)sender).Tag = ((NumericUpDown)sender).Value;
            errorProvider1.SetError((Control)sender, "");
            picHex.Refresh();
        }


        //Revert to last entry on escape key
        private void numControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                ((NumericUpDown)sender).Value = (decimal)((Control)sender).Tag;
                errorProvider1.SetError((Control)sender, "");
            }
        }


        /// <summary>
        /// Validate all entries as correct.
        /// </summary>
        /// <returns>TRUE if no errors are found.</returns>
        //Stolen from https://stackoverflow.com/questions/12323044/c-sharp-errorprovider-want-to-know-if-any-are-active
        private bool allFieldsValidated()
        {
            foreach (Control c in errorProvider1.ContainerControl.Controls)
                if (errorProvider1.GetError(c) != "")
                    return false;
            return true;
        }








        #endregion

        
    }
}
