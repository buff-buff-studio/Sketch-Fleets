using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class MenuParallax : MonoBehaviour
    {
        [SerializeField]
        private Transform cam;
        [SerializeField]
        [Range(0,5)]
        private float camVel;
        void Start()
        {
            transform.GetChild(Random.Range(0,transform.childCount)).gameObject.SetActive(true);
        }

        private void Update()
        {
            cam.position += Vector3.right * Time.deltaTime * camVel;
        }
    }
}
