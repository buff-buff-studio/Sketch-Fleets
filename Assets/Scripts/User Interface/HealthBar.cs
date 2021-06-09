using ManyTools.UnityExtended.Editor;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets.Entities;

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
            if (target.CurrentHealth <= 0 && gameOverScreen.activeSelf == false)
            {
                //Add coins and clear
                gameOverScreen.SetActive(true);
                Time.timeScale = 0;

                //Add coins
                ProfileSystem.Profile.Data.TotalCoins += ProfileSystem.ProfileData.ConvertCoinsToTotalCoins(pencilShell.Value);
                PencilBoxText.AddedAmount = pencilShell.Value;
                pencilShell.Value = 0;           

                SketchFleets.ProfileSystem.Profile.Data.Clear(this,(data) => {
                    //Clear data
                });
            }
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

        #endregion
    }
}
