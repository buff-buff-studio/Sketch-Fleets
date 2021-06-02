using UnityEngine;
using TMPro;
using ManyTools.Variables;

namespace SketchFleets
{
    public class SetTextToInt : MonoBehaviour
    {
        [SerializeField]
        private IntReference intRef;
        private TextMeshProUGUI textMesh;
        void Start()
        {
            TryGetComponent(out textMesh);
        }
        void Update()
        {
            textMesh.text = intRef.ToString();
        }
    }
}
