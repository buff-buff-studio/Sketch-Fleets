using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
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
        shellNum.text = pencilShell.ToString();
    }
    #endregion
}
