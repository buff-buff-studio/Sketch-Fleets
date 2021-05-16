using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;

public class LifeBar : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference life;
    [SerializeField]
    private FloatReference maxLife;
    [SerializeField]
    private GameObject deadMenu;
    private Image lifeBarAtt;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        lifeBarAtt = transform.GetChild(1).GetComponent<Image>();
        LifeBarUpdate();
    }

    private void Update()
    {
        if (life > 0)
        {
            LifeBarUpdate();
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
    public void LifeBarUpdate()
    {
        float maxLife = this.maxLife.Value;
        lifeBarAtt.fillAmount = Mathf.Lerp(lifeBarAtt.fillAmount,life / maxLife,Time.deltaTime * 5f);
    }
    #endregion
}
