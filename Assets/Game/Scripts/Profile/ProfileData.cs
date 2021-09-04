using UnityEngine;
using SketchFleets.SaveSystem;
using SketchFleets.Inventory;

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
        //Game Inventories
        public ItemInventory inventoryUpgrades;
        public ItemInventory inventoryItems;
        //Codex inventories
        public CodexInventory codex = new CodexInventory();

        public int Coins { get => save.Get<int>("coins"); set { save.Set("coins", value); Profile.SaveProfile((data) => { }); } }
        public int TotalCoins { get => save.Get<int>("totalCoins"); set { save.Set("totalCoins", value); Profile.SaveProfile((data) => { }); } }
        public int TimeSeconds { get => save.Get<int>("seconds"); set { save.Set("seconds", value); Profile.SaveProfile((data) => { }); } }
        public int Kills { get => save.Get<int>("kkkkkkills"); set { save.Set("kkkkkkills", value); Profile.SaveProfile((data) => { }); } }
        

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
            inventoryUpgrades = new ItemInventory(4);
            inventoryItems = new PlayerItemInventory(24);
            codex = new CodexInventory();

            //Read inventory data
            if (save.HasKey("items"))
            {
                SaveListObject list = save.Get<SaveListObject>("items");

                foreach (SaveObject o in list)
                {
                    inventoryItems.AddItem(new ItemStack(o.Get<int>("id"), o.Get<int>("amount")));
                }
            }

            if (save.HasKey("upgrades"))
            {
                SaveListObject list = save.Get<SaveListObject>("upgrades");

                foreach (SaveObject o in list)
                {
                    inventoryUpgrades.AddItem(new ItemStack(o.Get<int>("id"), o.Get<int>("amount")));
                }
            }

            //Load codex
            if (save.HasKey("codex"))
            {
                SaveListObject list = save.Get<SaveListObject>("codex");

                for(int i = 0; i < list.Count; i ++)
                {
                    SaveListObject entry = list.Get<SaveListObject>(i);
                    for(int j = 0; j < entry.Count; j ++)
                    {
                        codex.AddItem(new CodexEntry((CodexEntryType) i, entry.Get<int>(j)));
                    }
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
            foreach (ItemStack i in inventoryItems)
            {
                SaveObject o = list.CreateChild();
                o["id"] = i.Id;
                o["amount"] = i.Amount;

                list.Add(o);
            }
            save["items"] = list;

            list = save.CreateChildList();
            
            //Write upgrades data
            foreach (ItemStack i in inventoryUpgrades)
            {
                SaveObject o = list.CreateChild();
                o["id"] = i.Id;
                o["amount"] = i.Amount;

                list.Add(o);
            }
            save["upgrades"] = list;

            //Save Codex
            SaveListObject codex = save.CreateChildList();
            foreach(CodexEntryType type in System.Enum.GetValues(typeof(CodexEntryType)))
            {
                int id = (int) type;
                SaveListObject current = save.CreateChildList();
                foreach(CodexEntry entry in this.codex.GetUnlockedEntries(type))
                {
                    current.Add(entry.ID);
                }

                codex.Add(current);
            }
            save["codex"] = codex;
        }

        public static int ConvertCoinsToTotalCoins(int coins)
        {
            return coins/5;
        }

        public void Clear(MonoBehaviour behaviour,System.Action<ProfileData> callback)
        {
            save.Remove("mapState");
            save.Remove("items");
            save.Remove("coins");
            save.Remove("seconds");
            save.Remove("kkkkkkills");

            ReloadInventories();
            
            try
            {
                Profile.SaveProfile(callback);
            }
            catch(System.Exception)
            {
                callback(this);
            }
        }
    }
}