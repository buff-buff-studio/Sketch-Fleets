using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SketchFleets.ProfileSystem;
using TMPro;

namespace SketchFleets
{
    public class PencilBoxText : MonoBehaviour
    {
        private TextMeshProUGUI txtMeshPro;
        private void Start()
        {
            TryGetComponent(out txtMeshPro);
        }
        void Update()
        {
            txtMeshPro.text = Profile.Data.TotalCoins.ToString();
        }
    }
}
