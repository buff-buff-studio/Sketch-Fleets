using System;
using System.Collections.Generic;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Main codex inventory
    /// </summary>
    public class CodexInventory : IInventory<CodexEntry>
    {   
        #region Private Fields
        
        private Dictionary<CodexEntryType, List<CodexEntry>> foundEntries = 
            new Dictionary<CodexEntryType, List<CodexEntry>>();
        
        #endregion

        #region Constructors
        
        public CodexInventory()
        {
            foreach(CodexEntryType type in Enum.GetValues(typeof(CodexEntryType)))
            {
                foundEntries[type] = new List<CodexEntry>();
            }
        }
        
        #endregion

        #region IInventory
        
        /// <summary>
        /// Checks whether a given entry can be added to the codex inventory
        /// </summary>
        /// <param name="entry">The entry to add</param>
        /// <returns>Whether the entry was added</returns>
        public bool CanAddItem(CodexEntry entry)
        {
            return !ContainsID(foundEntries[entry.Type], entry.ID);
        }

        /// <summary>
        /// Adds an item to the codex
        /// </summary>
        /// <param name="entry">The entry to add</param>
        /// <returns>1 for failure to add, 0 for successful addition</returns>
        public int AddItem(CodexEntry entry)
        {
            if(!CanAddItem(entry))
                return 1;
            
            foundEntries[entry.Type].Add(entry);
            return 0;
        }

        /// <summary>
        /// Gets the slot count for the codex
        /// </summary>
        /// <returns>The slot count for the codex</returns>
        public int GetSlotCount()
        {
            return int.MaxValue;
        }
        
        /// <summary>
        /// Searches for a specific
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public int SearchItem(CodexEntry entry)
        {
            return ContainsID(foundEntries[entry.Type], entry.ID) ? 1 : 0;
        }

        public bool UseItem(CodexEntry entry)
        {
            return true;
        }

        public CodexEntry GetItem(int slot)
        {
            return null;
        }

        /// <summary>
        /// Searches a codex entry list for a specific ID
        /// </summary>
        /// <param name="entries">The list of entries</param>
        /// <param name="id">The ID to search for</param>
        /// <returns>Whether the list contains the given ID</returns>
        public bool ContainsID(List<CodexEntry> entries, int id)
        {
            for (int index = 0, upper = entries.Count; index < upper; index++)
            {
                if (entries[index].ID == id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public Methods
        
        public IEnumerable<CodexEntry> GetUnlockedEntries(CodexEntryType type)
        {
            foreach(CodexEntry entry in foundEntries[type])
                yield return new CodexEntry(type, entry.Rarity, entry.ID);
        }
        
        #endregion
    }
}