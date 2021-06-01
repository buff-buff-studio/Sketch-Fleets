using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.ProfileSystem;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Main Codex container class
    /// </summary>
    public class CodexContainer : MonoBehaviour 
    {
        #region Registers
        public ItemRegister registerItems;
        public ShipRegister registerEnemies;
        public ShipRegister registerAllies;
        public Register<object> registerRegions;
        #endregion 
        
        public void ListItems()
        {
            RenderEntries(CodexEntryType.Item,registerItems);
        }

        public void ListAllies()
        {
            RenderEntries(CodexEntryType.Ally,registerAllies);
        }

        /// <summary>
        /// Render all entries
        /// </summary>
        /// <param name="type"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        public void RenderEntries<T>(CodexEntryType type,Register<T> items)
        {
            List<int> unlockedList = new List<int>();

            //Add unlocked id
            foreach(CodexEntry entry in Profile.GetData().codex.GetUnlockedEntries(type))
                unlockedList.Add(entry.id);

            //Render items
            int index = 0;
            for(int i = 0; i < items.items.Length; i ++)
            {
                bool isUnlocked = unlockedList.Contains(i);
                if(RenderEntry(index,i,(object) items.items[i],isUnlocked))
                    index ++;
            }
        }

        /// <summary>
        /// Render entry
        /// </summary>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="unlocked"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RenderEntry(int index,int id,object item,bool unlocked)
        {
            if(item is Item)
            {
                Item it = (Item) item;
            }
            else if(item is ShipAttributes)
            {
                
            }
            else
            {

            }

            Debug.Log("Item: " + id + " Unlocked " + unlocked);
            return true;
        }
    }
}