using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;

public class bulletScript : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x > 25)
        {
            Destroy(gameObject);
        }
    }
}
