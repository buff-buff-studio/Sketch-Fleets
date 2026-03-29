using System.Linq;
using SketchFleets.Enemies;
using SketchFleets.Entities;
using SketchFleets.Systems.DeathContext;
using SketchFleets.UI;
using UnityEngine;

namespace SketchFleets.Enemies
{
    public class SamaelShip : EnemyShip
    {
        [Space]
        [Header("Samael Settings")]
        [SerializeField] private uint timesBeenInvincible = 0;
        [SerializeField] private Sprite normalSprite, invincibleSprite;
        private bool isInvincible = false;

        private float lifePercentageToInvincible
        {
            get
            {
                return timesBeenInvincible switch
                {
                    0 => .75f,
                    1 => .50f,
                    2 => .25f,
                    _ => -100
                };
            }
        }

        protected override void Start()
        {
            var bossHealthBar = FindObjectsByType<BossBar>(FindObjectsSortMode.None).FirstOrDefault();
            Debug.Log(bossHealthBar.gameObject.name);
            if (bossHealthBar != null)
                bossHealthBar.SetBoss(this);

            base.Start();
            //Find boss health bar in scene and set himseft as boss
        }

        protected override void Update()
        {
            base.Update();
            if (isInvincible && collisionTimer <= 0)
            {
                isInvincible = false;
                spriteRenderer.sprite = normalSprite;
            }
        }

        #region Ship Overrides

        public override void Damage(float amount, DamageContext context, bool makeInvincible = false, bool piercing = false)
        {
            base.Damage(amount, context, makeInvincible, piercing);

            var lifePercentage = currentHealth.Value / attributes.MaxHealth;
            if (!(lifePercentage <= lifePercentageToInvincible)) return;
            MakeInvulnerable(attributes.InvincibilityTime);
            timesBeenInvincible++;
            isInvincible = true;
            spriteRenderer.sprite = invincibleSprite;
        }

        #endregion
    }
}
