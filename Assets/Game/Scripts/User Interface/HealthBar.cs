using ManyTools.UnityExtended.Editor;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets.Entities;
using SketchFleets.General;
using System.Collections;
using System.Collections.Generic;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that controls a health bar display
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        #region Private Fields

        [Header("Target")]
        [SerializeField, RequiredField()]
        private Mothership target;

        [SerializeField]
        private GameObject gameOverScreen;

        [Header("UI Settings")]
        [SerializeField]
        private FloatReference lerpSpeed;

        [SerializeField, RequiredField()]
        private Image healthBar;

        [SerializeField]
        private IntReference pencilShell;

        private IHealthVerifiable healthVerifiable;

        #endregion

        #region Properties

        private float FillAmount => target.CurrentHealth / target.MaxHealth;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            healthVerifiable = target;
        }

        private void Update()
        {
            if (!Mathf.Approximately(healthBar.fillAmount, FillAmount))
            {
                LifeBarUpdate();
            }

            // NOTE: Why the fuck is this here? The health bar of all things is responsible for throwing game over?
            if (!IsGameOver()) return;

            if (!LevelManager.Instance.GameEnded)
            {
                LevelManager.Instance.GameEnded = true;
                ShowGameOver();
                RewardPlayer();

                // Forces the healthbar to be at 0
                healthBar.fillAmount = 0;

                // Clears map-specific data
                SketchFleets.ProfileSystem.Profile.Data.Clear(this, (data) => { });
            }
        }

        #endregion

        #region Public Methods

        private bool IsGameOver()
        {
            //return true;
            return target.CurrentHealth <= 0 && gameOverScreen.activeSelf == false;
        }

        /// <summary>
        /// Changes the graphic part of the life bar 
        /// </summary>
        private void LifeBarUpdate()
        {
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, FillAmount, Time.deltaTime * lerpSpeed);
        }

        /// <summary>
        /// Shows the game over screen
        /// </summary>
        private void ShowGameOver()
        {
            gameOverScreen.SetActive(true);
            StartCoroutine(LerpGameOverScreen());
            Time.timeScale = 0;
        }

        IEnumerator LerpGameOverScreen()
        {
            float timer = Time.unscaledTime;
            float actual = 0;
            CanvasGroup canvasGroup = gameOverScreen.GetComponent<CanvasGroup>();

            while ((actual = Time.unscaledTime - timer) < 1.5f)
            {
                canvasGroup.alpha = actual / 1.5f;
                yield return 0;
            }
        }

        /// <summary>
        /// Rewards the player
        /// </summary>
        private void RewardPlayer()
        {
            int n = pencilShell.Value - LevelManager.Instance.pencilShellAtStart;
            ProfileSystem.Profile.Data.TotalCoins += ProfileSystem.ProfileData.ConvertCoinsToTotalCoins(n);
            PencilBoxText.AddedAmount = n;
            pencilShell.Value = 0;
        }

        #endregion
    }
}