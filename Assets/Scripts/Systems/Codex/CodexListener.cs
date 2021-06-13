using System.Collections.Generic;
using ManyTools.UnityExtended;
using SketchFleets.Data;
using SketchFleets.Inventory;
using SketchFleets.ProfileSystem;
using UnityEngine;

namespace SketchFleets.Systems.Codex
{
    /// <summary>
    /// A class that listens for new codex entries
    /// </summary>
    public class CodexListener : Singleton<CodexListener>
    {
        #region Private Fields

        private ShipRegister shipRegister;
        private List<ShipAttributes> collectedEntries = new List<ShipAttributes>();

        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            LoadCollectedEntries();
        }
        
        #endregion

        #region Public Fields

        /// <summary>
        /// Collects the entry of a ship
        /// </summary>
        /// <param name="ship">The ship to collect the entry of</param>
        public void CollectEntry(ShipAttributes ship)
        {
            if (!collectedEntries.Contains(ship))
            {
                collectedEntries.Add(ship);
            }
        }

        #endregion

        #region Private Fields

        private void SaveCollectedEntries()
        {
            for (int index = 0, upper = collectedEntries.Count; index < upper; index++)
            {
                CodexEntry entry = new CodexEntry();
                Profile.Data.codex.AddItem(GetRegisterID(collectedEntries[index]));
            }
        }

        private List<ShipAttributes> LoadCollectedEntries()
        {
            return new List<ShipAttributes>();
        }

        /// <summary>
        /// Gets the register ID for the given ship type
        /// </summary>
        /// <param name="entry">The entry to get the ID of</param>
        /// <returns>The entry's ID</returns>
        private int GetRegisterID(ShipAttributes entry)
        {
            for (int index = 0, upper = shipRegister.items.Length; index < upper; index++)
            {
                if (shipRegister.items[index] == entry)
                {
                    return index;
                }
            }
            
            Debug.LogError("Entry did not exist in the register!");
            return default;
        }

        #endregion
    }
}
