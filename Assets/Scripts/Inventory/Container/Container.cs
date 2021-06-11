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
        protected TMP_Text tooltipDescription;
        [SerializeField]
        protected RectTransform tooltipBackground;
        [SerializeField]
        protected float changeY = 0;
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

        public virtual ItemStack GetItemInSlot(int slot)
        {
            return inventory.GetItem(slot);
        }

        protected virtual void Update() 
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
                        if(GetItemInSlot(slot) != null)
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
                ItemStackAnimation anim = slots[hoverSlot].GetComponentInChildren<ItemStackAnimation>();
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
                
                float off = tooltipText.GetRenderedValues(true).x * canvas.scaleFactor;
                float desc = tooltipDescription.GetRenderedValues(true).y;

                float w = tooltipText.GetRenderedValues(true).x;
                tooltipBackground.sizeDelta = new Vector2(w,100 + 5 + desc);

                tooltipText.text = GetTooltipText(hoverSlot);
                tooltipDescription.text = GetTooltipDescription(hoverSlot);

                tooltipDescription.rectTransform.sizeDelta = new Vector2(w,50);
                
                if(off + Input.mousePosition.x > Screen.width - 50)
                {
                    tooltipBackground.pivot = new Vector2(1,1f);
                    tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    tooltipDescription.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    tooltipDescription.margin = new Vector4(-w,0,w,0);

                    tooltipBox.anchoredPosition = local + new Vector2(-70,40) + new Vector2(0,changeY);
                }
                else
                {
                    tooltipBackground.pivot = new Vector2(0,1f);
                    tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tooltipDescription.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tooltipDescription.margin = new Vector4(0,0,0,0);

                    tooltipBox.anchoredPosition = local + new Vector2(70,40) + new Vector2(0,changeY);
                }
            }
            else
            {
                if(tooltipBox != null)
                    if(tooltipBox.gameObject.activeInHierarchy)
                        tooltipBox.gameObject.SetActive(false);

                if(heldItem != null)
                    heldItem.hovering = false;
                heldItem = null;
            }
        }

        private int lastHoveredSlot = -1;
        private string lastTooltipText = null;
        protected virtual string GetTooltipText(int slot)
        {
            if(slot != lastHoveredSlot)
            {
                lastHoveredSlot = slot;
                //lastTooltipText = register.items[inventory.GetItem(slot).Id].UnlocalizedName;
                lastTooltipText = LanguageSystem.LanguageManager.Localize(GetRegisterForSlot(slot).items[GetItemInSlot(slot).Id].UnlocalizedName);
            }
            
            return lastTooltipText;
        }

        private int lastHoveredSlotDesc = -1;
        private string lastTooltipTextDesc = null;
        protected virtual string GetTooltipDescription(int slot)
        {
            if(slot != lastHoveredSlotDesc)
            {
                lastHoveredSlotDesc = slot;
                //lastTooltipText = register.items[inventory.GetItem(slot).Id].UnlocalizedName;
                lastTooltipTextDesc = LanguageSystem.LanguageManager.Localize("desc_" + GetRegisterForSlot(slot).items[GetItemInSlot(slot).Id].UnlocalizedName);
            }
            
            return lastTooltipTextDesc;
        }

        public virtual ShopObjectRegister GetRegisterForSlot(int slot)
        {
            return register;
        }
    }
}
