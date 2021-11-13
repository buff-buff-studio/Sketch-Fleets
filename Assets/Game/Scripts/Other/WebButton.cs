using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SketchFleets
{
    public class WebButton : MonoBehaviour
    {
    public void GoURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
