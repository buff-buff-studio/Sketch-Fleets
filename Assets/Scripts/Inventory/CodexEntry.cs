using SketchFleets.Inventory;

namespace SketchFleets.Inventory
{   
    /// <summary>
    /// Codex entry
    /// </summary>
    public class CodexEntry
    {
        #region Public Fields
        
        public int ID;
        public CodexEntryType Type;
        public CodexEntryRarity Rarity;
        
        #endregion

        #region Constructors
        
        /// <summary>
        /// Constructs a codex entry class
        /// </summary>
        /// <param name="type">The type of the codex entry</param>
        /// <param name="rarity">The rarity of the codex entry</param>
        /// <param name="id">The register ID of the codex entry</param>
        public CodexEntry(CodexEntryType type, CodexEntryRarity rarity, int id)
        {
            Type = type;
            Rarity = rarity;
            ID = id;
        }
        
        #endregion
    }
}