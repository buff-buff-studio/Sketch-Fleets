using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManyTools.Variables;
using TMPro;

namespace SketchFleets
{
    public class IntReferenceText : MonoBehaviour
    {
        [SerializeField]
        private IntReference intRef;
        private TextMeshProUGUI txtMeshPro;

        private void Start()
        {
            TryGetComponent(out txtMeshPro);
        }
        void Update()
        {
            txtMeshPro.text = intRef.ToString();
        }
    }
}
