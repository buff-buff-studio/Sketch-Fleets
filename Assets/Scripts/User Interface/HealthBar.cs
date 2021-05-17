using System;
using ManyTools.UnityExtended.Editor;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using SketchFleets;
using SketchFleets.Data;
using SketchFleets.Entities;

public class HealthBar : MonoBehaviour
{
    #region Private Fields

    [Header("Target")]
    [SerializeField, RequiredField()]
    private Mothership target;
    
    [Header("UI Settings")]
    [SerializeField]
    private FloatReference lerpSpeed;
    [SerializeField, RequiredField()]
    private Image healthBar;

    private IHealthVerifiable healthVerifiable;
    
    #endregion

    #region Properties

    private float FillAmount => target.CurrentHealth / target.MaxHealth;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        healthVerifiable = target;
    }

    private void Update()
    {
        if (!Mathf.Approximately(healthBar.fillAmount, FillAmount))
        {
            LifeBarUpdate();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Changes the graphic part of the life bar 
    /// </summary>
    private void LifeBarUpdate()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, FillAmount, Time.deltaTime * lerpSpeed);
    }

    #endregion
}