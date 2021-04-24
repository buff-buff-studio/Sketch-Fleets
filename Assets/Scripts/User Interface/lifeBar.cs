using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;

public class lifeBar : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference life;
    [SerializeField]
    private ColorReference color75;
    [SerializeField]
    private ColorReference color50;
    [SerializeField]
    private ColorReference color25;
    private Image lifeBarTotal;
    private Image lifeBarAtt;
    private ColorShips colorShip;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        if (transform.parent.gameObject.name != "Canvas")
        {
            colorShip = transform.parent.GetComponent<ColorShips>();
            lifeBarTotal = transform.GetChild(1).GetComponent<Image>();
            lifeBarAtt = transform.GetChild(3).GetComponent<Image>();
        }
        else
        {
            lifeBarAtt = transform.GetChild(2).GetComponent<Image>();
        }
        lifeBarUpdate();
    }
    #endregion
    #region LifeBar
    /// <summary>
    /// Changes the graphic part of the life bar 
    /// </summary>
    public void lifeBarUpdate()
    {
        if (transform.parent.gameObject.name == "Canvas")
        {
            lifeBarAtt.fillAmount = life / 100;
        }
        else
        {
            lifeBarAtt.fillAmount = colorShip.Life / colorShip.LifeMax;
            if (colorShip.Life >= colorShip.LifeMax*.75f)
            {
                lifeBarTotal.color = color75;
                lifeBarAtt.color = color75;
            }
            else if (colorShip.Life >= colorShip.LifeMax*.25f)
            {
                lifeBarTotal.color = color50;
                lifeBarAtt.color = color50;
            }
            else
            {
                lifeBarTotal.color = color25;
                lifeBarAtt.color = color25;
            }
        }
    }
    #endregion
}
