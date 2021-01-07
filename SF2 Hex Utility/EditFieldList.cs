using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SF2_Hex_Utility
{
    public partial class EditFieldList : UserControl
    {
        //Properties
        public List<EditField> Items { get; set; } = new List<EditField>();
        public bool Valid { get; set; }
        public Rom Rom { get; set; }
        public Rom UnchangedRom { get; set; }
        public string Folder { get; set; }
        public string Group { get; set; }

        private HexLocations parentHexLocations = null;
        public HexLocations ParentHexLocations
        {
            get { return parentHexLocations; }
        }


        public bool Multiselect { get; set; } = false;
        public List<EditField> SelectedItems { get { return Items.FindAll(x => x.Selected == true); } }

        private bool dirty;
        public bool Dirty
        {
            get { return dirty; }
            set
            {
                dirty = value;
                if (ParentHexLocations != null)
                    ParentHexLocations.Dirty |= value; //Ensure the parent HexLocations's Dirty property reflects the dirty property of all child Hex Locations
            }
        }


        //Constructors
        public EditFieldList()
        {
            InitializeComponent();
        }

        public EditFieldList(Rom rom, Rom unchangedRom, HexLocations parentHexLocations, string folder, string group)
        {
            InitializeComponent();
            this.Rom = rom;
            this.UnchangedRom = unchangedRom;
            SetListContents(parentHexLocations, folder, group);
        }


        public void SetListContents(HexLocations parentHexLocations, string folder, string group)
        {
            //unsubscribe always, if it's not null 
            if (ParentHexLocations != null)
                ParentHexLocations.ListHexLocationChanged -= parentHexLocations_ListHexLocationChanged;

            //assign value 
            this.parentHexLocations = parentHexLocations;

            //subscribe again, if it's not null
            if (ParentHexLocations != null)
                ParentHexLocations.ListHexLocationChanged += parentHexLocations_ListHexLocationChanged;

            //Set the path for what hex locations to include in the list
            this.Folder = folder;
            this.Group = group;

            //deselect everything and reset the scroll
            SelectNone();
            panList.AutoScrollPosition = new Point(0, 0);

            //redraw the list of Edit Fields
            updateEditFieldList();
        }



        //------------------------------------------------------------------
        // Event Summary
        //------------------------------------------------------------------
        #region Event_Summary
        /// <raises>
        ///     FieldChanged = when editField.ValueChanged is raised
        ///     FieldFocusChanged = when editField.Enter (when the mouse enters the editField bounds)
        ///     NoneSelected = when SelectionChanged is raised and no items are selected, or if the list is refreshed with no items, or if rom is null when list is refreshed, or if SelectNone is called
        ///     SelectionChanged = when editField.SelectionChanged is raised; ensures only one item in the list is selected at a time; raises the NoneSelected event if none are selected
        /// </raises>
        /// <listens>
        ///     editField.ValueChanged = when setValueFromRom (only if the value changed), txtValue_TextChanged, EditField_Validated, and when the Escape key is pressed in txtValue, raise event FieldChange
        ///     editField.Enter = when the mouse enters the editField bounds, raise event FieldFocusChanged
        ///     editField.SelectionChanged = when editField.Select or editField.Deselect are called (unless events are suppressed), raise event SelectionChanged (which ensures only one item is selected at a time)
        ///     editField.HexLocationChanged = when hexLocation_ValueChanged is raised, when the property HexLocation is changed; not raised upon object initialization; 
        /// </listens>
        #endregion



        //---------------------------------------------------------
        // List Functions
        //---------------------------------------------------------
        #region List_Functions


        private void updateEditFieldList()
        {
            Random random = new Random();
            Debug.Print("updateEditFieldList " + random.Next(100));
            if (Rom == null || ParentHexLocations == null)
            {
                panList.Hide();
                panList.Controls.Clear();
                removeAllItems();
                OnNoneSelected(EventArgs.Empty);
            }
            else
            {
                /*
                Point currectScrollLocation = this.AutoScrollPosition;
                panList.Hide();
                panList.Controls.Clear();
                this.SuspendLayout();
                HexLocation[] newItems = ParentHexLocations.GetLocations(Folder, Group);
                buildListItems(newItems);
                this.ResumeLayout();
                this.AutoScrollPosition = new Point(Math.Abs(currectScrollLocation.X), Math.Abs(currectScrollLocation.Y));
                panList.Show();
                */

                //panList.Dock = DockStyle.Fill;
                //this.AutoScroll = false;
                //panList.AutoScroll = true;
                int selectedIndex = Items.FindIndex(x => x.Selected == true);
                Point CurrentPoint = panList.AutoScrollPosition;
                panList.SuspendLayout();
                HexLocation[] newItems = ParentHexLocations.GetLocations(Folder, Group);
                buildListItems(newItems);
                if (selectedIndex >= 0 && selectedIndex < Items.Count())
                {
                    Items[selectedIndex].Select();
                }
                panList.AutoScrollPosition = new Point(Math.Abs(panList.AutoScrollPosition.X), Math.Abs(CurrentPoint.Y));
                panList.ResumeLayout();
                panList.Show();
            }
        }


        private void buildListItems(HexLocation[] items)
        {
            removeAllItems();
            foreach (HexLocation h in items)
            {
                addItem(h);
            }
            OnNoneSelected(EventArgs.Empty);
        }


        private void addItem(HexLocation hexLocation)
        {
            Rectangle rect = listBounds();
            EditField editField = new EditField(hexLocation, Rom);
            Items.Add(editField);
            panList.Controls.Add(editField);
            editField.Enter += editField_Enter;
            editField.ValueChanged += editField_ValueChanged;
            editField.SelectionChanged += editField_SelectionChanged;
            editField.KeyDown += editField_KeyDown;
            editField.ContextMenuStrip = contextMenuStripEditField;
            editField.Location = new Point(rect.X, rect.Bottom);
            //editField.Dock = DockStyle.Top;
            //editField.BringToFront();
            //sizeList();
        }


        //This safetly removes all items.
        private void removeAllItems()
        {
            panList.Controls.Clear();
            if (Items != null)
            {
                foreach (EditField f in Items)
                {
                    f.Enter -= editField_Enter;
                    f.ValueChanged -= editField_ValueChanged;
                    f.SelectionChanged -= editField_SelectionChanged;
                    f.KeyDown -= editField_KeyDown;
                    f.Dispose();
                }
            }
            Items = new List<EditField>();
        }


        //Return the drawing rectangle of the list boundary
        private Rectangle listBounds()
        {
            int top = 0;
            int left = 0;
            int width = 0;
            int height = 0;

            if (Items.Count > 0)
            {
                width = Items.Max(x => x.Width);
                height = Items.Sum(x => x.Height); 
            }

            return new Rectangle(top, left, width, height);
        }


        private void sizeList()
        {
            Rectangle rect = listBounds();
            panList.Location = new Point(rect.X, rect.Y);
            panList.Width = rect.Width;
            panList.Height = rect.Height;
        }


        /// <summary>
        /// Deselect all items in the list.
        /// </summary>
        public void SelectNone()
        {
            if (Items != null)
            {
                foreach (EditField f in Items)
                {
                    f.Deselect();
                }
            }
            OnNoneSelected(EventArgs.Empty);
        }


        #endregion



        //---------------------------------------------------------
        // Right Click Menu Functions
        //---------------------------------------------------------
        #region RightClickMenu_Functions

        private void tsmContextEdit_Click(object sender, EventArgs e)
        {
            if (this.SelectedItems.Count > 0)
            {
                EditField editField = this.SelectedItems[0];
                using (EditHexLocation editHexLocation = new EditHexLocation(editField.HexLocation, ParentHexLocations, UnchangedRom))
                {
                    if (editHexLocation.ShowDialog() == DialogResult.OK)
                    {
                        editField.HexLocation = editHexLocation.newHexLocation;
                        FieldChangedEventArgs fieldChangedEventArgs = new FieldChangedEventArgs() { EditField = editField };
                        OnFieldChange(fieldChangedEventArgs);
                    }
                }
            }
        }


        private void tsmContextDuplicate_Click(object sender, EventArgs e)
        {
            EditField editField = this.SelectedItems[0];
            using (EditHexLocation editHexLocation = new EditHexLocation(editField.HexLocation.Clone(), ParentHexLocations, UnchangedRom))
            {
                if (editHexLocation.ShowDialog() == DialogResult.OK)
                {
                    FieldChangedEventArgs fieldChangedEventArgs = new FieldChangedEventArgs() { EditField = editField };
                    OnFieldFocusChanged(fieldChangedEventArgs);
                }
            }
        }

        #endregion  



        //---------------------------------------------------------
        // Custom Events
        //---------------------------------------------------------
        #region Custom_Events

        public event EventHandler<FieldChangedEventArgs> FieldChanged;
        public event EventHandler<FieldChangedEventArgs> FieldFocusChanged;
        public event EventHandler<EventArgs> NoneSelected;
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        protected virtual void OnFieldChange(FieldChangedEventArgs e)
        {
            FieldChanged?.Invoke(this, e);
        }


        protected virtual void OnFieldFocusChanged(FieldChangedEventArgs e)
        {
            FieldFocusChanged?.Invoke(this, e);
        }


        protected virtual void OnNoneSelected(EventArgs e)
        {
            NoneSelected?.Invoke(this, e);
        }


        /// <summary>
        /// Raise the event that the selected status for a EditField has changed.
        /// If an EditField was selected and multiselect is false, deselect all other EditFields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanged(EditField sender)
        {
            //Deselect other items if necessary.
            if (Multiselect == false)
            {
                if (sender.Selected == true)
                {
                    foreach (EditField f in Items)
                    {
                        if (!f.Equals(sender))
                        {
                            f.Deselect(true);
                        }
                    }
                }
            }

            //Check if all items are deselected.
            bool anySelected = false;
            foreach (EditField f in Items)
            {
                if (f.Selected)
                {
                    anySelected = true;
                    break;
                }
            }
            if (anySelected == false)
                OnNoneSelected(EventArgs.Empty);

            //Raise event.
            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this.SelectedItems));
        }


        public class FieldChangedEventArgs : EventArgs
        {
            public EditField EditField { get; set; }
        }


        public class SelectionChangedEventArgs : EventArgs
        {
            public List<EditField> SelectedItems { get; set; }
            public SelectionChangedEventArgs(List<EditField> selectedItems)
            {
                SelectedItems = selectedItems;
            }
        }


        #endregion



        //---------------------------------------------------------
        // EditField Events
        //---------------------------------------------------------
        #region EditField_Events

        private void editField_ValueChanged(object sender, EventArgs e)
        {
            FieldChangedEventArgs fieldChangedEventArgs = new FieldChangedEventArgs() { EditField = (EditField)sender };
            OnFieldChange(fieldChangedEventArgs);
        }


        private void editField_Enter(object sender, EventArgs e)
        {
            //FieldChangedEventArgs fieldChangedEventArgs = new FieldChangedEventArgs() { EditField = (EditField)sender };
            //OnFieldFocusChanged(fieldChangedEventArgs);
        }


        private void editField_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged((EditField)sender);
        }


        /// <summary>
        /// If the contents of the parentHexLocations changes, redraw the list of Edit Fields.
        /// </summary>
        private void parentHexLocations_ListHexLocationChanged(object sender, EventArgs e)
        {
           Debug.Print("parentHexLocations_ListHexLocationChanged");
           updateEditFieldList();
        }


        /// <summary>
        /// If the down key is pressed, select the next item in the list. If the up key is pressed, select the previous item in the list. Will wrap around the list.
        /// </summary>
        private void editField_KeyDown(object sender, KeyEventArgs e)
        {
            int n = 0;
            while (n < Items.Count() && Items[n].Selected == false)
            {
                n++;
            }
            if (n < Items.Count()) //only check for key presses if an EditField is currently selected
            {
                if (e.KeyCode == Keys.Down)
                {
                    n++;
                    if (n >= Items.Count())
                    {
                        n = 0;
                    }
                    Items[n].Select();
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.Up)
                {
                    n--;
                    if (n < 0)
                    {
                        n = Items.Count() - 1;
                    }
                    Items[n].Select();
                    e.SuppressKeyPress = true;
                }
            }
        }


        #endregion



    }
}
