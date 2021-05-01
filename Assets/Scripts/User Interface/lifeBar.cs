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
    private GameObject deadMenu;
    private Image lifeBarTotal;
    private Image lifeBarAtt;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        lifeBarAtt = transform.GetChild(2).GetComponent<Image>();
        lifeBarUpdate();
    }

    private void Update()
    {
        if (life > 0)
        {
            lifeBarUpdate();
        }
        else
        {
            deadMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
    #endregion
    #region LifeBar
    /// <summary>
    /// Changes the graphic part of the life bar 
    /// </summary>
    public void lifeBarUpdate()
    {
        lifeBarAtt.fillAmount = life / 1000;
    }
    #endregion
}
