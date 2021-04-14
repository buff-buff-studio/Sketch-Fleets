using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;

public class lifeBar : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference _life;
    [SerializeField]
    private ColorReference _color75;
    [SerializeField]
    private ColorReference _color50;
    [SerializeField]
    private ColorReference _color25;
    private Image _lifeBarTotal;
    private Image _lifeBarAtt;
    private ColorShips _colorShip;
    #endregion

    #region Unity Callback
    private void Start()
    {
        _lifeBarTotal = transform.GetChild(1).GetComponent<Image>();
        _lifeBarAtt = transform.GetChild(3).GetComponent<Image>();
        if (transform.parent.gameObject.name != "MotherShip")
        {
            _colorShip = transform.parent.GetComponent<ColorShips>();
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
        if (transform.parent.gameObject.name == "MotherShip")
        {
            _lifeBarAtt.fillAmount = _life / 100;
            if (_life >= 75)
            {
                _lifeBarTotal.color = _color75;
                _lifeBarAtt.color = _color75;
            }
            else if (_life >= 25)
            {
                _lifeBarTotal.color = _color50;
                _lifeBarAtt.color = _color50;
            }
            else
            {
                _lifeBarTotal.color = _color25;
                _lifeBarAtt.color = _color25;
            }
        }
        else
        {
            _lifeBarAtt.fillAmount = _colorShip._life / _colorShip._lifeMax;
            if (_colorShip._life >= _colorShip._lifeMax*.75f)
            {
                _lifeBarTotal.color = _color75;
                _lifeBarAtt.color = _color75;
            }
            else if (_colorShip._life >= _colorShip._lifeMax*.25f)
            {
                _lifeBarTotal.color = _color50;
                _lifeBarAtt.color = _color50;
            }
            else
            {
                _lifeBarTotal.color = _color25;
                _lifeBarAtt.color = _color25;
            }
        }
    }
    #endregion
}
