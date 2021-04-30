using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;
using TMPro;

public class ShipGenerator : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private List<GameObject> shipsGameObjects;
    [SerializeField]
    private int magentaShips;
    [SerializeField]
    private int yellowShips;

    [SerializeField]
    private IntReference magentaPrice;
    private int magentaPriceTotal;
    [SerializeField]
    private IntReference cyanPrice;
    [SerializeField]
    private IntReference yellowPrice;
    private int yellowPriceTotal;

    [SerializeField]
    private FloatReference life;
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
    public int Ships;
    public int CyanShips;

    public GameObject CircleShip;

    public Transform MagentaSpawn;
    public Transform CyanSpawn;

    public TextMeshProUGUI MagentaTimer;
    public TextMeshProUGUI CyanTimer;
    public TextMeshProUGUI YellowTimer;

    public TextMeshProUGUI MagentaPriceText;
    public TextMeshProUGUI CyanPriceText;
    public TextMeshProUGUI YellowPricText;

    public Image RegenIcon;

    public float MagentaReloadTime;
    public float CyanReloadTime;
    public float YellowReloadTime;

    public lifeBar lb;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        magentaPriceTotal = magentaPrice;
        yellowPriceTotal = yellowPrice;
    }
    void Update()
    {
        Regen();
        RegenLoading();
        ColorTimer();
        GraphitePrice();


        if (Input.GetAxis("CircleOpen") == 1 && life > 0)
        {
            CircleShip.SetActive(true);
            Time.timeScale = .5f;
        }
        else if(life > 0)
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
    public void GenerateShip(GameObject ShipPrefab)
    {
        if(ShipPrefab.name == "MagentaShip")
        {
            if (magentaShips < 10)
            {
                Ships++;
                GameObject magenta = (GameObject)Instantiate(ShipPrefab, MagentaSpawn.position, transform.rotation);

                shipsGameObjects.Add(magenta);

                magentaShips++;

                magenta.GetComponent<magentaAI>().Mothership = transform;

                MagentaSpawn.parent.eulerAngles += new Vector3(0, 0, 36);

                magenta.transform.parent = MagentaSpawn.parent.parent;

                life.Value -= magentaPrice;
                lifeRegen += magentaPrice;

                magentaPriceTotal += 4;

                magentaLoading = true;

                CircleShip.SetActive(false);
            }
        }
        else if (ShipPrefab.name == "CyanShip")
        {
            if (CyanShips < 2)
            {
                Ships++;

                GameObject cyan = (GameObject)Instantiate(ShipPrefab, transform.position, transform.rotation);

                if (CyanShips == 0)
                {
                    cyan.transform.position = CyanSpawn.GetChild(0).position;
                }
                else
                {
                    cyan.transform.position = CyanSpawn.GetChild(1).position;
                }

                GetComponent<Mothership>().CyanShips.Add(cyan);
                CyanShips++;
                cyan.transform.parent = CyanSpawn;
                shipsGameObjects.Add(cyan); 
                life.Value -= cyanPrice;
                lifeRegen += cyanPrice;
                cyanLoading = true;
                CircleShip.SetActive(false);
            }
        }
        else
        {
            Ships++;

            GameObject yellow = (GameObject)Instantiate(ShipPrefab, transform.position, transform.rotation);

            yellowShips++;
            shipsGameObjects.Add(yellow);
            life.Value -= yellowPrice;
            lifeRegen += yellowPrice;
            yellowPriceTotal += 5;
            yellowLoading = true;
            CircleShip.SetActive(false);
        }

        lb.lifeBarUpdate();
    }

    private void ColorTimer()
    {
        if (magentaLoading)
        {
            if (magentaTime == 0)
            {
                magentaTime = MagentaReloadTime;
            }
            else if (magentaTime > 0)
            {
                if(Time.timeScale == 1)
                {
                    magentaTime -= 1 * Time.deltaTime;
                }
                else
                {
                    magentaTime -= 1 * Time.deltaTime * 2;
                }
                MagentaTimer.text = magentaTime.ToString("F") + "s";
                MagentaTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                magentaTime = 0;
                magentaLoading = false;
                MagentaTimer.transform.parent.GetComponent<Button>().interactable = true;
                MagentaTimer.text = "";
            }
        }

        if (cyanLoading)
        {
            if(cyanTime == 0)
            {
                cyanTime = CyanReloadTime;
            }
            else if (cyanTime > 0)
            {
                if (Time.timeScale == 1)
                {
                    cyanTime -= 1 * Time.deltaTime;
                }
                else
                {
                    cyanTime -= 1 * Time.deltaTime * 2;
                }
                CyanTimer.text = cyanTime.ToString("F") + "s";
                CyanTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                cyanTime = 0;
                cyanLoading = false;
                CyanTimer.transform.parent.GetComponent<Button>().interactable = true;
                CyanTimer.text = "";
            }
        }

        if (yellowLoading)
        {
            if(yellowTime == 0)
            {
                yellowTime = YellowReloadTime;
            }
            else if (yellowTime > 0)
            {
                if (Time.timeScale == 1)
                {
                    yellowTime -= 1 * Time.deltaTime;
                }
                else
                {
                    yellowTime -= 1 * Time.deltaTime * 2;
                }
                YellowTimer.text = yellowTime.ToString("F") + "s";
                YellowTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                yellowTime = 0;
                yellowLoading = false;
                YellowTimer.transform.parent.GetComponent<Button>().interactable = true;
                YellowTimer.text = "";
            }
        }
    }

    private void GraphitePrice()
    {
        MagentaPriceText.text = magentaPriceTotal + "///";
        CyanPriceText.text = cyanPrice + "///";
        YellowPricText.text = yellowPriceTotal + "///";
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
            for (int i = 0; i < shipsGameObjects.Count; i++)
            {
                Destroy(shipsGameObjects[i]);
            }
            life.Value += lifeRegen;

            lifeRegen = 0;
            CyanShips = 0;
            magentaShips = 0;
            Ships = 0;
            magentaPriceTotal = magentaPrice;
            yellowPriceTotal = yellowPrice;

            lb.lifeBarUpdate();
            shipsGameObjects.Clear();
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
                RegenIcon.fillAmount = regenTimer / 30;
            }
            else
            {
                regenTimer = 30;
                regenLoad = true;
            }
        }
    }
    #endregion
}
