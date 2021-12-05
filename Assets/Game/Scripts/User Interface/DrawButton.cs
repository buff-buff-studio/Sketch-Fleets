using ManyTools.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    /// <summary>
    /// A class that manages the game's draw button
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class DrawButton : MonoBehaviour
    {
        #region Private Fields

        [Header("Color Reference")]
        [SerializeField]
        private Button button;
        
        private Image _image;
        
        private static readonly int RedMul = Shader.PropertyToID("_RedMult");
        private static readonly int BluMul = Shader.PropertyToID("_BluMult");
        private static readonly int GreMul = Shader.PropertyToID("_GreMult");

        #endregion

        #region Unity Callbacks

        private void Awake()
        {
            TryGetComponent(out button);
            TryGetComponent(out _image);
        }

        #endregion

        #region Public Methods

        public void UpdateButton(Color playerColor)
        {
            SetInteractable(!Mathf.Approximately(playerColor.a, 0f));
            SetColor(playerColor);
        }
        
        /// <summary>
        /// Sets the button's interactability state
        /// </summary>
        /// <param name="interactable">Whether the button is interactable</param>
        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        /// <summary>
        /// Sets the button's primary color
        /// </summary>
        /// <param name="drawColor">The primary color</param>
        public void SetColor(Color drawColor)
        {
            drawColor = new Color(drawColor.r * 0.9f, drawColor.g * 0.9f, drawColor.b * 0.9f);
            _image.material.SetColor(GreMul, drawColor);

            Color[] complementaryTriad = GetTriadicComplementaryOf(drawColor);
            
            _image.material.SetColor(RedMul, complementaryTriad[0]);
            _image.material.SetColor(BluMul, complementaryTriad[1]);
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// Gets the triad complementary colors of the given color
        /// </summary>
        /// <param name="color">The color to get complementary colors of</param>
        /// <returns>The triadic complementary colors of the given color</returns>
        private static Color[] GetTriadicComplementaryOf(Color color)
        {
            Color[] triad = new Color[2];

            Color.RGBToHSV(color, out float hue, out float saturation, out float value);
            
            triad[0] = Color.HSVToRGB(hue + 0.666f, saturation * 0.5f, value * 0.8f);
            triad[1] = Color.HSVToRGB(hue + 0.666f, saturation * 0.5f, value * 0.5f);
            
            return triad;
        }

        #endregion
    }
}
