using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float velocity = 8;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.right * velocity) * Time.deltaTime;
        if (transform.position.x > 9)
        {
            Destroy(gameObject);
        }
    }
}
