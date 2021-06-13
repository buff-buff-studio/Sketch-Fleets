using ManyTools.UnityExtended.Editor;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets.Entities;
using Unity.Mathematics;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that controls a shield bar display
    /// </summary>
    public class ShieldBar : MonoBehaviour
    {
        // NOTE: This class doesn't really need to exist. It should just
        #region Private Fields

        [Header("Target")]
        [SerializeField, RequiredField()]
        private Mothership target;

        [Header("UI Settings")]
        [SerializeField]
        private FloatReference lerpSpeed;
        [SerializeField, RequiredField()]
        private Image shieldBar;

        #endregion

        #region Properties

        private float FillAmount => target.CurrentShield / target.GetMaxShield();

        #endregion

        #region Unity Callbacks

        private void Update()
        {
            if (!Mathf.Approximately(shieldBar.fillAmount, FillAmount))
            {
                ShieldBarUpdate();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the graphic part of the life bar 
        /// </summary>
        private void ShieldBarUpdate()
        {
            shieldBar.fillAmount = math.lerp(shieldBar.fillAmount, FillAmount, Time.deltaTime * lerpSpeed);
        }

        #endregion
    }
}
