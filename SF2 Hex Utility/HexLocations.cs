using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF2_Hex_Utility
{
    public class HexLocations
    {
        //Properties
        public string Name { get { return Path.GetFileNameWithoutExtension(FilePath); } }
        public string FilePath { get; set; }

        public ObservableCollection<HexLocation> Items { get; } = new ObservableCollection<HexLocation>(); //never add directly to this; always use a function to add items
        //public List<HexLocation> Items { get; private set; } //never add directly to this; always use a function to add items
        public HexLocation this[int param]
        {
            get { return Items[param]; }
            set { Items[param] = value; }
        }

        private bool dirty = false;
        public bool Dirty
        {
            get { return dirty; }
            set
            {
                dirty = value;
                if (pauseEvents == false)
                    OnHexLocationDirtyFlagChanged(new EventArgs());
            }
        }

        private bool pauseEvents = false;


        //Constructor
        public HexLocations()
        {
            FilePath = GetFileLocation();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        public HexLocations(string filePath)
        {
            FilePath = filePath;
            Items.CollectionChanged += Items_CollectionChanged;
        }


        /// <summary>
        /// Return a duplicate of the current instance of this object
        /// </summary>
        /// <returns></returns>
        public HexLocations Clone()
        {
            HexLocations clone = new HexLocations();
            clone.Dirty = this.Dirty;
            clone.FilePath = this.FilePath;

            List<HexLocation> clonedItems = new List<HexLocation>();
            foreach (HexLocation h in this.Items)
            {
                clonedItems.Add(h.Clone());
            }
            clone.AddRange(clonedItems);
            
            return clone;
        }



        //------------------------------------------------------------------
        // Event Summary
        //------------------------------------------------------------------
        #region Event_Summary
        /// <raises>
        ///     ListHexLocationChanged = when an item is added or remove from the list of Hex Locations (Items.CollectionChanged)
        ///     HexLocationChanged = when a specific hex location has been edited (hexLocation.ValueChanged)
        ///     DirtyFlagChanged = whenever the dirty flag is changed, which can be done manually, or during ListHexLocationChanged, HexLocationChanged, Load, Save, and Clone
        /// </raises>
        /// <listens>
        ///     hexLocation.ValueChanged = when the hex location is changed (technically when the HexLocation.RaiseEventHexLocationValueChanged is called), raise the event HexLocationChanged
        ///     Items.CollectionChanged = observable collection items added or removed (basically, whenever new hex locations have been added or removed), raise event ListHexLocationChanged
        /// </listens>
        #endregion



        //---------------------------------------------------------
        // Item List Functions
        //---------------------------------------------------------
        #region ItemList_Functions

        /// <summary>
        /// Add a single item to the Hex Locations list.
        /// </summary>
        /// <param name="itemToAdd">Item to be added to the list.</param>
        public void Add(HexLocation itemToAdd)
        {
            AddRange(new List<HexLocation>() { itemToAdd });
        }


        /// <summary>
        /// Add a range of items while trigger the "list change" event only once.
        /// </summary>
        /// <param name="itemsToAdd">List of hex locations to add.</param>
        public void AddRange(List<HexLocation> itemsToAdd)
        {
            pauseEvents = true;  //prevent events from triggering
            foreach (HexLocation h in itemsToAdd)
            {
                h.ValueChanged += hexLocation_ValueChanged;
                Items.Add(h);
            }
            pauseEvents = false; //resume event triggers
            OnHexLocationListChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsToAdd));
        }


        /// <summary>
        /// Replace an item at a specified index with a new item.
        /// </summary>
        /// <param name="itemToInsert">Item to be added to the list.</param>
        /// <param name="index">Zero-based index denoting the location for insertion.</param>
        public void Replace(int index, HexLocation item)
        {
            Items[index] = item;
        }


        /// <summary>
        /// Set the range of items while trigger the "list change" event only once. This will first clear the existing items list.
        /// </summary>
        /// <param name="items">List of hex locations to add.</param>
        public void SetItems(List<HexLocation> items)
        {
            pauseEvents = true;  //prevent events from triggering for Items.Clear; this is set to FALSE in the AddRange call
            Items.Clear();
            AddRange(items);
        }


        /// <summary>
        /// Raise event when the item collection is changed.
        /// </summary>
        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (pauseEvents == false)
            {
                OnHexLocationListChanged(e);
            }
        }

        #endregion



        //---------------------------------------------------------
        // Import and Export Functions
        //---------------------------------------------------------
        #region Import&Export_Functions

        /// <summary>
        /// Return the file save location.
        /// </summary>
        /// <returns></returns>
        public string GetFileLocation()
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(appPath, @"Hex Locations.txt");
        }


        /// <summary>
        /// Load hex locations from the saved filepath and set the dirty flag to FALSE.
        /// </summary>
        public bool Load()
        {
            bool returnValue = ReadItemsFromFile();
            if (returnValue)
                dirty = false;
            return returnValue;
        }

        public bool Save()
        {
            return WriteItemsToFile();
        }


        /// <summary>
        /// Read hex locations from the saved filepath and set the dirty flag to TRUE.
        /// </summary>
        /// <returns>TRUE if the read action was successful.</returns>
        public bool ReadItemsFromFile()
        {
            return ReadItemsFromFile(FilePath);
        }

        public bool WriteItemsToFile()
        {
            return WriteItemsToFile(FilePath);
        }


        /// <summary>
        /// Read all hex locations from a specified file and populate Items with the data. No changes are made to Items if there is an error. If no error occurs, update the FilePath for future saving.
        /// </summary>
        /// <param name="file">The file to read.</param>
        /// <returns>TURE if the file was successfully read.</returns>
        public bool ReadItemsFromFile(string file)
        {
            bool error = false;
            List<HexLocation> newItems = new List<HexLocation>();
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        HexLocation h = new HexLocation(line);
                        newItems.Add(h);
                    }
                }
                FilePath = file;
                SetItems(newItems);
                this.Dirty = true;
            }
            catch (Exception e)
            {
                error = true;
                MessageBox.Show("Unable to read file " + file + Environment.NewLine + Environment.NewLine + e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //throw;
            }

            return !error;
        }


        /// <summary>
        /// Write all hex locations to the specified file. If no error occurs, update the FilePath for future loading and saving.
        /// </summary>
        /// <param name="file">File location to which to write. This file will be overwritten.</param>
        /// <returns>TRUE if the write tio file was successful.</returns>
        public bool WriteItemsToFile(string file)
        {
            bool error = false;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(file)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                }
                using (StreamWriter sw = new StreamWriter(file))
                {
                    foreach (HexLocation h in Items)
                    {
                        sw.WriteLine(h.GetExportString());
                    }
                }
                FilePath = file;
                this.Dirty = false;
            }
            catch (Exception e)
            {
                error = true;
                MessageBox.Show("Unable to write to file " + file + Environment.NewLine + Environment.NewLine + e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //throw;
            }
            return !error;
        }

        #endregion



        //---------------------------------------------------------
        // Get Arrays of Folders, Groups, or Locations
        //---------------------------------------------------------
        #region GetArray_Functions

        public string[] GetFolders()
        {
            return Items.Select(x => x.Folder).Distinct().ToArray();
        }

        public string[] GetGroups()
        {
            return Items.Select(x => x.Group).Distinct().ToArray();
        }

        public string[] GetGroups(string folder)
        {
            return Items.Where(x => x.Folder == folder).Select(x => x.Group).Distinct().ToArray();
        }

        public HexLocation[] GetLocations(string folder, string group)
        {
            return Items.Where(x => x.Folder == folder && x.Group == group).ToArray();
        }


        #endregion



        //---------------------------------------------------------
        // Custom Events
        //---------------------------------------------------------
        #region CustomEvents

        public event EventHandler<NotifyCollectionChangedEventArgs> ListHexLocationChanged;
        public event EventHandler<EventArgs> DirtyFlagChanged;
        public event EventHandler<HexLocationChangedEventArgs> HexLocationChanged;


        protected virtual void OnHexLocationListChanged(NotifyCollectionChangedEventArgs e)
        {
            Dirty = true;
            if (pauseEvents == false)
                ListHexLocationChanged?.Invoke(this, e);
        }


        protected virtual void OnHexLocationDirtyFlagChanged(EventArgs e)
        {
            if (pauseEvents == false) 
                DirtyFlagChanged?.Invoke(this, e);
        }


        protected virtual void OnHexLocationChanged(HexLocationChangedEventArgs e)
        {
            Dirty = true;
            if (pauseEvents == false) 
                HexLocationChanged?.Invoke(this, e);
        }


        public class HexLocationChangedEventArgs : EventArgs
        {
            public HexLocation HexLocation { get; set; }
        }


        #endregion



        //---------------------------------------------------------
        // Other Events
        //---------------------------------------------------------
        #region Other_Events

        private void hexLocation_ValueChanged(object sender, EventArgs e)
        {
            OnHexLocationChanged(new HexLocationChangedEventArgs() { HexLocation = (HexLocation)sender });
        }


        #endregion

    }
}
