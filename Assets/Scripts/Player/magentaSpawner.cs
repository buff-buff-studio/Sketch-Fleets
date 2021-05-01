 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magentaSpawner : MonoBehaviour
{
    public Transform Mothership;
    void Update()
    {
        transform.RotateAround(Mothership.position, Mothership.forward, 90f * Time.deltaTime);
    }
}
