using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SF2_Hex_Utility
{
    public enum DataFormat
    {
        UnsignedInt,
        SignedInt,
        String,
        Hexadecimal
    }

    public enum ReferenceType
    {
        Absolute,
        WildcardMatch
    }

    public class HexLocation
    {
        //Const for import/export
        const int FIELD_COUNT = 20;             //number of fields which are exported/imported, including the Description field
        const char DELIMITER = '\t';            //for constructing import/export strings
        const string NEW_LINE = @"\r\n";       //for saving and loading the Description from a file


        //Parameters
        private string folder = "";                  //folder which can be collapsed or expanded in the view
        public string Folder
        {
            get { return folder; }
            set { folder = value.Replace(DELIMITER, ' ').Substring(0, Math.Min(30, value.Length)); }
        }

        private string group = "";                  //line item in the folder
        public string Group
        {
            get { return group; }
            set { group = value.Replace(DELIMITER, ' ').Substring(0, Math.Min(30, value.Length)); }
        }

        private string name = "";                    //display name of the field
        public string Name
        {
            get { return name; }
            set { name = value.Replace(DELIMITER, ' ').Substring(0, Math.Min(60, value.Length)); }
        }

        private byte[] bytes = new byte[0];         //value at the hex location (or the original value, if this is the template)
        public byte[] Bytes
        {
            get { return bytes; }
            set { bytes = FixByteLength(value); }
        }

        public byte[] Context { get; set; } = new byte[0];      //the rest of the hex on the row where the data is being edited (displayed so the user can determine if there may be an error)

        private int byteOffsetInContext;                        //how many bytes into the context are the normal bytes normally occurring/inserted?
        public int ByteOffsetInContext
        {
            get { return byteOffsetInContext; }
            set { byteOffsetInContext = Math.Min(Context.Length, Math.Max(0, value)); } //for positive numbers only, with the option for the bytes to occur after the context, but no later
        }

        private int location;                                   //the first byte to read/write
        public int Location
        {
            get { return location; }
            set { location = Math.Min(int.MaxValue, Math.Max(0, value)); } //force positive numbers, limit to a max of 7F FF FF FF
        }

        private int length = 1;                                 //number of bytes to change (this forces the bytes to be of this size)
        public int Length
        {
            get { return length; }
            set { length = Math.Max(1, value); } //force positive numbers
        }

        public ReferenceType RefType { get; set; } //if Location is an absolute reference, or a pointer that points to the location
        public DataFormat Format { get; set; }  //how the hex data should be displayed to the user
        public string Description { get; set; } //description of the data

        public int ContextLocation { get; set; } //Always use GetContextLocation to reference the Context Location, as it will calculate the appropriate location when possible.
        public byte?[] SearchBytes { get; set; }


        //Default constructor
        public HexLocation() { }

        /// <summary>
        /// Convert a text string into a Hex Location object.
        /// </summary>
        /// <param name="importString"></param>
        public HexLocation(string importString)
        {
            ParseImportString(importString);
        }


        /// <summary>
        /// Return a duplicate of the current instance of this object
        /// </summary>
        /// <returns></returns>
        public HexLocation Clone()
        {
            HexLocation h = new HexLocation();
            h.Folder = this.folder;
            h.Group = this.group;
            h.Name = this.name;
            h.Location = this.Location;
            h.Length = this.Length;
            h.RefType = this.RefType;
            h.Format = this.Format;
            h.Bytes = this.bytes;
            h.Context = this.Context;
            h.ByteOffsetInContext = this.ByteOffsetInContext;
            h.Description = this.Description;
            h.SearchBytes = this.SearchBytes;
            h.ContextLocation = this.ContextLocation;

            return h;
        }


        //------------------------------------------------------------------
        // Event Summary
        //------------------------------------------------------------------
        #region Event_Summary
        /// <raises>
        ///     ValueChanged = only when RaiseEventHexLocationValueChanged is called (which should happen by the EditHexLocation form
        /// </raises>
        /// <listens>
        ///     None
        /// </listens>
        #endregion



        //------------------------------------------------------------------
        // Enum Stuff
        //------------------------------------------------------------------
        #region EnumStuff

        //Return text version of the enum
        static public string[] GetDataFormatNames()
        {
            return Enum.GetNames(typeof(DataFormat));
        }


        //Return text version of the enum
        static public string[] GetReferenceTypeNames()
        {
            return Enum.GetNames(typeof(ReferenceType));
        }


        #endregion



        //------------------------------------------------------------------
        // Location Functions
        //------------------------------------------------------------------
        #region Location_Functions

        public int GetEditLocation()
        {
            return Location;
        }


        /// <summary>
        /// Return the location to edit in the rom. When using a WildcardMatch, this returns the location of the first wildcard character within the match, or the location following the match if there is no wildcard.
        /// </summary>
        /// <param name="rom">The rom being searched for a match. Only necessary for WildcardMatch.</param>
        /// <returns>The location within in the rom to edit.</returns>
        public int GetEditLocation(Rom rom)
        {
            switch (RefType)
            {
                case ReferenceType.Absolute:
                    return Location;
                case ReferenceType.WildcardMatch:
                    int n = 0;
                    while (n < SearchBytes.Length && SearchBytes[n] != null)
                    {
                        n++;
                    }
                    return rom.Find(SearchBytes, location) + n;
                default:
                    return location;
            }
        }


        public int GetEditLocation(Rom rom, out bool isDuplicate)
        {
            isDuplicate = false;
            switch (RefType)
            {
                case ReferenceType.WildcardMatch:
                    int location1 = GetEditLocation(rom);
                    int location2 = rom.Find(SearchBytes, location1 + 1);
                    isDuplicate = (location2 != -1);
                    return location1;
                default:
                    return GetEditLocation(rom);
            }
        }
              

        #endregion



        //------------------------------------------------------------------
        // Validation Functions
        //------------------------------------------------------------------
        #region Validation_Functions

        //Validate Folder
        public bool ValidateFolder(string value)
        {
            string old = this.Folder;
            try
            {
                this.Folder = value;
                this.Folder = old;
                return true;
            }
            catch (Exception)
            {
                this.Folder = old;
                return false;
            }
        }


        //Validate Group
        public bool ValidateGroup(string value)
        {
            string old = this.Group;
            try
            {
                this.Group = value;
                this.Group = old;
                return true;
            }
            catch (Exception)
            {
                this.Group = old;
                return false;
            }
        }


        //Validate Name
        public bool ValidateName(string value)
        {
            string old = this.Name;
            try
            {
                this.Name = value;
                this.Name = old;
                return true;
            }
            catch (Exception)
            {
                this.Name = old;
                return false;
            }
        }


        //Validate Bytes
        public bool ValidateBytes(byte[] bytes)
        {
            return bytes.Length == Length;
        }


        //Validate Hex that can be used for Context or Bytes
        public bool ValidateHex(string hexString, out byte[] output)
        {
            byte[] check = ByteConversion.ToBytesFromHex(hexString);
            output = check;
            return (!(check.Length == 1 && check[0] == 0) || ByteConversion.CleanHexString(hexString) == "00");
        }


        //Validate Context
        public bool ValidateContext(byte[] context)
        {
            return (context.Length < 128000);
        }


        //Validate ByteOffsetInContext
        public bool ValidateByteOffsetInContext(int value)
        {
            int old = this.ByteOffsetInContext;
            try
            {
                this.ByteOffsetInContext = value;
                this.ByteOffsetInContext = old;
                return true;
            }
            catch (Exception)
            {
                this.ByteOffsetInContext = old;
                return false;
            }
        }


        //Validate Location
        public bool ValidateLocation(int value)
        {
            int old = this.Location;
            try
            {
                this.Location = value;
                this.Location = old;
                return true;
            }
            catch (Exception)
            {
                this.Location = old;
                return false;
            }
        }


        //Validate Length
        public bool ValidateLength(int value)
        {
            int old = this.Length;
            try
            {
                this.Length = value;
                this.Length = old;
                return true;
            }
            catch (Exception)
            {
                this.Length = old;
                return false;
            }
        }


        //Validate ReferenceType
        public bool ValidateReferenceType(string value)
        {
            return Enum.TryParse<ReferenceType>(value, out ReferenceType refType);
        }


        //Validate Data Format
        public bool ValidateFormat(string value)
        {
            return Enum.TryParse<DataFormat>(value, out DataFormat format);
        }


        //Validate Description
        public bool ValidateDescription(string value)
        {
            string old = this.Description;
            try
            {
                this.Description = value;
                this.Description = old;
                return true;
            }
            catch (Exception)
            {
                this.Description = old;
                return false;
            }
        }


        //Validate SearchBytes
        public bool ValidateSearchBytes(byte?[] value)
        {
            byte?[] old = this.SearchBytes;
            try
            {
                this.SearchBytes = value;
                this.SearchBytes = old;
                return true;
            }
            catch (Exception)
            {
                this.SearchBytes = old;
                return false;
            }
        }


        #endregion



        //------------------------------------------------------------------
        // Byte Conversions
        //------------------------------------------------------------------
        // This simplifies the data entry fields to edit the various bytes by referencing the DataFormat before calling the appropriate ByteConversion function.
        // So if one edits the starting gold amount, for example, they can type in normal numbers and not hex and it can still be validated.
        #region ByteConversions


        /// <summary>
        /// Set the byte length of value to the amount specified in the HexLocation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A byte series of appropriate length.</returns>
        public byte[] FixByteLength(byte[] value)
        {
            byte[] returnBytes;
            switch (Format)
            {
                case DataFormat.String:
                    returnBytes = ByteConversion.SetASCIIByteLength(value, length);
                    break;
                case DataFormat.UnsignedInt:
                    returnBytes = ByteConversion.SetUIntByteLength(value, length);
                    break;
                case DataFormat.SignedInt:
                    returnBytes = ByteConversion.SetIntByteLength(value, length);
                    break;
                default:
                    returnBytes = ByteConversion.SetHexByteLength(value, length);
                    break;
            }
            return returnBytes;
        }


        /// <summary>
        /// Convert the provide string value into bytes.
        /// </summary>
        /// <param name="value">String representing ASCII characters, an integer, or hex values.</param>
        /// <returns>Bytes based upon the text string.</returns>
        public byte[] ToBytesByFormat(string value)
        {
            byte[] returnBytes = new byte[0];
            switch (Format)
            {
                case DataFormat.String:
                    returnBytes = ByteConversion.ToBytes(value, length);
                    break;
                case DataFormat.UnsignedInt:
                    if (UInt64.TryParse(value, out UInt64 result))
                        returnBytes = ByteConversion.ToBytes(result, length);
                    break;
                case DataFormat.SignedInt:
                    if (Int64.TryParse(value, out Int64 result2))
                        returnBytes = ByteConversion.ToBytes(result2, length);
                    break;
                default:
                    returnBytes = ByteConversion.ToBytesFromHex(value, length);
                    break;
            }
            return returnBytes;
        }


        /// <summary>
        /// Return the byte value into a string representation based upon the Hex Location format.
        /// </summary>
        /// <param name="value">Bytes to convert into a string.</param>
        /// <returns>String representation of the bytes based upon the Hex Location format.</returns>
        public string ToString(byte[] value)
        {
            string returnString;
            if (value == null)
            {
                returnString = "";
            }
            else
            {
                switch (Format)
                {
                    case DataFormat.String:
                        returnString = ByteConversion.ToASCIIString(value);
                        break;
                    case DataFormat.UnsignedInt:
                        returnString = ByteConversion.ToUInt(value).ToString();
                        break;
                    case DataFormat.SignedInt:
                        returnString = ByteConversion.ToInt(value).ToString();
                        break;
                    default:
                        returnString = HexRenderer.BytesToHexString(value, false, 0); //ByteConversion.ToHex(value);
                        break;
                }
            }
            return returnString;
        }


        #endregion


        
        //------------------------------------------------------------------
        // Import and Export functions
        //------------------------------------------------------------------
        #region Import&Export_Functions

        public void ParseImportString(string importString)
        {
            try
            {
                char[] separators = new char[] { DELIMITER };
                string[] splitString = importString.Split(separators, FIELD_COUNT - 1);
                Location = int.Parse(splitString[6]);
                Length = int.Parse(splitString[7]); 
                Folder = splitString[0];
                Group = splitString[1];
                Name = splitString[2];
                Bytes = ByteConversion.ToBytesFromHex(splitString[3]);
                Context = ByteConversion.ToBytesFromHex(splitString[4]);
                ByteOffsetInContext = int.Parse(splitString[5]);
                RefType = (ReferenceType)Enum.Parse(typeof(ReferenceType), splitString[8]);
                Format = (DataFormat)Enum.Parse(typeof(DataFormat), splitString[9]);
                SearchBytes = ByteConversion.ToBytesFromHexWildcard(splitString[10]);
                ContextLocation = (splitString[11] == "") ? 0 : int.Parse(splitString[11]);

                int i = getNthIndex(importString, DELIMITER, FIELD_COUNT - 1);
                Description = importString.Substring(i + 1).Replace(NEW_LINE, Environment.NewLine); //Since I am making the text file, I know this is always the code
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Format all fields into the appropriate string for import/export.
        /// </summary>
        /// <returns>The export string.</returns>
        public string GetExportString()
        {
            string[] exportString = new string[FIELD_COUNT];

            exportString[0] = folder;
            exportString[1] = group;
            exportString[2] = name;
            exportString[3] = ByteConversion.ToHex(Bytes);
            exportString[4] = ByteConversion.ToHex(Context);
            exportString[5] = ByteOffsetInContext.ToString();
            exportString[6] = Location.ToString();
            exportString[7] = Length.ToString();
            exportString[8] = RefType.ToString();
            exportString[9] = Format.ToString();
            exportString[10] = ByteConversion.ToHexWildcard(SearchBytes);
            exportString[11] = GetContextLocation(null).ToString();
            exportString[FIELD_COUNT - 1] = Description == null ? "" : Description.Replace(Environment.NewLine, NEW_LINE).Replace("\r","").Replace("\n","");

            StringBuilder sb = new StringBuilder();
            sb.Append(exportString[0]);
            for (int i = 1; i < FIELD_COUNT; i++)
            {
                sb.Append(DELIMITER);
                sb.Append(exportString[i]);
            }

            return sb.ToString();
        }


        /// stolen from https://stackoverflow.com/questions/2571716/find-nth-occurrence-of-a-character-in-a-string
        /// <summary>
        /// Returns the location of the nth occurance of a character.
        /// </summary>
        /// <param name="s">string to search</param>
        /// <param name="value">which character to find</param>
        /// <param name="index">which occurance of the character to return from the string</param>
        /// <returns></returns>
        private int getNthIndex(string s, char value, int index)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == value)
                {
                    count++;
                    if (count == index)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        #endregion


        
        //------------------------------------------------------------------
        // Set Functions (these also run a validation step)
        //------------------------------------------------------------------
        #region Set_Functions

        //Set Folder
        public bool SetFolder(string value)
        {
            if (ValidateFolder(value))
            {
                this.Folder = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        //Set Group
        public bool SetGroup(string value)
        {
            if (ValidateGroup(value))
            {
                this.Group = value;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Name
        public bool SetName(string value)
        {
            if (ValidateName(value))
            {
                this.Name = value;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Bytes
        public bool SetBytes(byte[] value)
        {
            if (ValidateBytes(value))
            {
                this.bytes = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetBytesFromHex(string hex)
        {
            switch (Format)
            {
                case DataFormat.SignedInt:
                    return SetBytesFromHex(hex, true);
                case DataFormat.UnsignedInt:
                    return SetBytesFromHex(hex, true);
                default:
                    return SetBytesFromHex(hex, false);
            }
        }
        public bool SetBytesFromHex(string hex, bool preserveRightMostBytes)
        {
            if (ValidateHex(hex, out byte[] bytes))
            {
                SetBytes(ByteConversion.SetHexByteLength(bytes, Length, preserveRightMostBytes));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetBytes(string value)
        {
            return SetBytes(ByteConversion.ToBytes(value, Length));
        }
        public bool SetBytes(Int64 value)
        {
            return SetBytes(ByteConversion.ToBytes(value, Length));
        }
        public bool SetBytes(UInt64 value)
        {
            return SetBytes(ByteConversion.ToBytes(value, Length));
        }


        //Set Context
        public bool SetContext(byte[] value)
        {
            if (ValidateContext(value))
            {
                this.Context = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetContextFromHex(string hex)
        {
            if (ValidateHex(hex, out byte[] bytes))
            {
                SetContext(bytes);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetContextFromRom(Rom rom)
        {
            int contextLocation;
            byte[] value = FindContextFromRom(rom, out contextLocation);
            if (SetContext(value) == true)
            {
                this.ByteOffsetInContext = Location - contextLocation;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set ByteOffsetInContext
        public bool SetByteOffsetInContext(int value)
        {
            if (ValidateByteOffsetInContext(value))
            {
                this.ByteOffsetInContext = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetByteOffsetInContext(string originalHexValues)
        {
            int value = FindByteOffsetInContext(originalHexValues);
            return SetByteOffsetInContext(value);
        }
        public bool SetByteOffsetInContext(byte[] originalBytes)
        {
            int value = FindByteOffsetInContext(originalBytes);
            return SetByteOffsetInContext(value);
        }


        //Set Location
        public bool SetLocation(int value)
        {
            if (ValidateLocation(value))
            {
                this.Location = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetLocationFromHex(string hex)
        {
            if (ValidateHex(hex, out byte[] bytes))
            {
                SetLocation((int)ByteConversion.ToUInt(ByteConversion.SetUIntByteLength(bytes, 4)));
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Length
        public bool SetLength(int value)
        {
            if (ValidateLength(value))
            {
                this.Length = value;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set ReferenceType
        public bool SetReferenceType(string value)
        {
            if (Enum.TryParse<ReferenceType>(value, out ReferenceType refType))
            {
                this.RefType = refType;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Data Format
        public bool SetFormat(string value)
        {
            if (Enum.TryParse<DataFormat>(value, out DataFormat format))
            {
                this.Format = format;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Description
        public bool SetDescription(string value)
        {
            if (ValidateDescription(value))
            {
                this.Description = value;
                return true;
            }
            else
            {
                return false;
            }
        }


        //Set Search Bytes
        public bool SetSearchBytes(byte?[] value)
        {
            if (ValidateSearchBytes(value))
            {
                this.SearchBytes = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SetSearchBytesFromHex(string hex)
        {
            return SetSearchBytes(ByteConversion.ToBytesFromHexWildcard(hex));
        }


        #endregion



        //------------------------------------------------------------------
        // Context functions
        //------------------------------------------------------------------
        #region Context_Functions

        /// <summary>
        /// Calculate the context location based upon the byte location and the byteOffsetInContext. If a calculation is not possible, return the stored value for Context Location.
        /// Generally, the value should be calculated when the Hex Location is being edited, and the stored value should be used when the Rom is being edited.
        /// </summary>
        /// <param name="rom">Used when the RefType = WildcardMatch.</param>
        /// <returns>The location of the provided context.</returns>
        public int GetContextLocation(Rom rom)
        {
            int returnValue;
            switch (RefType)
            {
                case ReferenceType.Absolute:
                    returnValue = Location - ByteOffsetInContext;
                    break;
                case ReferenceType.WildcardMatch:
                    if (rom == null)
                    {
                        returnValue = ContextLocation;
                    }
                    else
                    {
                        returnValue = GetEditLocation(rom) - ByteOffsetInContext;
                    }
                    break;
                default:
                    returnValue = Location - ByteOffsetInContext;
                    break;
            }
            return Math.Max(0, returnValue);
        }


        /// <summary>
        /// Find the matching sequence of originalHexValues in Context.
        /// </summary>
        /// <param name="originalHexValues">Hex to find.</param>
        /// <param name="startIndex">Start searching at this point in the Context. The return value will always return a value comapred to the full Context.</param>
        /// <returns>Location in context of the found bytes, or -1 if no match is found.</returns>
        public int FindByteOffsetInContext(string originalHexValues, int startIndex = 0)
        {
            return FindByteOffsetInContext(ByteConversion.ToBytesFromHex(originalHexValues), startIndex);
        }


        /// <summary>
        /// Find the matching sequence of originalBytes in Context.
        /// </summary>
        /// <param name="originalBytes">Bytes to find.</param>
        /// <param name="startIndex">Start searching at this point in the Context. The return value will always return a value comapred to the full Context.</param>
        /// <returns>Location in context of the found bytes, or -1 if no match is found.</returns>
        public int FindByteOffsetInContext(byte[] originalBytes, int startIndex = 0)
        {
            int matchIndex = FindIndexOfByteSeries(originalBytes, Context.Skip(startIndex).ToArray());
            if (matchIndex == -1)
                return -1;
            else
                return startIndex + matchIndex;
        }


        /// <summary>
        /// Return the starting index of bytesToFind in bytesToSearch. All of bytesToFind must match in sequence.
        /// </summary>
        /// <param name="bytesToFind">Series of bytes to find within bytesToSearch.</param>
        /// <param name="bytesToSearch">Bytes to search for bytesToFind.</param>
        /// <returns></returns>
        static public int FindIndexOfByteSeries(byte[] bytesToFind, byte[] bytesToSearch)
        {
            int index = -1;
            for (int i = 0; i < bytesToSearch.Length - bytesToFind.Length + 1; i++)
            {
                index = i;
                for (int n = 0; n < bytesToFind.Length; n++)
                {
                    if (bytesToFind[n] != bytesToSearch[i + n])
                    {
                        index = -1;
                        break;
                    }
                }
                if (index != -1)
                {
                    break;
                }
            }
            return index;
        }


        /// <summary>
        /// Return at least three lines of context from a rom based upon the Location and Length of the Hex Location data.
        /// </summary>
        /// <param name="rom">Rom from which to derive the context.</param>
        /// <returns>Auto generated context bytes.</returns>
        public byte[] FindContextFromRom(Rom rom, out int contextLocation)
        {
            int startByte = Math.Max(0, HexRenderer.FindFirstByte(GetEditLocation(rom)) - HexRenderer.BytesPerLine);
            int length = ((HexRenderer.BytesPerLine * 3 + Length) / HexRenderer.BytesPerLine) * HexRenderer.BytesPerLine;
            contextLocation = startByte;
            return rom.GetData(startByte, length);
        }

        #endregion



        //------------------------------------------------------------------
        // Custom Events
        //------------------------------------------------------------------
        #region Custom_Events

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }


        /// <summary>
        /// This should be called after every major update to the Hex Location.
        /// It is set as a separate script to allow for multiple updates to the Hex Location before the event triggers.
        /// </summary>
        public void RaiseEventHexLocationValueChanged()
        {
            OnValueChanged(EventArgs.Empty);
        }


        #endregion



    }
}
