using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets.ProfileSystem;
using TMPro;

public class shellScript : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private IntReference pencilShell;
    [SerializeField]
    private ColorReference shellColor;
    [SerializeField]
    private TextMeshProUGUI shellNum;
    #endregion

    #region Unity Callbacks
    void Update()
    {
        GetComponent<Image>().color = shellColor;
        int value = pencilShell.Value;
        shellNum.text = value.ToString();    
    }
    #endregion
}
