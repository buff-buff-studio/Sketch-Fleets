using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using TMPro;
using SketchFleets.Data;
using SketchFleets.Entities;
using ManyTools.UnityExtended.Poolable;

public class ShipGenerator : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private MothershipAttributes mothershipAttributes;

    [SerializeField]
    private Mothership mothership;

    [SerializeField]
    private SpawnableShipAttributes magentaShip;
    [SerializeField]
    private SpawnableShipAttributes cyanShip;
    [SerializeField]
    private SpawnableShipAttributes yellowShip;

    private int magentaShips;
    private int cyanShips;

    private float lifeRegen;

    private bool magentaLoading = true;
    private bool cyanLoading = true;
    private bool yellowLoading = true;

    private bool regenLoad = true;

    private float magentaTime;
    private float cyanTime;
    private float yellowTime;

    private float regenTimer = 30;
    #endregion

    #region Public Fields

    public GameObject CircleShip;

    public GameObject MagentaBtt;
    public GameObject CyanBtt;
    public GameObject YellowBtt;

    public TextMeshProUGUI MagentaPriceText;
    public TextMeshProUGUI CyanPriceText;
    public TextMeshProUGUI YellowPriceText;

    public Image RegenIcon;

    #endregion

    #region Unity Callbacks
    void Update()
    {
        Regen();
        RegenLoading();
        ColorTimer();
        GraphitePrice();


        if (Input.GetKeyDown(KeyCode.Space) && mothership.MaxHealth > 0)
        {
            CircleShip.SetActive(true);
            Time.timeScale = .5f;
        }
        if(Input.GetKeyUp(KeyCode.Space) && mothership.CurrentHealth > 0)
        {
            CircleShip.SetActive(false);
            Time.timeScale = 1;
        }
    }
    #endregion

    #region Ships Generation
    /// <summary>
    /// Summon magenta ship
    /// Summon cyan ship
    /// Summon yellow ship
    /// </summary>
    public void GenerateShip(int shipNum)
    {
        if(shipNum == 0)
        {
            if (magentaShips < magentaShip.MaximumShips)
            {
                magentaShips++;

                mothership.CurrentHealth.Value -= magentaShip.GraphiteCost;
                lifeRegen += magentaShip.GraphiteCost;

                magentaLoading = true;

                CircleShip.SetActive(false);
            }
        }
        else if (shipNum == 1)
        {
            if (cyanShips < cyanShip.MaximumShips)
            {
                cyanShips++;

                mothership.CurrentHealth.Value -= cyanShip.GraphiteCost;

                lifeRegen += cyanShip.GraphiteCost;

                cyanLoading = true;

                CircleShip.SetActive(false);
            }
        }
        else
        {
            mothership.CurrentHealth.Value -= yellowShip.GraphiteCost;

            lifeRegen += yellowShip.GraphiteCost;

            yellowLoading = true;

            CircleShip.SetActive(false);
        }

    }

    private void ColorTimer()
    {
        if (magentaLoading)
        {
            if (magentaTime == magentaShip.SpawnCooldown)
            {
                magentaTime = 0;
                MagentaBtt.GetComponent<Button>().interactable = false;
                MagentaPriceText.gameObject.SetActive(false);
            }
            else if (magentaTime < magentaShip.SpawnCooldown)
            {
                if(Time.timeScale == 1)
                {
                    magentaTime += 1 * Time.deltaTime;
                }
                else
                {
                    magentaTime += 1 * Time.deltaTime * 2;
                }

                MagentaBtt.GetComponent<Image>().fillAmount = magentaTime / magentaShip.SpawnCooldown;
            }
            else
            {
                magentaTime = magentaShip.SpawnCooldown;
                magentaLoading = false;
                MagentaBtt.GetComponent<Button>().interactable = true;
                MagentaBtt.GetComponent<Image>().fillAmount = 1;
                MagentaPriceText.gameObject.SetActive(true);
            }
        }

        if (cyanLoading)
        {
            if(cyanTime == cyanShip.SpawnCooldown)
            {
                cyanTime = 0;
                CyanBtt.GetComponent<Button>().interactable = false;
                CyanPriceText.gameObject.SetActive(false);
            }
            else if (cyanTime < cyanShip.SpawnCooldown)
            {
                if (Time.timeScale == 1)
                {
                    cyanTime += 1 * Time.deltaTime;
                }
                else
                {
                    cyanTime += 1 * Time.deltaTime * 2;
                }
                CyanBtt.GetComponent<Image>().fillAmount = cyanTime / cyanShip.SpawnCooldown;
            }
            else
            {
                cyanTime = cyanShip.SpawnCooldown;
                cyanLoading = false;

                CyanBtt.GetComponent<Button>().interactable = true;
                CyanBtt.GetComponent<Image>().fillAmount = 1;

                CyanPriceText.gameObject.SetActive(true);
            }
        }

        if (yellowLoading)
        {
            if(yellowTime == yellowShip.SpawnCooldown)
            {
                yellowTime = 0;
                YellowBtt.GetComponent<Button>().interactable = false;
                YellowPriceText.gameObject.SetActive(false);
            }
            else if (yellowTime < yellowShip.SpawnCooldown)
            {
                if (Time.timeScale == 1)
                {
                    yellowTime += 1 * Time.deltaTime;
                }
                else
                {
                    yellowTime += 1 * Time.deltaTime * 2;
                }
                YellowBtt.GetComponent<Image>().fillAmount = yellowTime / yellowShip.SpawnCooldown;
            }
            else
            {
                yellowTime = yellowShip.SpawnCooldown;
                yellowLoading = false;
                YellowBtt.GetComponent<Button>().interactable = true;
                YellowBtt.GetComponent<Image>().fillAmount = 1;
                YellowPriceText.gameObject.SetActive(true);
            }
        }
    }

    private void GraphitePrice()
    {
        MagentaPriceText.text = magentaShip.GraphiteCost.ToString();
        CyanPriceText.text = cyanShip.GraphiteCost.ToString();
        YellowPriceText.text = yellowShip.GraphiteCost.ToString();
    }
    #endregion

    #region Ship Regenerate
    /// <summary>
    /// WIP
    /// </summary>
    public void Regen()
    {
        if (Input.GetAxis("Regen") == 1 && regenLoad)
        {
            mothership.CurrentHealth.Value += lifeRegen;

            lifeRegen = 0;
            cyanShips = 0;
            magentaShips = 0;

            PoolManager.Instance.DestroyPool(magentaShip.Prefab);
            PoolManager.Instance.DestroyPool(cyanShip.Prefab);
            PoolManager.Instance.DestroyPool(yellowShip.Prefab);

            magentaShip.GraphiteCost.Value = magentaShip.GraphiteCost;
            yellowShip.GraphiteCost.Value = yellowShip.GraphiteCost;
            regenLoad = false;
        }
    }

    private void RegenLoading()
    {
        if (!regenLoad)
        {
            if (regenTimer == 30)
            {
                regenTimer = 0;
            }
            else if (regenTimer < 30)
            {
                regenTimer += 1 * Time.deltaTime;
                RegenIcon.gameObject.SetActive(false);
            }
            else
            {
                RegenIcon.gameObject.SetActive(true);
                regenTimer = 30;
                regenLoad = true;
            }
        }
    }
    #endregion
}