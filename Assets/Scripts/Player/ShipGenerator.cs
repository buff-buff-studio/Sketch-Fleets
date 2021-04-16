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
    private GameObject[] shipsGameObjects;
    [SerializeField]
    private int magentaShips;
    [SerializeField]
    private int yellowShips;

    [SerializeField]
    private IntReference magentaNum;
    [SerializeField]
    private IntReference cyanNum;
    [SerializeField]
    private IntReference yellowNum;

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

    public Image RegenIcon;

    public float MagentaReloadTime;
    public float CyanReloadTime;
    public float YellowReloadTime;
    #endregion

    #region Unity Callbacks
    void Update()
    {
        Regen();
        RegenLoading();
        ColorTimer();
        
        if (Input.GetKey(KeyCode.Space))
        {
            CircleShip.SetActive(true);
        }
        else
        {
            CircleShip.SetActive(false);
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
            Ships += magentaNum;

            for (int i = 0; i < magentaNum; i++)
            {
                GameObject magenta = (GameObject)Instantiate(ShipPrefab, transform.position, transform.rotation);

                magentaShips++;
                magenta.transform.position = MagentaSpawn.GetChild(i).position;

                magenta.transform.parent = MagentaSpawn;
                shipsGameObjects[Ships - (magentaNum - i)] = magenta;
            }
            magentaLoading = true;
            CircleShip.SetActive(false);
        }
        else if (ShipPrefab.name == "CyanShip")
        {
            if (CyanShips < 2)
            {
                Ships += cyanNum;

                GameObject cyan = (GameObject)Instantiate(ShipPrefab, transform.position, transform.rotation);

                if (CyanShips == 0)
                {
                    cyan.transform.position = CyanSpawn.GetChild(0).position;
                }
                else
                {
                    cyan.transform.position = CyanSpawn.GetChild(1).position;
                }

                CyanShips++;
                cyan.transform.parent = CyanSpawn;
                shipsGameObjects[Ships - 1] = cyan;
                cyanLoading = true;
                CircleShip.SetActive(false);
            }
        }
        else
        {
            Ships += yellowNum;

            GameObject yellow = (GameObject)Instantiate(ShipPrefab, transform.position, transform.rotation);

            yellowShips++;
            shipsGameObjects[Ships - 1] = yellow;
            yellowLoading = true;
            CircleShip.SetActive(false);
        }
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
                magentaTime -= 1 * Time.deltaTime;
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
                cyanTime -= 1 * Time.deltaTime;
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
                yellowTime -= 1 * Time.deltaTime;
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
    #endregion

    #region Ship Regenerate
    /// <summary>
    /// WIP
    /// </summary>
    public void Regen()
    {
        if (Input.GetKeyUp(KeyCode.R) && regenLoad)
        {
            for (int i = 0; i < shipsGameObjects.Length; i++)
            {
                Destroy(shipsGameObjects[i]);
            }
            regenLoad = false;
            Ships = 0;
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
            }
        }
    }
    #endregion
}
