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
        protected ItemRegister register;
        [SerializeField]
        protected IInventory<ItemStack> inventory;
        [SerializeField]
        protected RectTransform[] slots;
        protected Action<int> OnClickSlot;
        #endregion

        /// <summary>
        /// Init container
        /// </summary>
        public void Start() 
        {
            #region Temporary
            //Load
            inventory = new ShopInventory();
            for(int i = 0; i < slots.Length; i ++)
            {
                inventory.AddItem(new ItemStack(register.PickRandom()));
            }

            for(int i = 0; i < slots.Length; i ++)
            {
                Button btn = slots[i].GetComponent<Button>();
                int ji = i;
                if(btn != null)
                    btn.onClick.AddListener(() => {
                        OnClickSlotInternal(ji);
                    });
            }

            //Render items
            Render();
            #endregion    
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
            ItemStack stack = inventory.GetItem(index);
            
            string name = "";
            if(stack != null)
                name = register.items[stack.Id].UnlocalizedName;

            #region Temporary
            slots[index].GetChild(0).GetComponent<TMP_Text>().text = name; 
            #endregion
        }

        /// <summary>
        /// Handle slot click
        /// </summary>
        /// <param name="slot"></param>
        private void OnClickSlotInternal(int slot)
        {
            if(OnClickSlot != null)
                OnClickSlot(slot);
        }
    }
}
