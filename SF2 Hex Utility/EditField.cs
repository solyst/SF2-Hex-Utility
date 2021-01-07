using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SF2_Hex_Utility
{
    public partial class EditField : UserControl
    {
        const int MAX_LINE_LENGTH = 16;
        const string UNKNOWN_VALUE = "?";
        private readonly Color COLOR_SELECT = SystemColors.ControlDark;
        private readonly Color COLOR_HOVER = SystemColors.ControlLight;
        private readonly Color COLOR_NORMAL = SystemColors.Control;
        private readonly Color COLOR_SELECT_HOVER = SystemColors.ControlDark;

        static public bool SuppressBeeps { get; set; } = false;

        public int RandomNumber { get; set; }

        private HexLocation hexLocation;
        public HexLocation HexLocation
        {
            get { return hexLocation; }
            set
            {
                //unsubscribe always, if it's not null 
                if (hexLocation != null)
                    hexLocation.ValueChanged -= hexLocation_ValueChanged;

                //assign value 
                hexLocation = value;

                //subscribe again, if it's not null
                if (hexLocation != null)
                    hexLocation.ValueChanged += hexLocation_ValueChanged;

                //raise event
                hexLocation_ValueChanged(this, EventArgs.Empty);
            }
        }

        public byte[] Value { get; set; }
        public bool Valid { get; set; }
        public bool Dirty { get; set; } //not used
        public Rom Rom { get; }

        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (value == true)
                    Select();
                else
                    Deselect();
            }
        }

        private byte[] oldValue;


        public EditField(HexLocation hexLocation, Rom rom)
        {
            Random rng = new Random();
            RandomNumber = rng.Next(1000);

            InitializeComponent();

            //Assign the hexlocation directly to the private value so that the ValueChanged event is not raised upon creation of the EditField
            this.hexLocation = hexLocation;
            HexLocation.ValueChanged += hexLocation_ValueChanged;

            this.Rom = rom;
            Rom.RomOpened += rom_Opened;
            //Rom.RomEdited += rom_Edited; //not used because there would be an endless loop of events from the value changing, then the rom, then the value, etc.

            setValueFromRom(); //this also updates the original value
            UpdateAllVisuals();

            //Validation
            txtValue.Validating += editField_Validating;
            txtValue.Validated += editField_Validated;

            //Selection when tabbing through fields
            txtValue.GotFocus += EditField_GotFocus;

            //Allows for escape to stop editing if there is an error.
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(Form))
                    c.MouseUp += EditFieldInert_MouseUp;
                else
                    c.MouseUp += EditFieldInteractive_MouseUp;
                c.KeyDown += EditField_KeyDown;
                c.MouseEnter += EditField_MouseEnter;
                c.MouseLeave += EditField_MouseLeave;
            }
        }


        /// <summary>
        /// Update all visuals and reload the rom information if the Hex Location changes. A HexLocationChanged event will be raised.
        /// The rom load can raise a ValueChanged event, if the value changed.
        /// </summary>
        private void hexLocation_ValueChanged(object sender, EventArgs e)
        {
            setValueFromRom();
            UpdateAllVisuals();
            OnHexLocationChanged(EventArgs.Empty);
        }


        //------------------------------------------------------------------
        // Event Summary
        //------------------------------------------------------------------
        #region Event_Summary
        /// <raises>
        ///     ValueChanged = when setValueFromRom (only if the value changed), txtValue_TextChanged, EditField_Validated, HexLocation is changed, and when the Escape key is pressed in txtValue; not raised during object initialization
        ///     SelectionChanged = when Select or Deselect are called (unless events are suppressed)
        ///     HexLocationChanged = when hexLocation_ValueChanged is raised, when the property HexLocation is changed; not raised upon object initialization
        /// </raises>
        /// <listens>
        ///     HexLocation.ValueChanged = update the visuals for the edit field
        ///     Rom.Opened = set value from rom
        /// </listens>
        #endregion



        //---------------------------------------------------------
        // Update Visuals Functions
        //---------------------------------------------------------
        #region UpdateVisuals_Functions

        public void UpdateAllVisuals()
        {
            UpdateFormatDisplay();
            sizeForm();
            UpdateName();
        }


        /// <summary>
        /// Update the name label with the value from the Hex Location.
        /// </summary>
        public void UpdateName()
        {
            lblName.Text = HexLocation.Name;
        }


        /// <summary>
        /// Update the screen visual for the format of the input (hex, string, int, etc.)
        /// </summary>
        public void UpdateFormatDisplay()
        {
            switch (HexLocation.Format)
            {
                case DataFormat.Hexadecimal:
                    lblFormat.Text = "HEX";
                    lblFormat.BackColor = Color.FromArgb(200, Color.FromArgb(0xF5793A)); //you can't do Color.FromArgb(0xFF85C0F9) because 0xFF85C0F9 is greater than int in size
                    //lblFormat.BackColor = Color.FromArgb(0x79F5793A); //max transparency value is 79ish
                    break;
                case DataFormat.String:
                    lblFormat.Text = "TEXT";
                    lblFormat.BackColor = Color.FromArgb(0x78A95AA1);
                    break;
                case DataFormat.SignedInt:
                    lblFormat.Text = "INT";
                    lblFormat.BackColor = Color.FromArgb(210, Color.FromArgb(0x85C0F9));
                    //lblFormat.BackColor = Color.FromArgb(0x7885C0F9);
                    break;
                case DataFormat.UnsignedInt:
                    lblFormat.Text = "UINT";
                    lblFormat.BackColor = Color.FromArgb(0x780F2080);
                    break;
                default:
                    lblFormat.Text = "???";
                    lblFormat.BackColor = Color.FromArgb(0x78000000);
                    break;
            }
        }


        //Resize the form according to the required inputs 
        private void sizeForm()
        {
            if (this.HexLocation.Length >= MAX_LINE_LENGTH)
            {
                txtValue.Multiline = true;
                txtValue.Dock = DockStyle.Fill;
                this.Height = 49;
            }
            else
            {
                txtValue.Multiline = false;
                txtValue.Dock = DockStyle.None;
                this.Height = 29;
            }
        }

        #endregion



        //---------------------------------------------------------
        // To Override Functions
        //---------------------------------------------------------
        #region To_Functions

        public override string ToString()
        {
            return HexLocation.Name;
        }


        public byte[] ToBytes()
        {
            return HexLocation.ToBytesByFormat(txtValue.Text);
        }

        #endregion



        //---------------------------------------------------------
        // Data Functions
        //---------------------------------------------------------
        #region Data_Functions

        //If RefType = WildcardMatch there can be serious slowdown loading information. So instead the information is only loaded if the EditField is selected. Otherwise it is left as an unknown.
        //Only trigger the OnValueChange event if there was an oldValue that was previously loaded (as in, only trigger the OnValueChange event if this isn't the first time the value is being set from the rom).
        private void setValueFromRom()
        {
            if (HexLocation.RefType == ReferenceType.WildcardMatch && this.selected == false)
            {
                //Value = new byte[] { 0 };
                txtValue.Text = UNKNOWN_VALUE;
                oldValue = null;
            }
            else
            {
                Value = getLocationDataFromRom();
                txtValue.Text = HexLocation.ToString(Value);
                if (oldValue != Value)
                {
                    if (oldValue != null)
                    {
                        OnValueChanged(EventArgs.Empty);
                    }
                    oldValue = (byte[])Value.Clone();
                }
            }
        }


        private void setRomDataFromValue()
        {
            int location = HexLocation.GetEditLocation(Rom);
            if (location >= 0)
            {
                txtValue.ForeColor = SystemColors.WindowText;
                Rom.Edit(location, Value);
            }
            else
            {
                txtValue.ForeColor = Color.Red;
            }
        }


        /// <summary>
        /// Return data in the rom which corresponds to Hex Location (this is the data which will be edited).
        /// </summary>
        /// <returns>Data in the rom which corresponds to Hex Location</returns>
        private byte[] getLocationDataFromRom()
        {
            return Rom.GetData(HexLocation.GetEditLocation(Rom), HexLocation.Length);
        }


        #endregion



        //---------------------------------------------------------
        // Validation Events and Methods
        //---------------------------------------------------------
        #region Validation_Events&Methods

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            setValueFromTextBox();
        }


        private void setValueFromTextBox()
        {
            if (txtValue.Text != UNKNOWN_VALUE)
            {
                Value = HexLocation.ToBytesByFormat(txtValue.Text);
            }
        }


        private void editField_Validating(object sender, CancelEventArgs e)
        {
            string errorMsg = "";
            bool valuesMatch = false;
            if (Value != null && HexLocation.Format == DataFormat.Hexadecimal)
            {
                valuesMatch = ByteConversion.ToHex(Value) == ByteConversion.CleanHexString(txtValue.Text);
            }
            else
            {
                valuesMatch = HexLocation.ToString(Value) == txtValue.Text;
            }
            if (Value == null || !HexLocation.ValidateBytes(Value) || !valuesMatch)
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                errorMsg = "Invalid Entry";
                txtValue.Select(0, txtValue.Text.Length);
                if (SuppressBeeps == false)
                    SystemSounds.Beep.Play();
                this.Valid = false;
                this.Refresh();
            }
        }


        private void editField_Validated(object sender, EventArgs e)
        {
            if (oldValue == null || !oldValue.SequenceEqual(Value))
            {
                setValueFromTextBox();
                setRomDataFromValue();
                oldValue = Value;
                this.Valid = true;
                this.Refresh();
                OnValueChanged(EventArgs.Empty);
            }
        }


        //Revert to last entry on escape key.
        //Force validation on enter key.
        private void EditField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                Value = oldValue;
                OnValueChanged(EventArgs.Empty);
                txtValue.Text = HexLocation.ToString(Value);
                this.Valid = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.Validate();
            }
            this.OnKeyDown(e);
        }


        #endregion



        //---------------------------------------------------------
        // Selection Functions
        //---------------------------------------------------------
        #region Selection_Functions


        public new void Select()
        {
            Select(false);
        }


        public void Select(bool suppressEvents = false)
        {
            if (selected == false)
            {
                this.BackColor = COLOR_SELECT;
                base.Select();
                selected = true;
                if (HexLocation.RefType == ReferenceType.WildcardMatch && txtValue.Text == UNKNOWN_VALUE)
                    setValueFromRom(); //when RefType = WildcardMatch, the data is only loaded from the rom when the EditField is selected
                if (txtValue.Focused == false)
                    txtValue.Focus();
                if (suppressEvents == false)
                    OnSelectionChanged(new EventArgs());
            }
        }


        public void Deselect(bool suppressEvents = false)
        {
            if (selected == true)
            {
                this.BackColor = COLOR_NORMAL;
                selected = false;
                if (suppressEvents == false)
                    OnSelectionChanged(new EventArgs());
            }
        }


        //When the user clicks on interactive elements of the form, select the form.
        private void EditFieldInteractive_MouseUp(object sender, MouseEventArgs e)
        {
             Select();
        }


        //When the user clicks inert elements of the form, toggle selection.
        private void EditFieldInert_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Selected)
                    Deselect();
                else
                    Select();
            }
            else
            {
                Select();
            }
        }


        private void EditField_MouseEnter(object sender, EventArgs e)
        {
            if (Selected)
                this.BackColor = COLOR_SELECT_HOVER;
            else
                this.BackColor = COLOR_HOVER;
        }


        private void EditField_MouseLeave(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                return;
            else
            {
                if (Selected)
                    this.BackColor = COLOR_SELECT;
                else
                    this.BackColor = COLOR_NORMAL;
            }
        }


        private void EditField_GotFocus(object sender, EventArgs e)
        {
            string s = ((TextBox)sender).Parent.Controls["lblName"].Text;
            int i = ((EditField)((TextBox)sender).Parent).RandomNumber;
            Random rng = new Random();
            System.Diagnostics.Debug.Print("Field Focus " + s + " " + i);
            Select();
        }


        #endregion



        //---------------------------------------------------------
        // Custom Events
        //---------------------------------------------------------
        #region Custom_Events

        public event EventHandler ValueChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler HexLocationChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }


        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }


        protected virtual void OnHexLocationChanged(EventArgs e)
        {
            HexLocationChanged?.Invoke(this, e);
        }


        #endregion



        //---------------------------------------------------------
        // Rom Events
        //---------------------------------------------------------
        #region Rom_Events

        private void rom_Opened(object sender, EventArgs e)
        {
            setValueFromRom();
        }


        #endregion

        
    }
}
