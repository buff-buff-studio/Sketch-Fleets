using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class PositionInScreen : MonoBehaviour
    {
        public bool isFront;
        void Start()
        {
            float posX;
            if(isFront)
                posX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + (transform.localScale.x*2.5f);
            else
                posX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - (transform.localScale.x*2.5f);
            
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }
}
