using ManyTools.UnityExtended;
using ManyTools.UnityExtended.Editor;
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

        [Header("Parameters")]
        [SerializeField, Tooltip("The ship register ScriptableObject"), RequiredField()]
        private ShipRegister shipRegister;

        #endregion

        #region Public Fields

        /// <summary>
        /// Collects the entry of a ship
        /// </summary>
        /// <param name="ship">The ship to collect the entry of</param>
        public void CollectEntry(ShipAttributes ship)
        {
            CodexEntry entry = new CodexEntry(CodexEntryType.Ship, ship.CodexRarity, GetRegisterID(shipRegister, ship));
            Profile.Data.codex.AddItem(entry);
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Gets the register ID for the given entry
        /// </summary>
        /// <param name="register">The register to search in</param>
        /// <param name="entry">The entry to get the ID of</param>
        /// <returns>The entry's ID</returns>
        private static int GetRegisterID(Register<ShipAttributes> register, Object entry)
        {
            for (int index = 0, upper = register.items.Length; index < upper; index++)
            {
                if (register.items[index] == entry)
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
