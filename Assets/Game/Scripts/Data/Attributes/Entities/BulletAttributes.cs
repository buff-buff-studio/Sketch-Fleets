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
        private FloatReference directDamage = new FloatReference(0);
        [Tooltip("What is the most damage can vary per shot?")]
        [SerializeField]
        private FloatReference maxDamageVariation = new FloatReference(0);
        [Tooltip("How fast the bullet moves.")]
        [SerializeField]
        private FloatReference speed = new FloatReference(10);
        [Tooltip("How large should the impact radius be for indirect damage.")]
        [SerializeField]
        private FloatReference impactRadius = new FloatReference(0);
        [Tooltip("How much damage should the bullet do if it hits an enemy indirectly.")]
        [SerializeField]
        private FloatReference indirectDamage = new FloatReference(5);
        [Tooltip("Whether the bullet should not deal damage to player and player-related ships.")]
        [SerializeField]
        private BoolReference ignorePlayer = new BoolReference(false);
        [Tooltip("How many times the bullet will bounce.")]
        [SerializeField]
        private FloatReference maxBounce = new FloatReference(0);        
        [Tooltip("Time the target will be stationary in seconds.")]
        [SerializeField]
        private FloatReference downtime = new FloatReference(0);        
        [Tooltip("How many shots does the target have to take to stop.")]
        [SerializeField]
        private IntReference hitsLock = new IntReference(0);
        [SerializeField]
        [Tooltip("The maximum angle variance on firing a shot")]
        private FloatReference angleJitter = new FloatReference(8f);
        [SerializeField]
        [Tooltip("The cooldown when firing a bullet of this kind")]
        private FloatReference cooldown = new FloatReference(0.5f);

        [Header("Special Effects")]
        [Tooltip("The sound effect played when the bullet is shot")]
        [SerializeField]
        private AudioClip fireSound;
        [Tooltip("The pitch variation of the sound effect")]
        [SerializeField]
        private FloatReference pitchVariation = new FloatReference(0.25f);
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

        public BoolReference IgnorePlayer => ignorePlayer;
        
        public FloatReference MaxBounce => maxBounce;
        
        public FloatReference Downtime => downtime;
        
        public IntReference HitsLock => hitsLock;

        public FloatReference AngleJitter => angleJitter;

        public FloatReference Cooldown => cooldown;

        public AudioClip FireSound => fireSound;

        public FloatReference MaxDamageVariation => maxDamageVariation;

        public FloatReference PitchVariation => pitchVariation;

        #endregion

    }
}