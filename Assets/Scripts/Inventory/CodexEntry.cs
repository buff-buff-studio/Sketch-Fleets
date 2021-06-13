using SketchFleets.Inventory;

namespace SketchFleets.Inventory
{   
    /// <summary>
    /// Codex entry
    /// </summary>
    public class CodexEntry
    {
        #region Public Fields
        
        public int id;
        public CodexEntryType type;
        
        #endregion

        #region Constructors
        
        public CodexEntry(CodexEntryType type,int id)
        {
            this.type = type;
            this.id = id;
        }
        
        #endregion
    }
}