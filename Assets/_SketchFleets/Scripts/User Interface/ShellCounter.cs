using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using TMPro;
using UnityEngine.Serialization;

public class ShellCounter : MonoBehaviour
{
    #region Private Fields

    [Header("Counter Parameters")]
    [SerializeField, Tooltip("The counter's image component")]
    private Image shellImage;
    [SerializeField, Tooltip("The pencil shell variable held by the player")]
    private IntReference pencilShell;
    [SerializeField, Tooltip("The color of the last collected shell")]
    private ColorReference collectedShellColor;
    [SerializeField, Tooltip("The shell counter text")]
    private TextMeshProUGUI displayText;

    private int displayedValue;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        TryGetComponent(out shellImage);
    }

    private void Update()
    {
        if (displayedValue == pencilShell.Value) return;
        
        UpdateDisplayedValue();
        UpdateDisplayedColor();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the displayed color of the shell counter
    /// </summary>
    private void UpdateDisplayedColor()
    {
        shellImage.color = collectedShellColor;
    }

    /// <summary>
    /// Updates the displayed value
    /// </summary>
    private void UpdateDisplayedValue()
    {
        displayText.text = pencilShell.Value.ToString();
        displayedValue = pencilShell.Value;
    }

    #endregion
}