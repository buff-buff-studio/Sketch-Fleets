using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeScript : MonoBehaviour
{
    #region Public Fields

    [SerializeField]
    private string exposedParam;
    [SerializeField]
    private AudioMixer mixer;
    
    // Choose which mixer will have this method applied to it

    #endregion

    #region Public Methods

    /// <summary>
    /// This makes the slider value change the volume on a logarithmic scale instead of linear.
    /// </summary>
    /// <param name="sliderValue">Turns the min and max value of the slider into a number between 0 and 80 decibels allowing for a more accurate sound setting.</param>
    public void SetVolume(float sliderValue)
    {
        mixer.SetFloat(exposedParam, Mathf.Log10(sliderValue) * 20);
    }

    #endregion
}
