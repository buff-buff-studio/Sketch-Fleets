﻿using ManyTools.Variables;
using UnityEngine;

namespace SketchFleets.Data
{
    /// <summary>
    /// The attributes for an obstacle
    /// </summary>
    [CreateAssetMenu(fileName = CreateMenus.obstacleFileName, 
        menuName = CreateMenus.obstacleMenuName, order = CreateMenus.obstacleAttributesOrder)]
    public sealed class ObstacleAttributes : Attributes
    {
        #region Private Fields

        [Header("Attributes")]
        [SerializeField]
        [Tooltip("The damage taken upon colliding with this obstacle. Negative damage indicates healing.")]
        private FloatReference collisionDamage = new FloatReference(100f);
        [SerializeField]
        [Tooltip("Whether the obstacle should collide with the player only")]
        private BoolReference collideWithPlayerOnly = new BoolReference(false);
        [SerializeField]
        [Tooltip("The movement of the obstacle per frame.")]
        private Vector2Reference motion = new Vector2Reference(Vector2.zero);
        [SerializeField]
        [Tooltip("The maximum health of the obstacle.")]
        private FloatReference maxHealth = new FloatReference(100f);
        [SerializeField]
        [Tooltip("The effect created when the obstacle is destroyed")]
        private GameObject deathEffect;
        [SerializeField]
        [Tooltip("Whether the obstacle should display a warning when spawning")]
        private BoolReference warnOnSpawn = new BoolReference(false);

        #endregion

        #region Properties

        public FloatReference CollisionDamage => collisionDamage;

        public Vector2Reference Motion => motion;

        public FloatReference MaxHealth => maxHealth;

        public GameObject DeathEffect => deathEffect;

        public bool IsStatic => motion.Value == Vector2.zero;

        public BoolReference WarnOnSpawn => warnOnSpawn;

        public BoolReference CollideWithPlayerOnly => collideWithPlayerOnly;

        #endregion

    }
}