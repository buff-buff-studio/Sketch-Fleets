using System;
using System.Collections;
using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Entities;
using UnityEngine;
using Random = UnityEngine.Random;
using SketchFleets.Systems.Codex;

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
        protected Transform lockParent;

        protected int lockHit = 0;
        protected bool isLocked = false;
        
        private bool isDead = false;

        #endregion

        #region Private Fields

        protected FloatReference currentHealth = new FloatReference(0f);
        protected FloatReference currentShield = new FloatReference(0f);
        
        protected readonly int redMultiplier = Shader.PropertyToID("_redMul");
        protected readonly int blueMultiplier = Shader.PropertyToID("_bluMul");
        protected readonly int greenMultiplier = Shader.PropertyToID("_greMul");
        private readonly int blinkColor = Shader.PropertyToID("_blinkColor");
        
        protected Color shipColor;

        #endregion

        #region Properties
        
        public Action TookDamage { get; set; }

        public FloatReference CurrentShield => currentShield;

        public FloatReference CurrentHealth => currentHealth;

        public virtual FloatReference MaxHealth => Attributes.MaxHealth;

        public T Attributes => attributes;

        #endregion

        #region IDamageable Implementation

        public virtual void Damage(float amount, bool makeInvincible = false, bool piercing = false)
        {
            // Rejects damage during invincibility time or death
            if (collisionTimer > 0 || isDead) return;

            // Adds invincibility time if necessary
            if (makeInvincible)
            {
                MakeInvulnerable(Attributes.InvincibilityTime);
            }

            // Deals damage to shields first
            if (currentShield.Value > 0 && !piercing)
            {
                DamageShields(RawToEffectiveDamage(amount, false));
            }
            else
            {
                // Reduces health
                DamageHealth(RawToEffectiveDamage(amount, piercing));
            }
            
            TookDamage?.Invoke();
           
            // Applies shield regen cooldown 
            shieldRegenTimer = Attributes.ShieldRegenDelay;

            PlayHitEffects();

            // Dies if necessary
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Damages the health by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage the health for</param>
        protected virtual void DamageHealth(float amount)
        {
            currentHealth.Value -= amount;
        }
        
        /// <summary>
        /// Damages shields and overflows any damage to the health
        /// </summary>
        /// <param name="amount">The amount to damage the shields for</param>
        protected virtual void DamageShields(float amount)
        {
            float damageResult = currentShield.Value - amount;

            if (damageResult < 0)
            {
                currentShield.Value = 0f;
                DamageHealth(damageResult * -1f);
            }
            else
            {
                currentShield.Value = damageResult;
            }
        }
        /// <summary>
        /// Gets given damage as effective damage; I.E, after factoring defense and other bonuses
        /// </summary>
        /// <returns></returns>
        protected virtual float RawToEffectiveDamage(float rawDamage, bool piercingDamage)
        {
            return piercingDamage ? rawDamage : rawDamage / Attributes.Defense;
        }
        
        /// <summary>
        /// Makes the ship invulnerable for the given amount of time
        /// </summary>
        protected virtual void MakeInvulnerable(float invulnerabilityTime)
        {
            collisionTimer = invulnerabilityTime;
        }
        
        /// <summary>
        /// Plays visual effects related to taking damage
        /// </summary>
        protected virtual void PlayHitEffects()
        {
            // Plays hit sounds
            soundSource.clip = Attributes.HitSound;
            
            if (soundSource.clip != null)
            {
                soundSource.Play();
            }
            
            // Flashes red
            Color tempColor = spriteRenderer.material.GetColor(blinkColor);
            tempColor.a = 1f;
            spriteRenderer.material.SetColor(blinkColor, tempColor);
        }

        public virtual void Heal(float amount)
        {
            currentHealth.Value = Mathf.Min(attributes.MaxHealth, currentHealth + amount);

            Transform cachedTransform = transform;

            if (Attributes.HealEffect != null)
            {
                PoolManager.Instance.Request(Attributes.HealEffect).
                    Emerge(cachedTransform.position, cachedTransform.rotation);
            }
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
        protected virtual void Start()
        {
            shipColor = spriteRenderer.material.GetColor(redMultiplier);
            ResetInstanceVariables();
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
            if (fireTimer > 0f || isLocked) return;

            for (int index = 0, upper = bulletSpawnPoints.Length; index < upper; index++)
            {
                PoolMember bullet = PoolManager.Instance.Request(attributes.Fire.Prefab);
                bullet.Emerge(bulletSpawnPoints[index].position, bulletSpawnPoints[index].rotation);

                bullet.GetComponent<SpriteRenderer>().color = spriteRenderer.material.GetColor(redMultiplier);
                bullet.transform.Rotate(0f, 0f,
                    Random.Range(Attributes.Fire.AngleJitter * -1f, Attributes.Fire.AngleJitter));
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
            if (Mathf.Approximately(0f, Time.timeScale) || isLocked) return;

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
            if (isDead) return;

            if (Attributes.DeathEffect != null)
            {
                PoolMember death = PoolManager.Instance.Request(Attributes.DeathEffect);
                death.Emerge(transform.position, Quaternion.identity);
                
                ParticleSystem deathCache = death.GetComponent<ParticleSystem>();
                deathCache.startColor = shipColor;
            }

            if (deathEvent != null)
            {
                deathEvent.Invoke();
            }

            DropLoot();

            Submerge();

            isDead = true;
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
            if (Attributes.ShellDrop != null)
            {
                DropShells();
            }

            if (Random.Range(0f, 1f) <= Attributes.CodexDropChance && 
                ProfileSystem.Profile.Data.codex.SearchItem(new Inventory.CodexEntry(Inventory.CodexEntryType.Ship,CodexListener.GetRegisterID(CodexListener.Instance.ShipRegister,attributes))) == 0)
            {
                DropCodexEntry();
            }
        }

        /// <summary>
        /// Drops the ship's pencil shells
        /// </summary>
        protected virtual void DropShells()
        {
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

                drop.TryGetComponent(out PencilShell shell);
                shell.SetDropColor(Attributes.ShipColor);
            }
        }
        
        /// <summary>
        /// Drops the ship's codex entry
        /// </summary>
        protected virtual void DropCodexEntry()
        {
            Transform cachedTransform = transform;

            PoolMember codexEntry = PoolManager.Instance.Request(Attributes.CodexEntryTemplate);
            codexEntry.Emerge(cachedTransform.position, cachedTransform.rotation);

            codexEntry.TryGetComponent(out CodexEntryDrop drop);

            drop.Entry = Attributes;
        }

        /// <summary>
        /// Resets all instance-specific variables
        /// </summary>
        protected virtual void ResetInstanceVariables()
        {
            currentHealth.Value = attributes.MaxHealth.Value;
            currentShield.Value = attributes.MaxShield.Value;
            fireTimer = 0;

            isDead = false;
        }

        #endregion

        protected virtual IEnumerator LockState(float lockTime)
        {
            isLocked = true;
            
            yield return new WaitForSeconds(lockTime);
            
            isLocked = false;
        }

        public IEnumerator ContinuousDamage(float damage, float time)
        {
            for (int i = 1; i <= Mathf.Abs(time); i++)
            {
                Damage(damage/i);
                yield return new WaitForSeconds(1);
            }
        }
    }
}