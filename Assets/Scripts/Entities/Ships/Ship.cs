using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets
{
    /// <summary>
    /// A class that controls 
    /// </summary>
    /// <typeparam name="T">An attribute data structure that inherits from ShipAttributes</typeparam>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ship<T> : PoolMember, IHealthVerifiable, IDamageable where T : ShipAttributes
    {
        #region Protected Fields

        [Header("Ship Properties")]
        [SerializeField, RequiredField()]
        protected T attributes;
        [SerializeField, RequiredField()]
        protected Transform[] bulletSpawnPoints;
        [SerializeField]
        private GameEvent deathEvent;
        [Header("Component Dependencies")]
        [SerializeField, RequiredField()]
        protected AudioSource soundSource;
        [SerializeField, RequiredField()]
        protected SpriteRenderer spriteRenderer;

        protected float fireTimer;
        protected float shieldRegenTimer;
        protected float collisionTimer;

        #endregion

        #region Private Fields

        private FloatReference currentHealth = new FloatReference(0f);
        private FloatReference currentShield = new FloatReference(0f);
        private readonly int blinkColor = Shader.PropertyToID("_blinkColor");

        #endregion

        #region Properties

        public FloatReference CurrentShield => currentShield;

        public FloatReference CurrentHealth => currentHealth;

        public FloatReference MaxHealth => Attributes.MaxHealth;

        public T Attributes => attributes;

        #endregion

        #region IDamageable Implementation

        public void Damage(float amount, bool makeInvincible = false, bool piercing = false)
        {
            // Rejects damage during invincibility time
            if (collisionTimer > 0) return;

            // Adds invincibility time if necessary
            if (makeInvincible)
            {
                collisionTimer = Attributes.InvincibilityTime;
            }

            // Calculates effective damaged based on defense
            float actualDamage;
            actualDamage = piercing ? actualDamage = amount : actualDamage = (amount / Attributes.Defense);

            // Deals damage to shields first
            if (currentShield.Value > 0 && !piercing)
            {
                // Damages shield
                float tempShield = CurrentShield.Value -= actualDamage;
                CurrentShield.Value = Mathf.Max(tempShield, 0);

                // Applies excess damage to health
                if (tempShield < 0)
                {
                    // Reduces health
                    currentHealth.Value -= tempShield;
                }
            }
            else
            {
                // Reduces health
                currentHealth.Value -= actualDamage;
            }

            // Applies shield regen cooldown 
            shieldRegenTimer = Attributes.ShieldRegenDelay;

            // Plays hit sounds
            soundSource.clip = Attributes.HitSound;
            if (soundSource.clip != null)
            {
                soundSource.Play();
            }

            // Gets and sets material property block's blink color
            // TODO: Use material property blocks instead once we figure out how to
            // TODO: get the damn ShaderGraph to generate them
            // spriteRenderer.sharedMaterial.GetPropertyBlock(propertyBlock);
            //
            // Debug.Log(propertyBlock.GetColor("_redMul"));
            //
            // Color tempColor = propertyBlock.GetColor(blinkColor);
            // //Debug.Log(propertyBlock.GetColor(blinkColor));
            // tempColor.a = 1f;
            // propertyBlock.SetColor(blinkColor, tempColor);
            //
            // spriteRenderer.SetPropertyBlock(propertyBlock);

            Color tempColor = spriteRenderer.material.GetColor(blinkColor);
            tempColor.a = 1f;
            spriteRenderer.material.SetColor(blinkColor, tempColor);

            // Dies if necessary
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            currentHealth.Value = Mathf.Min(attributes.MaxHealth, currentHealth + amount);
        }

        #endregion

        #region PoolMember Overrides

        /// <summary>
        /// Emerges the Poolable object from the pool
        /// </summary>
        /// <param name="position">The position at which to emerge the object</param>
        /// <param name="rotation">The rotation to emerge the object with</param>
        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            ResetInstanceVariables();

            base.Emerge(position, rotation);
        }

        #endregion

        #region Unity Callbacks

        // Start is called before the first update
        protected virtual void Awake()
        {
            // Gets blink color hash id
            //propertyBlock = new MaterialPropertyBlock();
            //spriteRenderer.GetPropertyBlock(propertyBlock);

            // Initializes health and shields
            currentHealth.Value = attributes.MaxHealth.Value;
            currentShield.Value = attributes.MaxShield.Value;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            collisionTimer -= Time.deltaTime;
            fireTimer -= Time.deltaTime;
            RegenShield();

            // TODO: Remove this workaround once we figure out whether MPBs are supported in ShaderGraph yet
            Color tempColor = spriteRenderer.material.GetColor(blinkColor);
            tempColor.a = Mathf.Max(tempColor.a - Time.deltaTime * 3f, 0);
            spriteRenderer.material.SetColor(blinkColor, tempColor);
        }

        protected void Reset()
        {
            soundSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CompareTag(other.tag)) return;
            
            if (other.CompareTag("Enemy") ||
                other.CompareTag("PlayerSpawn") ||
                other.CompareTag("Player") ||
                other.CompareTag("Obstacle"))
            {
                other.GetComponent<IDamageable>()?.Damage(Attributes.CollisionDamage, true);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fires the ship's weapons
        /// </summary>
        public virtual void Fire()
        {
            if (fireTimer > 0f) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(attributes.Fire.Prefab);
                bullet.Emerge(bulletSpawnPoints[index].position, bulletSpawnPoints[index].rotation);

                bullet.transform.Rotate(0f, 0f,
                    Random.Range(Attributes.Fire.AngleJitter * -1f, Attributes.Fire.AngleJitter));

                bullet.GetComponent<BulletController>().BarrelAttributes = Attributes;
            }

            fireTimer = Attributes.Fire.Cooldown;
        }

        /// <summary>
        /// Makes the ship look at a point
        /// </summary>
        /// <param name="target">Where to look at</param>
        public virtual void Look(Vector2 target)
        {
            // Workaround, this should be done using a proper Pausing interface
            if (Mathf.Approximately(0f, Time.timeScale)) return;

            // This doesn't really solve the problem. The transform should be a member variable
            // to avoid the constant marshalling
            Transform transformCache = transform;
            transformCache.up = (Vector3)target - transformCache.position;
        }

        /// <summary>
        /// Makes the ship die
        /// </summary>
        public virtual void Die()
        {
            if(!gameObject.activeSelf) return;

            if (Attributes.DeathEffect != null)
            {
                PoolManager.Instance.Request(Attributes.DeathEffect).Emerge(transform.position, Quaternion.identity);
            }

            if (deathEvent != null)
            {
                deathEvent.Invoke();
            }

            DropLoot();

            Submerge();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Regenerates the ship's shields
        /// </summary>
        protected virtual void RegenShield()
        {
            // Decrements the regen timer
            shieldRegenTimer -= Time.deltaTime;

            // If the regen timer is not over or there is no regen, end it here
            if (shieldRegenTimer > 0 || Mathf.Approximately(Attributes.ShieldRegen, 0)) return;

            // Regen shield
            CurrentShield.Value = Mathf.Min(Attributes.MaxShield,
                CurrentShield.Value + Attributes.ShieldRegen * Time.deltaTime);
        }

        /// <summary>
        /// Drops the ship's loot
        /// </summary>
        protected virtual void DropLoot()
        {
            // Drops pencil shells
            if (Attributes.ShellDrop == null) return;

            int dropCount = Mathf.RoundToInt(
                Random.Range(Attributes.DropMinMaxCount.Value.x, Attributes.DropMinMaxCount.Value.y));

            for (int index = 0; index < dropCount; index++)
            {
                // Generates randomized rotations and positions
                Vector3 randomRotation = new Vector3(0f, 0f, Random.Range(0, 359f));
                Vector3 dropPosition =
                    new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);

                // Instantiates and colors shell drop
                PoolMember drop = PoolManager.Instance.Request(Attributes.ShellDrop);
                drop.Emerge(transform.position + dropPosition, Quaternion.Euler(randomRotation));

                drop.GetComponent<PencilShell>().SetDropColor(Attributes.ShipColor);
            }
        }

        /// <summary>
        /// Resets all instance-specific variables
        /// </summary>
        protected virtual void ResetInstanceVariables()
        {
            currentHealth.Value = attributes.MaxHealth.Value;
            currentShield.Value = attributes.MaxShield.Value;
            fireTimer = 0;
        }

        #endregion
    }
}