using ManyTools.UnityExtended.Editor;
using ManyTools.Variables;
using SketchFleets.Inventory;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that contains data about a ship's attributes
    /// </summary>
    [CreateAssetMenu(order = CreateMenus.shipAttributesOrder, fileName = CreateMenus.shipAttributesFileName, 
        menuName = CreateMenus.shipAttributesMenuName)]
    public class ShipAttributes : Attributes
    {
        #region Protected Fields

        [Header("Attributes")]
        [Tooltip("The ship's codex entry rarity")]
        [SerializeField]
        protected CodexEntryRarity codexRarity;
        [Tooltip("The ship's faction for targeting purposes")]
        [SerializeField]
        protected Faction shipFaction;
        [Tooltip("The ship's color.")]
        [SerializeField]
        protected ColorReference shipColor = new ColorReference(new Color());
        [SerializeField]
        protected FloatReference maxHealth;
        [Tooltip("The maximum amount of points the shield can have.")]
        [SerializeField]
        protected FloatReference maxShield;
        [Tooltip("The amount of shield points regenerated per second.")]
        [SerializeField]
        protected FloatReference shieldRegen = new FloatReference(1f);
        [Tooltip("The delay in seconds before regenerating the shield.")]
        [SerializeField]
        protected FloatReference shieldRegenDelay = new FloatReference(5f);
        [Tooltip("The multiplier for the attack's damage.")]
        [SerializeField]
        protected FloatReference damageMultiplier = new FloatReference(1);
        [SerializeField]
        protected FloatReference speed;
        [Tooltip("An inverse multiplier for how much damage the ship takes with an attack.")]
        [SerializeField]
        protected FloatReference defense = new FloatReference(0);
        [Tooltip("How much a ship will take when colliding with this ship.")]
        [SerializeField]
        protected FloatReference collisionDamage = new FloatReference(100f);
        [Tooltip("How long the ship should be invincible after taking invincibility-triggering damage.")]
        [SerializeField]
        protected FloatReference invincibilityTime = new FloatReference(1.3f);

        [Header("Drops")]
        [Tooltip("The minimum and maximum amount of shells dropped")]
        [SerializeField]
        protected Vector2Reference dropMinMaxCount = new Vector2Reference(new Vector2(0, 1));
        [Tooltip("The prefab of the dropped shell")]
        [SerializeField]
        protected GameObject shellDrop;
        [Tooltip("The template object for codex entries")]
        [SerializeField]
        protected GameObject codexEntryTemplate;
        [Tooltip("The chance of dropping the codex entry for this ship, in percentage, from 0 to 1")]
        [SerializeField]
        protected FloatReference codexDropChance = new FloatReference(0.02f);

        [Header("References")]
        [Tooltip("The prefab spawned by the ship by an attack.")]
        [SerializeField, RequiredField()]
        protected BulletAttributes fire;

        [Header("Special Effects")]
        [Tooltip("The sound effect when the ship gets hit")]
        [SerializeField]
        private AudioClip hitSound;
        [Tooltip("The effect spawned when the ship dies")]
        [SerializeField]
        private GameObject deathEffect;
        [Tooltip("The effect when the ship gets healed")]
        [SerializeField]
        private GameObject healEffect;

        #endregion

        #region Properties

        public ColorReference ShipColor => shipColor;

        public FloatReference MaxHealth => maxHealth;

        public FloatReference MaxShield => maxShield;

        public FloatReference DamageMultiplier => damageMultiplier;

        public FloatReference Speed => speed;

        public BulletAttributes Fire => fire;

        public FloatReference Defense => defense;

        public AudioClip HitSound => hitSound;

        public GameObject DeathEffect => deathEffect;

        public Vector2Reference DropMinMaxCount => dropMinMaxCount;

        public FloatReference ShieldRegen => shieldRegen;

        public FloatReference ShieldRegenDelay => shieldRegenDelay;

        public FloatReference CollisionDamage => collisionDamage;

        public GameObject ShellDrop => shellDrop;

        public FloatReference InvincibilityTime => invincibilityTime;

        public GameObject HealEffect => healEffect;

        public GameObject CodexEntryTemplate => codexEntryTemplate;

        public FloatReference CodexDropChance => codexDropChance;

        public CodexEntryRarity CodexRarity => codexRarity;
        
        public Faction ShipFaction => shipFaction;

        #endregion

        #region Public Enums

        public enum Faction
        {
            Friendly,
            Hostile,
            Neutral
        }

        #endregion
    }
}
