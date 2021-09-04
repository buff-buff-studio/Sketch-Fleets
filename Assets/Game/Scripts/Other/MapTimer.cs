using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ManyTools.Variables;

namespace SketchFleets
{
    public class MapTimer : MonoBehaviour
    {
        [SerializeField]
        private StringReference mapTimer;
        private TextMeshProUGUI textMesh;
        void Start()
        {
            TryGetComponent(out textMesh);
        }
        void Update()
        {
            textMesh.text = mapTimer;
        }
    }
}
