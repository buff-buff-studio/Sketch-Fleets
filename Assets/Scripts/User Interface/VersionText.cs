using UnityEngine;
using TMPro;

namespace SketchFleets
{
    public class VersionText : MonoBehaviour
    {
        void Start()
        {
            GetComponent<TextMeshProUGUI>().text = "V: " + Application.version;
        }

    }
}
