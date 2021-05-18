using System;
using ManyTools.UnityExtended.Poolable;
using UnityEngine;
using ManyTools.Variables;
using SketchFleets;

public class PencilShell : PoolMember, ICollectible
{
    #region Private Fields

    [Header("Shell Parameters")]
    [SerializeField]
    private ColorReference shellColor = new ColorReference(Color.white);
    [SerializeField]
    private IntReference shellWorth = new IntReference(1);
    [SerializeField]
    private GameObject collectEffect;

    [Header("Player Reference Variables")]
    [SerializeField]
    [Tooltip("The amount of player shells owned by the player")]
    private IntReference playerShells;
    [SerializeField]
    [Tooltip("The color of the last shell collected by the player")]
    private ColorReference playerShellColor;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        // TODO: Replace the color in the mask shader's material property block
        GetComponent<SpriteRenderer>().color = shellColor;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Collect();
    }

    #endregion

    #region ICollectible Implementation

    /// <summary>
    /// Applies all necessary effects upon collection
    /// </summary>
    public void Collect()
    {
        // Adds value and updates color on player HUD
        playerShells.Value += shellWorth.Value;
        playerShellColor.Value = shellColor.Value;

        // If there is a collect effect, spawn it
        if (collectEffect != null)
        {
            Transform cachedTransform = transform;
            Instantiate(collectEffect, cachedTransform.position, cachedTransform.rotation);
        }

        // Sends its back to the pool
        Submerge();
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
        shellColor.Value = Color.white;
        shellWorth.Value = 1;
        
        base.Emerge(position, rotation);
    }

    #endregion
}