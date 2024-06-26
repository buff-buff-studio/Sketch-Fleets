using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Default container base class. Used to render and handle an inventory
    /// </summary>
    public class Container : MonoBehaviour
    {
        #region Public Static Fields
        //Used to track tooltip
        public static int tooltipId = 0;
        #endregion

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

        #region Tooltip
        private Vector2 mouse;
        private int heldSlot = -1;
        #endregion

        #region Public Fields
        public bool centered = false;
        #endregion

        /// <summary>
        /// Init container
        /// </summary>
        public virtual void Start()
        {
            HideTooltip();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnDisable()
        {
            HideTooltip();
        }

        public void HideTooltip()
        {
            tooltipId++;
            heldSlot = -1;
            if (tooltipBox.gameObject.activeInHierarchy)
                tooltipBox.gameObject.SetActive(false);

            tooltipBox.GetComponent<CanvasGroup>().alpha = 0;
        }

        IEnumerator ShowToolTip(float time)
        {
            tooltipId ++;
            int id = tooltipId;
            yield return new WaitForSeconds(time);

            float t = Time.time;
            while(tooltipId == id)
            {
                float tm = Mathf.Clamp01((Time.time - t) * 5);
                tooltipBox.GetComponent<CanvasGroup>().alpha = tm;
                if(tm == 1)
                    break;
                yield return null;
            }
        }

        /// <summary>
        /// Render entire container
        /// </summary>
        public virtual void Render()
        {
            //Render all items
            for (int i = 0; i < slots.Length; i++)
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
            //Hide


            if (heldSlot == slot)
            {
                //heldSlot = -1;

                if (OnClickSlot != null)
                {
                    if (tooltipBox.gameObject.activeInHierarchy)
                        tooltipBox.gameObject.SetActive(false);
                    tooltipBox.GetComponent<CanvasGroup>().alpha = 0;

                    OnClickSlot(slot);
                }

                return;
            }

            if (tooltipBox.gameObject.activeInHierarchy)
                tooltipBox.gameObject.SetActive(false);
            tooltipBox.GetComponent<CanvasGroup>().alpha = 0;

            this.mouse = slots[slot].position;
            //this.mouse = Mouse.current.position.ReadValue();
            this.heldSlot = slot;

            if (heldSlot >= 0 && GetItemInSlot(heldSlot) != null)
            {
                tooltipText.text = GetTooltipText(heldSlot);
                tooltipDescription.text = GetTooltipDescription(heldSlot);

                if (!tooltipBox.gameObject.activeInHierarchy)
                    tooltipBox.gameObject.SetActive(true);

                StartCoroutine(ShowToolTip(0.05f));
            }

            //Open toolbar
        }

        public virtual ItemStack GetItemInSlot(int slot)
        {
            return inventory.GetItem(slot);
        }

        protected virtual void Update()
        {
            if (heldSlot >= 0 && GetItemInSlot(heldSlot) != null)
            {
                ItemStackAnimation anim = slots[heldSlot].GetComponentInChildren<ItemStackAnimation>();
                if (anim != null)
                    anim.hovering = true;

                if (heldItem != null)
                    if (anim == null || anim != heldItem)
                    {
                        heldItem.hovering = false;
                    }

                heldItem = anim;

                Vector2 local;
                Camera cam = canvas.worldCamera;
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, mouse, cam, out local);

                float off = tooltipText.GetRenderedValues(true).x * canvas.scaleFactor;
                float desc = tooltipDescription.GetRenderedValues(true).y;

                float w = tooltipText.GetRenderedValues(true).x;
                tooltipBackground.sizeDelta = new Vector2(w, 100 + 5 + desc);

                tooltipDescription.rectTransform.sizeDelta = new Vector2(w, 50);

                if (centered)
                {
                    tooltipBackground.pivot = new Vector2(0f, 1f);
                    tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tooltipDescription.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    tooltipDescription.margin = new Vector4(0, 0, 0, 0);

                    tooltipBox.anchoredPosition = new Vector2(-w / 2f, 0);
                }
                else
                {
                    if (off + mouse.x > Screen.width - 50)
                    {
                        tooltipBackground.pivot = new Vector2(1, 1f);
                        tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                        tooltipDescription.horizontalAlignment = HorizontalAlignmentOptions.Right;
                        tooltipDescription.margin = new Vector4(-w, 0, w, 0);

                        tooltipBox.anchoredPosition = local + new Vector2(-100, 40) + new Vector2(0, changeY);
                    }
                    else
                    {
                        tooltipBackground.pivot = new Vector2(0, 1f);
                        tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                        tooltipDescription.horizontalAlignment = HorizontalAlignmentOptions.Left;
                        tooltipDescription.margin = new Vector4(0, 0, 0, 0);

                        tooltipBox.anchoredPosition = local + new Vector2(100, 40) + new Vector2(0, changeY);
                    }
                }
            }

            /*
            //Check mouse over slot
            PointerEventData ev = new PointerEventData(EventSystem.current);
            ev.position = Mouse.current.position.ReadValue();
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
            */
        }

        private int lastHoveredSlot = -1;
        private string lastTooltipText = null;
        protected virtual string GetTooltipText(int slot)
        {
            if (slot != lastHoveredSlot)
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
            if (slot != lastHoveredSlotDesc)
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
