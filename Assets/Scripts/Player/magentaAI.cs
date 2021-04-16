using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class magentaAI : MonoBehaviour
{
    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField]
    private FloatReference timeReload;
    private bool shootTime = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.position, transform.parent.forward, 90f * Time.deltaTime);
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (shootTime)
            {
                GameObject bullet = (GameObject)Instantiate(BulletPrefab, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 40, ForceMode2D.Impulse);

                shootTime = false;
                StartCoroutine(ShootTimer());
            }
        }
    }

    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(timeReload);
        shootTime = true;
    }
}
