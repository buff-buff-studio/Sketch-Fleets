using SketchFleets.Data;
using UnityEditor;
using UnityEngine;

namespace SketchFleets.Editor
{
    [CustomEditor(typeof(ShipFormation))]
    public sealed class ShipFormationInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                AssetPreview.GetAssetPreview((target as ShipFormation).Formation) as Texture2D,
                GUILayout.Height(128f), GUILayout.Width(128f));
        }
    }
}