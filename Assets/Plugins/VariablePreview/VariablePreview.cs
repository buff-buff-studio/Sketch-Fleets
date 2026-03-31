#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SketchFleets.Plugins
{
    [CustomPropertyDrawer(typeof(VariablePreviewAttribute))]
    public class VariablePreview : PropertyDrawer
    {
        private static Dictionary<int, Texture> previewCache = new Dictionary<int, Texture>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ((VariablePreviewAttribute)attribute).height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect contentRect = EditorGUI.PrefixLabel(position, label);
            contentRect.xMax -= 4;

            float size = contentRect.height;

            Rect previewRect = new Rect(
                contentRect.x,
                contentRect.y,
                size,
                size
            );

            Object obj = property.objectReferenceValue;

            // 🔲 Fundo padrão Unity
            if (Event.current.type == EventType.Repaint)
            {
                EditorStyles.objectFieldThumb.Draw(
                    previewRect,
                    GUIContent.none,
                    false, false, false, false
                );
            }

            // 🧠 Controle de mudança + Undo
            EditorGUI.BeginChangeCheck();

            Object newValue = EditorGUI.ObjectField(
                previewRect,
                property.objectReferenceValue,
                fieldInfo.FieldType,
                false
            );

            if (EditorGUI.EndChangeCheck())
            {
                // 🔥 Undo (ESSENCIAL)
                foreach (var target in property.serializedObject.targetObjects)
                {
                    Undo.RecordObject(target, "Change Object Reference");
                }

                property.objectReferenceValue = newValue;

                // 🔥 aplica mudança
                property.serializedObject.ApplyModifiedProperties();

                // 🔥 marca dirty corretamente (SO / assets)
                foreach (var target in property.serializedObject.targetObjects)
                {
                    EditorUtility.SetDirty(target);
                }
            }

            // 🎯 Preview custom
            if (obj != null && !IsImage(obj))
            {
                int id = obj.GetInstanceID();
                Texture preview = null;

                if (!previewCache.TryGetValue(id, out preview) || preview == null)
                {
                    preview = AssetPreview.GetAssetPreview(obj);

                    if (preview == null && IsUIObject(obj))
                    {
                        preview = GetUIPreview(obj);
                    }

                    if (preview != null)
                    {
                        previewCache[id] = preview;
                    }
                    else
                    {
                        preview = AssetPreview.GetMiniThumbnail(obj);
                    }
                }

                if (preview != null)
                {
                    Rect inner = EditorStyles.objectFieldThumb.padding.Remove(previewRect);

                    if (Event.current.type == EventType.Repaint)
                    {
                        GUI.DrawTexture(inner, preview, ScaleMode.ScaleToFit);
                    }
                }

                // 🔄 força repaint enquanto preview carrega
                if (AssetPreview.IsLoadingAssetPreview(id))
                {
                    EditorApplication.QueuePlayerLoopUpdate();
                }
            }

            EditorGUI.EndProperty();
        }

        private bool IsImage(Object obj)
        {
            return obj is Texture2D || obj is Sprite;
        }

        private bool IsUIObject(Object obj)
        {
            if (obj is GameObject go)
                return go.GetComponentInChildren<RectTransform>() != null;

            return false;
        }

        private Texture GetUIPreview(Object obj)
        {
            if (obj is GameObject go)
            {
                var image = go.GetComponentInChildren<Image>();
                if (image != null && image.sprite != null)
                    return image.sprite.texture;

                var raw = go.GetComponentInChildren<RawImage>();
                if (raw != null && raw.texture != null)
                    return raw.texture;
            }

            return null;
        }
    }
}
#endif