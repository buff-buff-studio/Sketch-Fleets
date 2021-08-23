using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class MapEndScript : MonoBehaviour
    {
        public GameObject WinMenu;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {

            }
        }
    }
}
