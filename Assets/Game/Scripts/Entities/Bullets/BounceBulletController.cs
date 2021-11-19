using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SketchFleets
{
    public class BounceBulletController : BulletController
    {
        [SerializeField]
        private float maxBounceLife = 4;

        private float bounceLife = 4;
        private Vector3 maxScale = new Vector3(.5f, .5f, .5f);

        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            bounceLife = maxBounceLife;

            if (transform.localScale.x < maxScale.x)
            {
                transform.localScale = maxScale;
            }
            
            base.Emerge(position, rotation);
        }

        protected override void Update()
        {
            if (PlayerBuletVelocity == 0)
            {
                Move(Vector3.up * Attributes.Speed, Space.Self);
            }
            else
            {
                Move(Vector3.up * PlayerBuletVelocity, Space.World);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Bounce(other.gameObject.CompareTag("bullet"));
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            Bounce(col.gameObject.CompareTag("bullet"));
            DealDamageToTarget(false, col.gameObject);
        }

        protected override void DealDamageToTarget(bool directDamage, GameObject target)
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
        }

        private void Bounce(bool ignoreCooldown)
        {
            if (!gameObject.activeSelf || ignoreCooldown) return;

            transform.Rotate(new Vector3(0, 0, Random.Range(80, 101)));
            bounceLife--;
            if (bounceLife < maxBounceLife - 1) transform.localScale *= .85f;
            if (bounceLife <= 0 && gameObject.activeSelf) Submerge();
        }
    }
}