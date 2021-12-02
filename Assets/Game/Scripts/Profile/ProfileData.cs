using UnityEngine;
using System.Collections.Generic;
using SketchFleets.Inventory;
using SketchFleets.Systems.Tutorial;


namespace SketchFleets.ProfileSystem
{
    [System.Serializable]
    public class MapData
    {
        public int currentStar = 0;
        public List<int> openPath = new List<int>();
        //public List<int> openQueue = new List<int>();
        public List<int> choosen = new List<int>();
        public int seed = -1; //0 = any random seed
    }

    [System.Serializable]
    public class SaveObject
    {
        public int coins = 0;
        public int totalCoins = 0;
        public int seconds = 0;
        public int kills = 0;

        public List<ProfileItem> items = new List<ProfileItem>();
        public List<ProfileItem> upgrades = new List<ProfileItem>();
        public List<CodexItem> codex = new List<CodexItem>();

        public MapData mapData = new MapData();
        public TutorialData tutorialData = new TutorialData();
    }

    [System.Serializable]
    public class CodexItem
    {
        public int type;
        public int id;

        public CodexItem(int type, int id)
        {
            this.id = id;
            this.type = type;
        }
    }

    [System.Serializable]
    public class ProfileItem
    {
        public int id;
        public int amount;

        public ProfileItem(int id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
    /// <summary>
    /// Main profile system
    /// </summary>
    public class ProfileData
    {
        #region Public Fields
        //Game Inventories
        public ItemInventory inventoryUpgrades;
        public ItemInventory inventoryItems;
        //Codex inventories
        public CodexInventory codex = new CodexInventory();

        public int Coins { get => saveObject.coins; set { saveObject.coins = value; Profile.SaveProfile((data) => { }); } }
        public int TotalCoins { get => saveObject.totalCoins; set { saveObject.totalCoins = value; Profile.SaveProfile((data) => { }); } }
        public int TimeSeconds { get => saveObject.seconds; set { saveObject.seconds = value; Profile.SaveProfile((data) => { }); } }
        public int Kills { get => saveObject.kills; set { saveObject.kills = value; Profile.SaveProfile((data) => { }); } }
        public MapData Map => saveObject.mapData;
        public TutorialData Tutorials => saveObject.tutorialData;

        public int ColorUpgradeCount => inventoryUpgrades.SearchItem(4);

        #endregion

        #region Core Object
        public SaveObject saveObject;
        #endregion

        /// <summary>
        /// Update data
        /// </summary>
        public ProfileData()
        {
            inventoryUpgrades = new ItemInventory(5);
            inventoryItems = new PlayerItemInventory(24);
            codex = new CodexInventory();
            saveObject = new SaveObject();
        }

        /// <summary>
        /// Reload inventory data
        /// </summary>
        public void ReloadInventories()
        {
            inventoryUpgrades = new ItemInventory(5);
            inventoryItems = new PlayerItemInventory(24);
            codex = new CodexInventory();

            //Read inventory data
            foreach (ProfileItem o in saveObject.items)
            {
                inventoryItems.AddItem(new ItemStack(o.id, o.amount));
            }

            foreach (ProfileItem o in saveObject.upgrades)
            {
                inventoryUpgrades.AddItem(new ItemStack(o.id, o.amount));
            }

            //Load codex
            foreach (CodexItem item in saveObject.codex)
            {
                codex.AddItem(new CodexEntry((CodexEntryType)item.type, item.id));
            }
        }

        /// <summary>
        /// Save inventory data to save object
        /// </summary>
        public void SaveInventories()
        {
            saveObject.items.Clear();
            //Write inventory data
            foreach (ItemStack i in inventoryItems)
                saveObject.items.Add(new ProfileItem(i.Id,i.Amount));

            saveObject.upgrades.Clear();
            //Write upgrades data
            foreach (ItemStack i in inventoryUpgrades)
                saveObject.upgrades.Add(new ProfileItem(i.Id,i.Amount));

            //Save Codex
            saveObject.codex.Clear();
            foreach (CodexEntryType type in System.Enum.GetValues(typeof(CodexEntryType)))
            {
                int id = (int)type;
                foreach (CodexEntry entry in this.codex.GetUnlockedEntries(type))
                    saveObject.codex.Add(new CodexItem(id,entry.ID));
            }
        }

        public static int ConvertCoinsToTotalCoins(int coins)
        {
            return coins / 5;
        }

        public void Clear(MonoBehaviour behaviour, System.Action<ProfileData> callback)
        {
            int totalCoins = TotalCoins;
            List<string> tutorial = Tutorials.Completed;
            saveObject = new SaveObject();
            ReloadInventories();
            saveObject.totalCoins = totalCoins;
            saveObject.tutorialData.Completed = tutorial;

            try
            {
                Profile.SaveProfile(callback);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                callback(this);
            }
        }
    }
}