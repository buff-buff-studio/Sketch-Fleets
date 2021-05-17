using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class cyanAI : MonoBehaviour
{
    [SerializeField]
    private FloatReference life;

    public bool shoot;
    public float Damage;
    void Update()
    {
        if (shoot)
        {
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && !shoot)
        {
            Destroy(gameObject);
            if (col.gameObject.name == "LimeEnemy(Clone)")
            {
                life.Value += col.GetComponent<limeAI>().LimeLife;
            }
            else if (col.gameObject.name == "PurpleEnemy(Clone)")
            {
                life.Value += col.GetComponent<purpleAI>().PurpleLife;
            }
            else if (col.gameObject.name == "OrangeEnemy(Clone)")
            {
                life.Value += col.GetComponent<orangeAI>().OrangeLife;
            }
        }
        else if (col.gameObject.CompareTag("EndMap") && !shoot)
        {
            Destroy(gameObject);
        }
    }
}
