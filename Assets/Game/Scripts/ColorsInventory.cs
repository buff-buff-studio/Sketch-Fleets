using System.Collections.Generic;
using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.ProfileSystem;
using SketchFleets.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    /// <summary>
    /// A class that manages an inventory of colors
    /// </summary>
    public sealed class ColorsInventory : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("The color of the last enemy killed")]
        [SerializeField]
        private ColorReference enemyDeathColor;

        [SerializeField]
        private BulletAttributesReference enemyDeathBullet;

        [SerializeField]
        private GameObject colorInvPrefab;

        [SerializeField]
        private Transform colorInvParent;

        [SerializeField]
        private List<ColorInfo> colorsInventory;

        [SerializeField]
        private List<Image> colorsSlot;

        [Header("Events")]
        [SerializeField]
        [RequiredField]
        private GameEvent onColorAbsorbed;

        private readonly int redMultiplier = Shader.PropertyToID("_redMul");

        #endregion

        #region Properties

        public Color drawColor => colorsInventory[colorsInventory.Count - 1].color;
        public BulletAttributes latestBullet => colorsInventory[colorsInventory.Count - 1].bulletAttributes;

        private int ColorInventoryCapacity => 2 + Profile.Data.ColorUpgradeCount;

        #endregion


        private void Awake()
        {
            colorsSlot.Clear();
            colorsInventory.Clear();

            for (int i = 0; i < ColorInventoryCapacity; i++)
            {
                colorsSlot.Add(Instantiate(colorInvPrefab, colorInvParent).GetComponent<Image>());
                colorsInventory.Add(SetColorInfo(Color.black, null));
            }

            ColorUpdate();
        }

        private void Update()
        {
            if (enemyDeathColor != Color.black)
            {
                NewColor(enemyDeathColor, enemyDeathBullet);
            }
        }

        private void ColorUpdate()
        {
            for (int i = 0; i < colorsSlot.Count; i++)
            {
                colorsSlot[i].color = colorsInventory[i].color;
            }
        }

        private void NewColor(Color col, BulletAttributes bullet)
        {
            enemyDeathColor.Value = Color.black;
            enemyDeathBullet.Value = null;

            for (int i = 0; i < colorsSlot.Count - 1; i++)
            {
                colorsInventory[i] = colorsInventory[i + 1];
            }

            colorsInventory[colorsSlot.Count - 1] = SetColorInfo(col, bullet);

            ColorUpdate();
            onColorAbsorbed.Invoke();
        }

        public void UseColor()
        {
            enemyDeathColor.Value = Color.black;

            for (int i = colorsSlot.Count - 1; i > 0; i--)
            {
                colorsInventory[i] = colorsInventory[i - 1];
            }

            colorsInventory[0] = SetColorInfo(Color.black, enemyDeathBullet);
            ColorUpdate();
        }

        private static ColorInfo SetColorInfo(Color col, BulletAttributes bullet)
        {
            ColorInfo colInf;
            colInf.color = col;
            colInf.bulletAttributes = bullet;
            return colInf;
        }
    }

    [System.Serializable]
    public struct ColorInfo
    {
        public Color color;
        public BulletAttributes bulletAttributes;
    }
}