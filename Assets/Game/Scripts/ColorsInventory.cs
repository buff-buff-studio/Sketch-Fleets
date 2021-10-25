using System;
using System.Collections;
using System.Collections.Generic;
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
        private GameObject colorInvPrefab;        
        [SerializeField]
        private Transform colorInvParent;
        
        [SerializeField] 
        private List<Color> colorsInventory;
        [SerializeField]
        private List<Image> colorsSlot;

        public int invCol;

        public Color drawColor => colorsInventory[colorsInventory.Count-1];
        
        protected readonly int redMultiplier = Shader.PropertyToID("_redMul");

        private void Awake()
        {
            colorsSlot.Clear();
            colorsInventory.Clear();
            
            for (int i = 0; i < invCol; i++)
            {
                colorsSlot.Add(Instantiate(colorInvPrefab, colorInvParent).GetComponent<Image>());
                
                colorsInventory.Add(Color.black);
            }
            
            ColorUpdate();
        }

        private void Update()
        {
            if (enemyDeathColor != Color.black)
            {
                NewColor(enemyDeathColor);
            }
        }

        private void ColorUpdate()
        {
            for (int i = 0; i < colorsSlot.Count; i++)
            {
                colorsSlot[i].color = colorsInventory[i];
                //colorsSlot[i].GetComponent<Image>().material.SetColor(redMultiplier, colorsInventory[i]);
            }
        }

        private void NewColor(Color col)
        {
            enemyDeathColor.Value = Color.black;
            for (int i = 0; i < colorsSlot.Count-1; i++)
            {
                colorsInventory[i] = colorsInventory[i+1];
            }
            colorsInventory[colorsSlot.Count-1] = col;

            ColorUpdate();
        }

        public void UseColor()
        {
            enemyDeathColor.Value = Color.black;

            for (int i = colorsSlot.Count-1; i > 0; i--)
            {
                colorsInventory[i] = colorsInventory[i-1];
            }
            colorsInventory[0] = Color.black;
            ColorUpdate();
        }
    }
}
