using System;
using System.Collections.Generic;
using SketchFleets.Data;

namespace SketchFleets.Inventory
{
    /// <summary>
    /// Class used to apply effects
    /// </summary>
    public class IngameEffectApplier
    {
        private static List<IngameEffect> appliedEffects = new List<IngameEffect>();
        public static Action OnEffectsChange;

        /// <summary>
        /// Clear item effects
        /// </summary>
        public static void Clear()
        {
            appliedEffects.Clear();
            if(OnEffectsChange != null)
                OnEffectsChange();
        }

        /// <summary>
        /// Tick all items
        /// </summary>
        /// <param name="deltaTime"></param>
        public static void TickItems(float deltaTime)
        {
            List<IngameEffect> remove = new List<IngameEffect>();
            foreach(IngameEffect effect in appliedEffects)
            {
                effect.duration -= deltaTime;

                if(effect.duration <= 0)
                {
                    remove.Add(effect);
                }
            }

            foreach(IngameEffect effect in remove)
                appliedEffects.Remove(effect);

            if(OnEffectsChange != null && remove.Count > 0)
                OnEffectsChange();
        }

        /// <summary>
        /// Apply items
        /// </summary>
        /// <param name="item"></param>
        public static void ApplyItem(Item item)
        {
            appliedEffects.Add(new IngameEffect(item.Effect,1));
            if(OnEffectsChange != null)
                OnEffectsChange();
        }

        /// <summary>
        /// Generate ingame effect result
        /// </summary>
        /// <param name="itemRegister"></param>
        /// <param name="upgradeRegister"></param>
        /// <returns></returns>
        public static IngameEffectResult GetResult(ShopObjectRegister itemRegister,ShopObjectRegister upgradeRegister)
        {
            IngameEffectResult result = new IngameEffectResult();

            //Apply upgrades
            foreach(ItemStack up in ProfileSystem.Profile.Data.inventoryUpgrades)
            {
                Upgrade upgrade = (Upgrade) upgradeRegister.items[up.Id];
                result.upgradeLifeIncrease += upgrade.UpgradeLifeIncrease * up.Amount;
                result.upgradeDamageIncrease += upgrade.UpgradeDamageIncrease * up.Amount;
                result.upgradeShieldIncrease += upgrade.UpgradeShieldIncrease * up.Amount;
                result.upgradeSpeedIncrease += upgrade.UpgradeSpeedIncrease * up.Amount;
            }

            //Apply item effects
            foreach (ItemStack stack in ProfileSystem.Profile.GetData().inventoryItems)
            {
                Item it = (Item) itemRegister.items[stack.Id];
                if(it.IsConsumable)
                    continue;
                ItemEffect effect = it.Effect;
                result.spawnSlotBonus += effect.SpawnSlotBonus * stack.Amount;
                result.spawnCooldownMultiplierBonus += effect.SpawnCooldownMultiplierBonus * stack.Amount;
                result.abilityCooldownMultiplierBonus += effect.AbilityCooldownMultiplierBonus * stack.Amount;
                
                result.maxHealthBonus += effect.MaxHealthBonus * stack.Amount;
                result.maxShieldBonus += effect.MaxShieldBonus * stack.Amount;
                result.damageMultiplierBonus += effect.DamageMultiplierBonus * stack.Amount;
                result.speedMultiplierBonus += effect.SpeedMultiplierBonus * stack.Amount;
                result.defenseBonus += effect.DefenseBonus * stack.Amount;
            }

            //Consumables
            foreach(IngameEffect a in appliedEffects)
            {
                ItemEffect effect = a.effect;

                result.spawnSlotBonus += effect.SpawnSlotBonus * a.amount;
                result.spawnCooldownMultiplierBonus += effect.SpawnCooldownMultiplierBonus * a.amount;
                result.abilityCooldownMultiplierBonus += effect.AbilityCooldownMultiplierBonus * a.amount;
                
                result.maxHealthBonus += effect.MaxHealthBonus * a.amount;
                result.maxShieldBonus += effect.MaxShieldBonus * a.amount;
                result.damageMultiplierBonus += effect.DamageMultiplierBonus * a.amount;
                result.speedMultiplierBonus += effect.SpeedMultiplierBonus * a.amount;
                result.defenseBonus += effect.DefenseBonus * a.amount;

                result.healthRegen += effect.HealthRegen;
                result.shieldRegen += effect.ShieldRegen;
            }

            return result;
        }
    }

    /// <summary>
    /// Ingame effect
    /// </summary>
    public class IngameEffect
    {
        public ItemEffect effect;
        public int amount;
        public float duration;

        public IngameEffect(ItemEffect effect,int amount)
        {
            this.effect = effect;
            this.amount = amount;
            this.duration = effect.Duration;
        }
    }

    /// <summary>
    /// Ingame effect result
    /// </summary>
    public class IngameEffectResult
    {
        public int upgradeLifeIncrease = 0;
        public int upgradeDamageIncrease = 0;
        public int upgradeShieldIncrease = 0;
        public int upgradeSpeedIncrease = 0;

        public float healthRegen = 0;
        public float shieldRegen = 0;
        public int spawnSlotBonus = 0;
        public int spawnCooldownMultiplierBonus = 0;
        public int abilityCooldownMultiplierBonus = 0;
        public float maxHealthBonus = 0;
        public float maxShieldBonus = 0;
        public float damageMultiplierBonus = 0;
        public float speedMultiplierBonus = 0;
        public float defenseBonus = 0;
    }
}