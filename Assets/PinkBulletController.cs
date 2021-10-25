using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets
{
    public class PinkBulletController : BulletController
    {
        [SerializeField]
        private FloatReference bounceCooldown = new FloatReference(4f);
        private Transform cachedTransform;
        private Vector3 moveDirection;
        private float bounceTimer;
        protected bool CanBounce => bounceTimer <= 0f;

        private void Awake()
        {
            cachedTransform = transform;
            moveDirection = cachedTransform.up;
        }
        private void Update()
        {
            cachedTransform.Translate(moveDirection * Attributes.Speed, Space.World);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Bounce(!other.gameObject.CompareTag("bullet"));
        }

        private void Bounce(bool ignoreCooldown = true)
        {
            if (!ignoreCooldown)
            {
                if (!CanBounce) return;
            }

            float random = Random.Range(0.1f, 0.3f);
            Vector3 randomDirection = new Vector3(random, random, 0f);
            moveDirection *= -1f;
            moveDirection += randomDirection;

            bounceTimer = bounceCooldown;
        }
    }
}
