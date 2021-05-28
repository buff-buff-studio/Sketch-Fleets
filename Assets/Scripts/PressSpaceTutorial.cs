using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class PressSpaceTutorial : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
                gameObject.SetActive(false);
        }
    }
}
