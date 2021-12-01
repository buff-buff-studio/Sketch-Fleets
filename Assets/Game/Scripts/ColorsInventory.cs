using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Events;
using ManyTools.UnityExtended.Editor;
using ManyTools.UnityExtended.Poolable;
using ManyTools.Variables;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    public class ColorsInventory : MonoBehaviour
    {
        [Tooltip("The color of the last enemy killed")]
        [SerializeField]
        private ColorReference enemyDeathColor;        
        [SerializeField]
        private GameObjectReference enemyDeathBullet;
        
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

        private int invCol => 2 + ProfileSystem.Profile.Data.ColorUpgradeCount;

        public Color drawColor => colorsInventory[colorsInventory.Count-1].color;
        public GameObject bulletColor => colorsInventory[colorsInventory.Count-1].bulletPrefab;
        
        protected readonly int redMultiplier = Shader.PropertyToID("_redMul");

        private void Awake()
        {
            colorsSlot.Clear();
            colorsInventory.Clear();
            
            for (int i = 0; i < invCol; i++)
            {
                colorsSlot.Add(Instantiate(colorInvPrefab, colorInvParent).GetComponent<Image>());
                
                colorsInventory.Add(SetColorInfo(Color.black, new GameObject()));
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
                //colorsSlot[i].GetComponent<Image>().material.SetColor(redMultiplier, colorsInventory[i]);
            }
        }

        private void NewColor(Color col, GameObject bullet)
        {
            enemyDeathColor.Value = Color.black;
            enemyDeathBullet.Value = null;
            for (int i = 0; i < colorsSlot.Count-1; i++)
            {
                colorsInventory[i] = colorsInventory[i+1];
            }
            colorsInventory[colorsSlot.Count-1] = SetColorInfo(col, bullet);;

            ColorUpdate();
            onColorAbsorbed.Invoke();
        }

        public void UseColor()
        {
            enemyDeathColor.Value = Color.black;

            for (int i = colorsSlot.Count-1; i > 0; i--)
            {
                colorsInventory[i] = colorsInventory[i-1];
            }
            colorsInventory[0] = SetColorInfo(Color.black, new GameObject());
            ColorUpdate();
        }

        private ColorInfo SetColorInfo(Color col, GameObject gm)
        {
            ColorInfo colInf;
            colInf.color = col;
            colInf.bulletPrefab = gm;
            return colInf;
        }
    }
}

[System.Serializable]
public struct ColorInfo
{
    public Color color;
    public GameObject bulletPrefab;
}
