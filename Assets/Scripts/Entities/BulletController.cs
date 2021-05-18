using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using UnityEngine;
using SketchFleets.Data;
using SketchFleets;

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

    #endregion

    #region Properties

    public BulletAttributes Attributes => attributes;

    public ShipAttributes BarrelAttributes { get; set; }

    #endregion

    #region PoolMember Overrides

    /// <summary>
    /// Emerges the Poolable object from the pool
    /// </summary>
    /// <param name="position">The position at which to emerge the object</param>
    /// <param name="rotation">The rotation to emerge the object with</param>
    public override void Emerge(Vector3 position, Quaternion rotation)
    {
        soundSource.pitch = Random.Range(1 - Attributes.PitchVariation, 1 + Attributes.PitchVariation);
        base.Emerge(position, rotation);
    }

    #endregion
    
    #region Unity Callbacks

    private void Start()
    {
        if (Attributes.FireSound != null)
        {
            soundSource.clip = Attributes.FireSound;
            soundSource.pitch = Random.Range(1 - Attributes.PitchVariation, 1 + Attributes.PitchVariation);
            soundSource.Play();
        }

        if (Attributes.FireEffect != null)
        {
            Instantiate(Attributes.FireEffect, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * Attributes.Speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D col)
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

    #region Private Fields

    /// <summary>
    /// Performs operations related to hitting something
    /// </summary>
    /// <param name="directHit">The collider that directly hit the bullet</param>
    private void Hit(Collider2D directHit)
    {
        float damageVariation = Random.Range(0, Attributes.MaxDamageVariation);        
        
        DealDamageToTarget(Attributes.DirectDamage + damageVariation, directHit.gameObject);

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

            DealDamageToTarget(Attributes.IndirectDamage + damageVariation, directHit.gameObject);
        }

        if (Attributes.HitEffect != null)
        {
            Instantiate(Attributes.HitEffect, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Deals damage to a given target, taking into account whether it is the player or not
    /// </summary>
    /// <param name="damageAmount">The amount of damage to deal</param>
    /// <param name="target">The target to deal the damage to</param>
    private void DealDamageToTarget(float damageAmount, GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("PlayerSpawn"))
        {
            if (Attributes.IgnorePlayer) return;
            target.GetComponent<IDamageable>()?.Damage(damageAmount * BarrelAttributes.DamageMultiplier);
        }
        else
        {
            target.GetComponent<IDamageable>()?.Damage(damageAmount * BarrelAttributes.DamageMultiplier);
        }

        Submerge();
    }

    #endregion
}