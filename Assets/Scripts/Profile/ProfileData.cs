using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.SaveSystem;
using SketchFleets.Inventory;
using System.IO;

namespace SketchFleets.ProfileSystem
{
    /// <summary>
    /// Main profile system
    /// </summary>
    public class ProfileData
    {
        #region Private Fields
        private Save _save;
        #endregion

        #region Public Fields
        public UpgradeInventory inventoryUpgrades;
        public ItemInventory inventoryItems;
        #endregion

        #region Properties
        public Save save { get { return _save; } set { _save = value; ReloadInventories(); } }
        #endregion

        /// <summary>
        /// Update data
        /// </summary>
        public ProfileData()
        {
            save = new Save();
        }

        /// <summary>
        /// Reload inventory data
        /// </summary>
        public void ReloadInventories()
        {
            inventoryUpgrades = new UpgradeInventory();
            inventoryItems = new ItemInventory(24);

            //Read inventory data
            if(save.HasKey("items"))
            {
                SaveListObject list = save.Get<SaveListObject>("items");
                
                foreach(SaveObject o in list)
                {
                    inventoryItems.AddItem(new ItemStack(o.Get<int>("id"),o.Get<int>("amount")));
                }
            }

            if(save.HasKey("upgrades"))
            {
                SaveListObject list = save.Get<SaveListObject>("upgrades");
                
                foreach(SaveObject o in list)
                {
                    inventoryUpgrades.AddItem(new ItemStack(o.Get<int>("id")));
                }
            }
        }

        /// <summary>
        /// Save inventory data to save object
        /// </summary>
        public void SaveInventories()
        {
            SaveListObject list = save.CreateChildList();
            //Write inventory data
            foreach(ItemStack i in inventoryItems)
            {
                SaveObject o = list.CreateChild();
                o["id"] = i.Id;
                o["amount"] = i.Amount;
                
                list.Add(o);
            }
            save["items"] = list;

            list = save.CreateChildList();
            //Write upgrades data
            foreach(ItemStack i in inventoryUpgrades)
            {
                SaveObject o = list.CreateChild();
                o["id"] = i.Id;
                
                list.Add(o);
            }
            save["upgrades"] = list;
        }

        public void Clear(MonoBehaviour behaviour)
        {
            save.Clear();
            ReloadInventories();
            Profile.Using(behaviour);
            Profile.SaveProfile((data) => {});
        }
    }
}