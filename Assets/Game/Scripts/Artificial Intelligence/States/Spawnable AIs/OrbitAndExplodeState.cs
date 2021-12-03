using ManyTools.Variables;
using SketchFleets.General;
using UnityEngine;

namespace SketchFleets.AI
{
    /// <summary>
    /// An AI state that orbits around the player and fires when he does so
    /// </summary>
    public sealed class OrbitAndExplodeState : BaseOrbitState
    {
        #region Private Fields

        [Header("Bullet Time Settings")]
        [SerializeField]
        private AnimationCurve timeByRealTime = new AnimationCurve();
        [SerializeField]
        private float bulletTimeDuration = 1.3f;
        
        [Header("Launch Settings")]
        [SerializeField]
        private FloatReference launchSpeedMultiplier = new FloatReference(5f);

        #endregion

        #region Properties

        public bool Explode { get; private set; }

        #endregion

        #region State Implementation

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!Explode)
            {
                base.StateUpdate();
            }
            else
            {
                Vector3 movement = Vector3.up *
                                   (AI.Ship.Attributes.Speed * Time.deltaTime * Time.timeScale * launchSpeedMultiplier);
                transform.Translate(movement);
            }
        }

        #endregion

        #region Unity Callbacks

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!Explode) return;

            if (other.gameObject.CompareTag("Player") ||
                other.gameObject.CompareTag("PlayerSpawn") ||
                other.gameObject.CompareTag("bullet"))
            {
                return;
            }

            Explode = false;
            AI.Ship.Die();
        }

        private void OnEnable()
        {
            Explode = false;
        }

        private void OnDisable()
        {
            Explode = false;

            if (AI == null) return;
            LevelManager.Instance.BulletTimeManager.StartBulletTime(timeByRealTime, bulletTimeDuration);
            AI.Ship.Fire();
        }

        #endregion

        #region Public Methods

        public void LightFuse()
        {
            Explode = true;
        }

        public void LookAtTarget(Vector2 pos) //TODO: Remove
        {
            AI.Ship.Look(pos);
            Explode = true;
        }

        #endregion

        #region Private Methods

#if UNITY_EDITOR
        [ContextMenu("Add Fire Transforms for Bomb")]
        private void AddCircularFireTransforms()
        {
            int totalTransformCount = 40;
            
            for (int index = 0; index < totalTransformCount; index++)
            {
                GameObject fireTransform = new GameObject($"Fire Transform ({index})");
                fireTransform.transform.SetParent(transform);
                fireTransform.transform.localPosition = Vector3.zero;
                fireTransform.transform.localRotation = Quaternion.Euler(0, 0, 360f / totalTransformCount * index);
                fireTransform.transform.Translate(0f, 2.5f, 0f, Space.Self);
            }
        }
#endif

        #endregion
    }
}