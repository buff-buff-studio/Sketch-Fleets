using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// A class that holds attributes about bullets
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.bulletAttributesFileName, menuName = CreateMenus.bulletAttributesMenuName,
        order = CreateMenus.bulletAttributesOrder)]
    public sealed class BulletAttributes : Attributes
    {
        #region Private Fields

        [Header("Attributes")]
        [Tooltip("How much damage the bullet does if it hits an enemy directly.")]
        [SerializeField]
        private FloatReference directDamage;
        [Tooltip("How fast the bullet moves.")]
        [SerializeField]
        private FloatReference speed;
        [Tooltip("How large should the impact radius be for indirect damage.")]
        [SerializeField]
        private FloatReference impactRadius;
        [Tooltip("How much damage should the bullet do if it hits an enemy indirectly.")]
        [SerializeField]
        private FloatReference indirectDamage;

        [Header("Visual Effects")]
        [Tooltip("The effect spawned when the bullet is fired.")]
        [SerializeField]
        private GameObject fireEffect;
        [Tooltip("The effect spawned when the bullet hits something.")]
        [SerializeField]
        private GameObject hitEffect;

        #endregion

        #region Properties

        public GameObject HitEffect => hitEffect;

        public GameObject FireEffect => fireEffect;

        public FloatReference IndirectDamage => indirectDamage;

        public FloatReference ImpactRadius => impactRadius;

        public FloatReference Speed => speed;

        public FloatReference DirectDamage => directDamage;

        #endregion

    }
}