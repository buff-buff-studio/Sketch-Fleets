namespace SketchFleets.Inventory
{
    /// <summary>
    /// Base inventory interface
    /// </summary>
    public interface IInventory<T>
    {
        /// <summary>
        /// Check if can add item
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        bool CanAddItem(T stack);

        /// <summary>
        /// Add item and return overflow
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        int AddItem(T stack);

        /// <summary>
        /// Return inventory slot count
        /// </summary>
        /// <returns></returns>
        int GetSlotCount();
        

        /// <summary>
        /// Search for item and return count
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        int SearchItem(T stack);

        /// <summary>
        /// Try to use item and return true if contains
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        bool UseItem(T stack);

        /// <summary>
        /// Get item at slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        T GetItem(int slot);
    }
}