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
        private Dictionary<CodexEntryType,List<int>> foundEntries = new Dictionary<CodexEntryType, List<int>>();
        #endregion

        #region Constructors
        public CodexInventory()
        {
            foreach(CodexEntryType type in Enum.GetValues(typeof(CodexEntryType)))
            {
                foundEntries[type] = new List<int>();
            }
        }
        #endregion

        #region IInventory
        public bool CanAddItem(CodexEntry entry)
        {
            return !foundEntries[entry.type].Contains(entry.id);
        }

        public int AddItem(CodexEntry entry)
        {
            if(!CanAddItem(entry))
                return 1;
            
            foundEntries[entry.type].Add(entry.id);
            return 0;
        }

        public int GetSlotCount()
        {
            return int.MaxValue;
        }
        
        public int SearchItem(CodexEntry entry)
        {
            return foundEntries[entry.type].Contains(entry.id) ? 1 : 0;
        }

        public bool UseItem(CodexEntry entry)
        {
            return true;
        }

        public CodexEntry GetItem(int slot)
        {
            return null;
        }

        #endregion

        #region Public Methods
        public IEnumerable<CodexEntry> GetUnlockedEntries(CodexEntryType type)
        {
            foreach(int id in foundEntries[type])
                yield return new CodexEntry(type,id);
        }
        #endregion
    }
}