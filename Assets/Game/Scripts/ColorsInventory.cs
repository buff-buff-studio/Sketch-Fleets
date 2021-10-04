using System;
using System.Collections;
using System.Collections.Generic;
using ManyTools.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    public class ColorsInventory : MonoBehaviour
    {
        [SerializeField]
        private Color[] colorsInventory = new Color[3]{Color.black, Color.black, Color.black};
        [Tooltip("The color of the last enemy killed")]
        [SerializeField]
        private ColorReference enemyDeathColor;

        [SerializeField] 
        private Image[] colorsSlot = new Image[3];

        public Color drawColor => colorsInventory[0];

        private void Update()
        {
            if (enemyDeathColor != Color.black)
            {
                NewColor(enemyDeathColor);
            }
        }

        private void ColorUpdate()
        {
            colorsSlot[0].color = colorsInventory[0];
            colorsSlot[1].color = colorsInventory[1];
            colorsSlot[2].color = colorsInventory[2];
        }

        private void NewColor(Color col)
        {
            enemyDeathColor.Value = Color.black;
            
            colorsInventory[2] = colorsInventory[1];
            colorsInventory[1] = colorsInventory[0];
            colorsInventory[0] = col;

            ColorUpdate();
        }

        public void UseColor()
        {
            enemyDeathColor.Value = Color.black;
            
            colorsInventory[0] = colorsInventory[1];
            colorsInventory[1] = colorsInventory[2];
            colorsInventory[2] = Color.black;

            ColorUpdate();
        }
    }
}
