using System.Collections.Generic;

namespace SketchFleets.SaveSystem
{
    /// <summary>
    /// Used to retrieve all siblings of pointer and parent of a sibling provider
    /// </summary>
    public interface ISaveObject
    {
        /// <summary>
        /// Get all child pointers of a object
        /// </summary>
        /// <returns></returns>
        IEnumerable<Pointer> GetPointers();
        
        /// <summary>
        /// Change byte size header of object
        /// </summary>
        /// <param name="change"></param>
        void ChangeByteSizeHeader(int change);

        /// <summary>
        /// Get parent of object
        /// </summary>
        /// <returns></returns>
        ISaveObject GetParent();

        /// <summary>
        /// Set parent of object (Internal Use Only)
        /// </summary>
        /// <param name="parent"></param>
        void SetParent(ISaveObject parent);

        /// <summary>
        /// Update value providers
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="mode"></param>
        void Init(IByteBuffer buffer,IEditModeProvider mode);

        /// <summary>
        /// Change current Dynamic byte information (Dynamic Only)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        void SetByteInfo(int position,int length);
    }
}