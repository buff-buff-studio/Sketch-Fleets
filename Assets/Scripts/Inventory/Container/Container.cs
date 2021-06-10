using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

        [SerializeField]
        protected RectTransform tooltipBox;
        [SerializeField]
        protected TMP_Text tooltipText;
        [SerializeField]
        protected RectTransform tooltipBackground;
        #endregion
        #region Private Fields
        private Canvas canvas;
        private ItemStackAnimation heldItem;
        #endregion

        /// <summary>
        /// Init container
        /// </summary>
        public virtual void Start() 
        {
            canvas = GetComponentInParent<Canvas>();
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

        private void Update() 
        {
            //Check mouse over slot
            PointerEventData ev = new PointerEventData(EventSystem.current);
            ev.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ev,results);

            int hoverSlot = -1;
            Vector2 screenPos = Vector2.zero;
            if(results.Count > 0)
            {
                for(int slot = 0; slot < slots.Length; slot ++)
                {
                    if(slots[slot].gameObject == results[0].gameObject)
                    {                 
                        if(inventory.GetItem(slot) != null)
                        {
                            screenPos = results[0].screenPosition;
                            hoverSlot = slot;
                        }
                        break;
                    }
                }
            }

            //Display info
            if(hoverSlot >= 0)
            {
                ItemStackAnimation anim = slots[hoverSlot].GetChild(0).GetComponent<ItemStackAnimation>();
                if(anim != null)
                    anim.hovering = true;

                if(heldItem != null)
                    if(anim == null || anim != heldItem)
                    {
                        heldItem.hovering = false;
                    }

                heldItem = anim;

                if(!tooltipBox.gameObject.activeInHierarchy)
                    tooltipBox.gameObject.SetActive(true);

                Vector2 local;
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) canvas.transform, Input.mousePosition, GetComponentInParent<Canvas>().worldCamera, out local);
                tooltipBox.anchoredPosition = local + new Vector2(70,40);

                float off = tooltipText.GetRenderedValues(true).x * canvas.scaleFactor;
                tooltipBackground.sizeDelta = new Vector2(tooltipText.GetRenderedValues(true).x,100);

                tooltipText.text = GetTooltipText(hoverSlot);
                
                if(off + Input.mousePosition.x > Screen.width - 50)
                {
                    tooltipBackground.pivot = new Vector2(1,0.5f);
                    tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                }
                else
                {
                    tooltipBackground.pivot = new Vector2(0,0.5f);
                    tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                }
            }
            else
            {
                if(tooltipBox.gameObject.activeInHierarchy)
                    tooltipBox.gameObject.SetActive(false);

                if(heldItem != null)
                    heldItem.hovering = false;
                heldItem = null;
            }
        }

        private int lastHoveredSlot = -1;
        private string lastTooltipText = null;
        protected string GetTooltipText(int slot)
        {
            if(slot != lastHoveredSlot)
            {
                lastHoveredSlot = slot;
                lastTooltipText = register.items[inventory.GetItem(slot).Id].UnlocalizedName;
                //lastTooltipText = LanguageSystem.LanguageManager.Localize(register.items[inventory.GetItem(slot).Id].UnlocalizedName);
            }
            
            return lastTooltipText;
        }
    }
}
