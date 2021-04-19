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
        #region Private Fields

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
        [Tooltip("The multiplier for the movement speed.")]
        [SerializeField]
        protected FloatReference speedMultiplier = new FloatReference(1);
        [Tooltip("An inverse multiplier for how much damage the ship takes with an attack.")]
        [SerializeField]
        protected FloatReference defense = new FloatReference(0);

        [Tooltip("The prefab spawned by the ship by an attack.")]
        [SerializeField]
        protected GameObject fire;

        #endregion

        #region Properties

        public FloatReference MaxHealth
        {
            get => maxHealth;
        }

        public FloatReference MaxShield
        {
            get => maxShield;
        }

        public FloatReference DamageMultiplier
        {
            get => damageMultiplier;
        }

        public FloatReference Speed
        {
            get => speed;
        }

        public FloatReference SpeedMultiplier
        {
            get => speedMultiplier;
        }

        public GameObject Fire
        {
            get => fire;
        }

        #endregion
    }
}
