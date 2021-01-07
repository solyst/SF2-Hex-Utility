using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public class Rom
    {
        public byte[] Data { get; set; }
        private bool dirty;
        public bool Dirty { get { return dirty; } }

        private string filePath; //keep private so that I always know the filePath == the actual file
        public string FilePath { get { return filePath; } }
        public string Name { get { return Path.GetFileName(filePath); } }


        /// <summary>
        /// Return the specified bytes.
        /// </summary>
        /// <param name="location">Starting index for the location of the bytes to return.</param>
        /// <param name="length">The number of bytes to return.</param>
        /// <returns>Returns a single byte of 0 if there is an error. Otherwise, returns the specified bytes.</returns>
        public byte[] GetData(int location, int length)
        {
            if (location + length > Data.Length)
                return new byte[] { 0 };
            else
                return Data.Skip(location).Take(length).ToArray();
        }


        /// <summary>
        /// Find the location matching the provided searchBytes, where a NULL entry is a wildcard and can be matched on any value.
        /// </summary>
        /// <param name="searchBytes">Bytes to find in the rom. A NULL byte is a wildcard and can be matched on any value.</param>
        /// <param name="startLocation">Start searching at this byte location within the data.</param>
        /// <returns></returns>
        
        public int Find(byte?[] searchBytes, int startLocation = 0)
        {
            //Create BoyerMoore table
            BoyerMoore boyerMoore = new BoyerMoore(searchBytes);
            int len = searchBytes.Length - 1;
            int limit = Data.Length - searchBytes.Length + 1;
            for (int i = startLocation; i < limit; )
            {
                //if (i >= 0xB2440 && i < 0xB2460)
                //    Debugger.Break();
                int n = len;
                for ( ; n >= 0; n--)
                {
                    if (searchBytes[n] != null && searchBytes[n] != Data[i + n])
                    {
                        i += boyerMoore.Table[n, Data[i + n]];
                        break;
                    }
                }
                if (n == -1)
                {
                    return i;
                }
            }
            return -1;
        }
        


        /// <summary>
        /// Reload rom data directly from the filepath without saving. This effectively overwrites all changes.
        /// </summary>
        /// <returns>TRUE if the reload was successful. FALSE otherwise.</returns>
        public bool Reload()
        {
            return Open(filePath);
        }

        /// <summary>
        /// Edit the specified byets.
        /// </summary>
        /// <param name="location">Starting index for the location of the bytes to edit.</param>
        /// <param name="newValue">The new bytes.</param>
        /// <returns>TRUE if the edit was successful. FALSE otherwise.</returns>
        public bool Edit(int location, byte[] newValue)
        {
            if (location + newValue.Length > Data.Length)
                return false;

            for (int i = 0; i < newValue.Length; i++)
            {
                Data[location + i] = newValue[i];
            }
            dirty = true;
            OnRomEdited(EventArgs.Empty);
            return true;
        }


        /// <summary>
        /// Read the file and load into memory. Updates the FilePath to the open file.
        /// </summary>
        /// <param name="file">File path to read.</param>
        /// <returns>TRUE if the file successfully loaded.</returns>
        public bool Open(string file)
        {
            bool error = false;
            try
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length > Math.Pow(2,24))
                    throw new InvalidOperationException("Rom can only be 16MB or less in size.");
                byte[] newBytes = File.ReadAllBytes(file);
                Data = newBytes;
                filePath = file;
                dirty = false;
                OnRomOpened(EventArgs.Empty);
            }
            catch (Exception e)
            {
                error = true;
                MessageBox.Show("Unable to read file " + file + Environment.NewLine + Environment.NewLine + e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return !error;
        }


        /// <summary>
        /// overwrite the current file.
        /// </summary>
        /// <returns>TRUE if the file successfully loaded.</returns>
        public bool Save()
        {
            return Save(filePath);
        }

        /// <summary>
        /// Write the file to the selected location. Updates the FilePath to the save location.
        /// </summary>
        /// <param name="file">File path to read.</param>
        /// <returns>TRUE if the file successfully loaded.</returns>
        public bool Save(string file)
        {
            bool error = false;
            try
            {
                File.WriteAllBytes(file, Data);
                filePath = file;
                dirty = false;
                OnRomSaved(EventArgs.Empty);
            }
            catch (Exception e)
            {
                error = true;
                MessageBox.Show("Unable to write to file " + file + Environment.NewLine + Environment.NewLine + e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return !error;
        }


        //---------------------------------------------------------
        // Custom Events
        //---------------------------------------------------------
        #region Custom_Events

        public event EventHandler<EventArgs> RomOpened;
        public event EventHandler<EventArgs> RomSaved;
        public event EventHandler<EventArgs> RomEdited;

        protected virtual void OnRomOpened(EventArgs e)
        {
            RomOpened?.Invoke(this, e);
        }

        protected virtual void OnRomSaved(EventArgs e)
        {
            RomSaved?.Invoke(this, e);
        }

        protected virtual void OnRomEdited(EventArgs e)
        {
            RomEdited?.Invoke(this, e);
        }

        #endregion



    }
}
