using System;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using UnityEngine;
using SketchFleets.Data;
using SketchFleets;
using Random = UnityEngine.Random;

/// <summary>
/// A class that controls a bullet and its behaviour
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BulletController : PoolMember
{
    #region Private Fields

    [SerializeField, RequiredField()]
    private BulletAttributes attributes;
    [SerializeField, RequiredField()]
    private AudioSource soundSource;

    private bool hasFireEffect;
    private float damageMultiplier = 1f;
    private float damageIncrease = 0f;

    #endregion

    public float PlayerBuletVelocity;

    #region Properties

    public BulletAttributes Attributes => attributes;

    public float DamageMultiplier
    {
        get => damageMultiplier;
        set => damageMultiplier = value;
    }

    public float DamageIncrease
    {
        get => damageIncrease;
        set => damageIncrease = value;
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
        base.Emerge(position, rotation);

        soundSource.pitch = Random.Range(1 - Attributes.PitchVariation, 1 + Attributes.PitchVariation);

        soundSource.clip = Attributes.FireSound;
        soundSource.pitch = Random.Range(1 - Attributes.PitchVariation, 1 + Attributes.PitchVariation);
        soundSource.Play();

        if (hasFireEffect)
        {
            PoolManager.Instance.Request(Attributes.FireEffect).Emerge(transform.position, Quaternion.identity);
        }
    }

    #endregion

    #region Unity Callbacks

    protected virtual void Start()
    {
        hasFireEffect = Attributes.FireEffect != null;
        PlayerBuletVelocity = 0;
    }

    protected virtual void Update()
    {
        if(PlayerBuletVelocity == 0)
            Move(Vector3.up * Attributes.Speed, Space.Self);
        else
            Move(Vector3.up * PlayerBuletVelocity, Space.Self);
    }

    protected void Move(Vector3 pos, Space space)
    {
        transform.Translate(pos * Time.deltaTime, space);
        //transform.Translate(Vector3.up * Time.deltaTime * Attributes.Speed, Space.Self);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EndMap"))
        {
            Submerge();
            return;
        }

        if (!col.isTrigger) return;
        Hit(col);
    }

    private void Reset()
    {
        soundSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Performs operations related to hitting something
    /// </summary>
    /// <param name="directHit">The collider that directly hit the bullet</param>
    private void Hit(Collider2D directHit)
    {
        DealDamageToTarget(true, directHit.gameObject);

        // If the bullet has no area effects, stop here
        if (Mathf.Approximately(Attributes.IndirectDamage, 0f) ||
            Mathf.Approximately(Attributes.ImpactRadius, 0f))
        {
            return;
        }

        // Caches colliders in the area
        Collider2D[] colliders = new Collider2D[0];
        Physics2D.OverlapCircleNonAlloc(transform.position, Attributes.ImpactRadius, colliders);

        // Applies damage to every IDamageable in the radius
        for (int index = 0, upper = colliders.Length; index < upper; index++)
        {
            // If collected collider isn't a trigger, skip
            if (!colliders[index].isTrigger) continue;
            // If the collected collider is the direct hit, skip
            if (directHit.gameObject == colliders[index].gameObject) continue;

            DealDamageToTarget(false, directHit.gameObject);
        }

        if (Attributes.HitEffect != null)
        {
            PoolManager.Instance.Request(Attributes.HitEffect).Emerge(transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Deals damage to a given target, taking into account whether it is the player or not
    /// </summary>
    /// <param name="directDamage">Whether the damage is direct or indirect</param>
    /// <param name="target">The target to deal the damage to</param>
    protected virtual void DealDamageToTarget(bool directDamage, GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
        {
            if (Attributes.IgnorePlayer) return;
            target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
        }
        else
        {
            target.GetComponent<IDamageable>()?.Damage(GetDamage(directDamage));
        }

        Submerge();
    }

    /// <summary>
    /// Gets the damage of the bullet
    /// </summary>
    /// <returns>The damage of the bullet</returns>
    protected float GetDamage(bool direct)
    {
        float damageVariation = Random.Range(0, Attributes.MaxDamageVariation);
        float baseDamage = direct ? Attributes.DirectDamage : Attributes.IndirectDamage;
        return baseDamage * DamageMultiplier + damageVariation + DamageIncrease;
    }

    #endregion
}