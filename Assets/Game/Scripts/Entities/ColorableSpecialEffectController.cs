using UnityEngine;

namespace SketchFleets.Entities
{
    /// <summary>
    /// A class that controls special effects
    /// </summary>
    [RequireComponent(typeof(AudioSource), typeof(ParticleSystem))]
    public sealed class ColorableSpecialEffectController : SpecialEffectController
    {
        #region Private Fields

        private Color _effectColor = Color.white;

        #endregion

        #region Properties

        public Color EffectColor
        {
            get => _effectColor;
            set
            {
                _effectColor = value;
                UpdateEffectColor();
            }
        }

        #endregion

        #region Overrides

        public override void Emerge(Vector3 position, Quaternion rotation)
        {
            UpdateEffectColor();
            base.Emerge(position, rotation);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the effect's main color
        /// </summary>
        private void UpdateEffectColor()
        {
            ParticleSystem.MainModule mainModule = visualParticleSystem.main;
            mainModule.startColor = EffectColor;
        }

        #endregion
    }
}