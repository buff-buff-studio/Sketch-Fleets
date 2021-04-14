using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManyTools.Variables;

public class GenerateShips : MonoBehaviour
{
    #region Private Fields
    private GameObject[] _shipsGameObjects;
    [SerializeField]
    private int _magentaShips;
    [SerializeField]
    private int _yellowShips;

    private bool _magentaLoading = true, _cyanLoading = true, _yellowLoading = true;
    private float _magentaTime, _cyanTime, _yellowTime;
    #endregion
    #region Public Fields
    public int _ships;
    public int _cyanShips;

    public GameObject _circleShip;

    public Transform _motherShip;

    public GameObject _magentaPrefab;
    public GameObject _cyanPrefab;
    public GameObject _yellowPrefab;

    public Transform _magentaSpawn;
    public Transform _cyanSpawn;

    public Text _magentaTimer, _cyanTimer, _yellowTimer;
    public float _magentaReloadTime, _cyanReloadTime, _yellowReloadTime;
    #endregion

    #region Unity Callbacks
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _circleShip.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _circleShip.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Regen();
        }
        ColorTimer();
    }
    #endregion
    #region Ships Generation
    /// <summary>
    /// Summon the Magenta Ships
    /// </summary>
    public void GenerateMagenta(int num)
    {
        _ships += num;
        _shipsGameObjects = new GameObject[_ships];

        for (int i = 0; i < num; i++) 
        {
            GameObject magenta = (GameObject)Instantiate(_magentaPrefab, _motherShip.position, _motherShip.rotation);

            _magentaShips++;
            magenta.transform.position = _magentaSpawn.GetChild(_magentaShips - 1).position;

            magenta.transform.parent = _magentaSpawn;
            _shipsGameObjects[_ships - 1] = magenta;
        }
        _magentaLoading = true;
        _circleShip.SetActive(false);
    }
    /// <summary>
    /// Summon the Cyan Ships
    /// </summary>
    public void GenerateCyan(int num)
    {
        if (_cyanShips < 2)
        {
            _ships += num;
            _shipsGameObjects = new GameObject[_ships];

            GameObject cyan = (GameObject)Instantiate(_cyanPrefab, _motherShip.position, _motherShip.rotation);

            if (_cyanShips == 0)
            {
                cyan.transform.position = _cyanSpawn.GetChild(0).position;
            }
            else
            {
                cyan.transform.position = _cyanSpawn.GetChild(1).position;
            }

            _cyanShips++;
            cyan.transform.parent = _cyanSpawn;
            _shipsGameObjects[_ships - 1] = cyan;
            _cyanLoading = true;
            _circleShip.SetActive(false);
        }
    }
    /// <summary>
    /// Summon the Yellow Ships
    /// </summary>
    public void GenerateYellow(int num)
    {
        _ships += num;
        _shipsGameObjects = new GameObject[_ships];

        GameObject yellow = (GameObject)Instantiate(_yellowPrefab, _motherShip.position, _motherShip.rotation);

        _yellowShips++;
        _shipsGameObjects[_ships - 1] = yellow;
        _yellowLoading = true;
        _circleShip.SetActive(false);
    }
    /// <summary>
    /// Timer to summon again the ship
    /// </summary>
    private void ColorTimer()
    {
        if (_magentaLoading)
        {
            if (_magentaTime == 0)
            {
                _magentaTime = _magentaReloadTime;
            }
            else if (_magentaTime > 0)
            {
                _magentaTime -= 1 * Time.deltaTime;
                _magentaTimer.text = _magentaTime.ToString("F") + "s";
                _magentaTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                _magentaTime = 0;
                _magentaLoading = false;
                _magentaTimer.transform.parent.GetComponent<Button>().interactable = true;
                _magentaTimer.text = "";
            }
        }

        if (_cyanLoading)
        {
            if(_cyanTime == 0)
            {
                _cyanTime = _cyanReloadTime;
            }
            else if (_cyanTime > 0)
            {
                _cyanTime -= 1 * Time.deltaTime;
                _cyanTimer.text = _cyanTime.ToString("F") + "s";
                _cyanTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                _cyanTime = 0;
                _cyanLoading = false;
                _cyanTimer.transform.parent.GetComponent<Button>().interactable = true;
                _cyanTimer.text = "";
            }
        }

        if (_yellowLoading)
        {
            if(_yellowTime == 0)
            {
                _yellowTime = _yellowReloadTime;
            }
            else if (_yellowTime > 0)
            {
                _yellowTime -= 1 * Time.deltaTime;
                _yellowTimer.text = _yellowTime.ToString("F") + "s";
                _yellowTimer.transform.parent.GetComponent<Button>().interactable = false;
            }
            else
            {
                _yellowTime = 0;
                _yellowLoading = false;
                _yellowTimer.transform.parent.GetComponent<Button>().interactable = true;
                _yellowTimer.text = "";
            }
        }
    }
    #endregion

    private void Regen()
    {

    }
}
