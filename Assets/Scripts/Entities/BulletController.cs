using UnityEngine;
using SketchFleets.Data;
using SketchFleets;

/// <summary>
/// A class that controls a bullet and its behaviour
/// </summary>
public class BulletController : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private BulletAttributes attributes;
    private ShipAttributes barrelAttributes;

    #endregion

    #region Properties

    public BulletAttributes Attributes => attributes;

    public ShipAttributes BarrelAttributes
    {
        get => barrelAttributes;
        set => barrelAttributes = value;
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // TODO: Replace by pooling call
        Destroy(gameObject, 10f);

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
        if (!col.isTrigger) return;
        Hit(col);
    }

    #endregion

    #region Private Fields

    /// <summary>
    /// Performs operations related to hitting something
    /// </summary>
    /// <param name="directHit">The collider that directly hit the bullet</param>
    private void Hit(Collider2D directHit)
    {
        DealDamageToTarget(Attributes.DirectDamage, directHit.gameObject);

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

            DealDamageToTarget(Attributes.IndirectDamage, directHit.gameObject);
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
            target.GetComponent<IDamageable>()?.Damage(damageAmount * barrelAttributes.DamageMultiplier);
        }
        else
        {
            target.GetComponent<IDamageable>()?.Damage(damageAmount * barrelAttributes.DamageMultiplier);
        }

        Destroy(gameObject);
    }

    #endregion
}