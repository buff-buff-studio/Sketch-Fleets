using ManyTools.Variables;
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
        [SerializeField]
        protected FloatReference maxHealth;
        [Tooltip("The maximum amount of points the shield can have.")]
        [SerializeField]
        protected FloatReference maxShield;
        [Tooltip("The multiplier for the attack's damage.")]
        [SerializeField]
        protected FloatReference damageMultiplier = new FloatReference(1);
        [SerializeField]
        protected FloatReference speed;
        [Tooltip("An inverse multiplier for how much damage the ship takes with an attack.")]
        [SerializeField]
        protected FloatReference defense = new FloatReference(0);

        [Header("References")]
        [Tooltip("The prefab spawned by the ship by an attack.")]
        [SerializeField]
        protected BulletAttributes fire;

        [Header("Special Effects")]
        [Tooltip("The sound effect when the ship gets hit")]
        [SerializeField]
        private AudioClip hitSound;
        [Tooltip("The effect spawned when the ship dies")]
        [SerializeField]
        private GameObject deathEffect;
        
        #endregion

        #region Properties

        public FloatReference MaxHealth => maxHealth;

        public FloatReference MaxShield => maxShield;

        public FloatReference DamageMultiplier => damageMultiplier;

        public FloatReference Speed => speed;

        public BulletAttributes Fire => fire;

        public FloatReference Defense => defense;

        public AudioClip HitSound => hitSound;

        public GameObject DeathEffect => deathEffect;

        #endregion
    }
}
