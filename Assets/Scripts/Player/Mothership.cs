using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class Mothership : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    private FloatReference _life;
    [SerializeField]
    private FloatReference _speed;
    [SerializeField]
    private FloatReference _bulletForce;
    private int _cyanShips;
    #endregion
    #region Public Fields
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    #endregion
    #region Unity Callback
    void Start()
    {
        
    }

    void Update()
    {
        MothershipMovement();
        MothershipShoot();
        MothershipCyanShoot();
    }
    #endregion
    #region Commands Mothership
    /// <summary>
    /// WASD move to up, down, left and right; Q and E rotate the ship
    /// </summary>
    private void MothershipMovement()
    {
        float _timeVelocity = _speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W) && transform.position.y < 14)
        {
            transform.position += (Vector3.up * _timeVelocity);
        }

        if (Input.GetKey(KeyCode.S) && transform.position.y > -14)
        {
            transform.position += (Vector3.down * _timeVelocity);
        }

        if (Input.GetKey(KeyCode.A) && transform.position.x > -23)
        {
            transform.position += (Vector3.left * _timeVelocity);
        }

        if (Input.GetKey(KeyCode.D) && transform.position.y < 23)
        {
            transform.position += (Vector3.right * _timeVelocity);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles += new Vector3(0, 0, _timeVelocity * 6);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles -= new Vector3(0, 0, _timeVelocity * 6);
        }
    }
    /// <summary>
    /// Mouse 0 shoot with the Mothership
    /// </summary>
    private void MothershipShoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpawn.up * _bulletForce, ForceMode2D.Impulse);
        }
    }
    /// <summary>
    /// Mouse 1 launch the cyan ship
    /// </summary>
    private void MothershipCyanShoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _cyanShips = GetComponent<GenerateShips>()._cyanShips;            
            if (_cyanShips > 0)
            {
                Rigidbody2D _cyan = transform.GetChild(3).GetChild(2).GetComponent<Rigidbody2D>();
                _cyan.AddForce(_cyan.transform.GetChild(1).up * _bulletForce, ForceMode2D.Impulse);
                _cyan.transform.parent = transform.parent;
                GetComponent<GenerateShips>()._cyanShips--;
            }

        }
    }
    #endregion
}
