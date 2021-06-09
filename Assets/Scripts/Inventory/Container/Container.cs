using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Default container base class. Used to render and handle an inventory
    /// </summary>
    public class Container : MonoBehaviour
    {
        #region Protected Fields
        [SerializeField]
        protected ShopObjectRegister register;
        [SerializeField]
        protected IInventory<ItemStack> inventory;
        [SerializeField]
        protected RectTransform[] slots;
        protected Action<int> OnClickSlot;
        #endregion

        /// <summary>
        /// Init container
        /// </summary>
        public virtual void Start() 
        {
            
        }

        /// <summary>
        /// Render entire container
        /// </summary>
        public virtual void Render()
        {
            //Render all items
            for(int i = 0; i < slots.Length; i ++)
            {
                RenderSlot(i);
            }
        }

        /// <summary>
        /// Render single slot
        /// </summary>
        /// <param name="index"></param>
        public virtual void RenderSlot(int index)
        {
            
        }

        /// <summary>
        /// Handle slot click
        /// </summary>
        /// <param name="slot"></param>
        protected void OnClickSlotInternal(int slot)
        {
            if(OnClickSlot != null)
                OnClickSlot(slot);
        }
    }
}
