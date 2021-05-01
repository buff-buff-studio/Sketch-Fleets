using UnityEngine;
using UnityEngine.Audio;

namespace ManyTools.UnityExtended
{
    public class VolumeSlider : MonoBehaviour
    {
        #region Public Fields

        [SerializeField]
        private string _groupName;
        [SerializeField]
        private AudioMixer _mixer;

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the volume of an audio group on a logarithmic scale.
        /// </summary>
        /// <param name="sliderValue">The value of the slider</param>
        public void SetVolume(float sliderValue)
        {
            _mixer.SetFloat(_groupName, Mathf.Log10(sliderValue) * 20);
        }

        #endregion
    }
}
