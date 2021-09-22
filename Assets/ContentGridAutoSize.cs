using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    public class ContentGridAutoSize : MonoBehaviour
    {
        private ScrollRect scrollRect => this.GetComponent<ScrollRect>();
        private RectTransform scrollView => this.transform.parent.GetComponent<RectTransform>();
        private GridLayoutGroup gridLayoutGroup => scrollRect.content.GetComponent<GridLayoutGroup>();
        private Scrollbar scrollbar => scrollRect.verticalScrollbar;
        
        void Start()
        {
            scrollbar.value = 1;
            gridLayoutGroup.cellSize = scrollView.rect.size;
        }

        private void OnEnable()
        {
            gridLayoutGroup.cellSize = scrollView.rect.size;
        }
        
    }
}
