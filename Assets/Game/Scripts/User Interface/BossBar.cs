using ManyTools.UnityExtended.Editor;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets.Entities;
using SketchFleets.General;
using System.Collections;
using System.Collections.Generic;
using SketchFleets.Enemies;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that controls a health bar display
    /// </summary>
    public sealed class BossBar : MonoBehaviour
    {
        #region Private Fields

        [Header("Target")]
        [SerializeField, RequiredField()]
        private EnemyShip target;

        [Header("UI Settings")]
        [SerializeField]
        private FloatReference lerpSpeed;

        [SerializeField, RequiredField()]
        private Image healthBar;

        #endregion

        #region Properties

        private float FillAmount => target.CurrentHealth / target.MaxHealth;

        #endregion

        #region Unity Callbacks

        private void Update()
        {
            if (!Mathf.Approximately(healthBar.fillAmount, FillAmount))
            {
                LifeBarUpdate();
            }

            if (LevelManager.Instance.GameEnded) return;
            LevelManager.Instance.GameEnded = true;
            // Forces the healthbar to be at 0
            healthBar.fillAmount = 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the graphic part of the life bar 
        /// </summary>
        private void LifeBarUpdate()
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, FillAmount, Time.deltaTime * lerpSpeed);
        }

        public void SetBoss(EnemyShip boss)
        {
            gameObject.SetActive(true);
            target = boss;
        }

        #endregion
    }
}