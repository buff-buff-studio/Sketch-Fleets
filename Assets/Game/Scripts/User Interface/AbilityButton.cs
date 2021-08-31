using ManyTools.UnityExtended.Editor;
using ManyTools.Variables;
using SketchFleets.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that handles the UI ability button
    /// </summary>
    public class AbilityButton : MonoBehaviour
    {
        #region Private Fields

        [Header("References")]
        [Tooltip("The mothership reference so that the cooldown can be accessed")]
        [SerializeField, RequiredField()]
        private Mothership mothership;
        [Tooltip("The ability icon's image")]
        [SerializeField, RequiredField()]
        private Image abilityIcon;
        
        [Tooltip("The color to which the icon should be set if the ability is not available")]
        [SerializeField]
        private ColorReference disabledColor = new ColorReference(Color.black);

        private Color bufferColor;
        
        #endregion

        #region Unity Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            bufferColor = abilityIcon.color;
        }

        // Update is called once per frame
        private void Update()
        {
            SetIconFill();
            SetIconColor();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the ability icon's fill
        /// </summary>
        private void SetIconFill()
        {
            abilityIcon.fillAmount = GetIconFill();
        }

        /// <summary>
        /// Gets the proper fill for the icon
        /// </summary>
        /// <returns>The fill amount for the icon</returns>
        private float GetIconFill()
        {
            float adjustedCooldown = math.max(mothership.AbilityTimer, 0f);
            float fillAmount = mothership.GetMaxAbilityCooldown() - adjustedCooldown;
            fillAmount = math.remap(0, mothership.GetMaxAbilityCooldown(), 1f, 
            0f, fillAmount);
                        
            return 1f - fillAmount;
        }

        /// <summary>
        /// Sets the ability icon's color
        /// </summary>
        private void SetIconColor()
        {
            abilityIcon.color = mothership.IsAbilityAvailable() ? bufferColor : disabledColor;
        }
        
        #endregion
    }
}
