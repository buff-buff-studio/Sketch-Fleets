using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    public class ItemStackAnimation : MonoBehaviour
    {         
        #region Public Fields
        [Header("Rotation")]
        public float minRotation = 16;
        public float maxRotation = 25;
        
        [Header("Initial Rotation")]
        public float minRotationInitial = 15; 
        public float maxRotationInitial = 15;

        [Header("Rotation Speed")]
        public float minRotationSpeed = 160;
        public float maxRotationSpeed = 200;

        [Header("Scale Difference")]
        private float minScaleDifference = 0.05f;
        private float maxScaleDifference = 0.15f;

        [Header("Scale Speed")]
        public float minScaleSpeed = 120f;
        public float maxScaleSpeed = 180f;

        [Header("Time Offset")]
        public float minTimeOffset = 0;
        public float maxTimeOffset = 7;

        public bool hovering = false;
        #endregion

        #region Private Fields
        private RectTransform rectTransform;
        
        private float rotation;
        private float rotationInitial;
        private float rotationSpeed;

        private float scaleDifference;
        private float scaleSpeed;

        private float randomTimeOffset;

        private Image image;
        private float targetAlpha = 1f;

        #endregion

        void Start()
        {
            this.rectTransform = GetComponent<RectTransform>();
            this.image = GetComponent<Image>();
            
            rotation = Random.Range(minRotation,maxRotation);
            rotationInitial = Random.Range(minRotationSpeed,maxRotationSpeed);
            rotationSpeed = Random.Range(minRotationSpeed,maxRotationSpeed);

            scaleSpeed = Random.Range(minScaleSpeed,maxScaleSpeed);
            scaleDifference = Random.Range(minScaleDifference,maxScaleDifference);

            randomTimeOffset = Random.Range(minTimeOffset,maxTimeOffset);
        }

        // Update is called once per frame
        void Update()
        {
            rectTransform.localEulerAngles = new Vector3(0,0,180 + rotationInitial + rotation * Mathf.Sin((Time.time + randomTimeOffset) * rotationSpeed * Mathf.Deg2Rad));
            rectTransform.localScale = Vector3.one * (1 + Mathf.Cos((Time.time + randomTimeOffset ) * scaleSpeed * Mathf.Deg2Rad) * scaleDifference);

            image.color = hovering ? new Color(1,1,1,targetAlpha) : new Color(1,1,1,1);

            targetAlpha = Mathf.Sin(Time.time * Mathf.Deg2Rad * 540)/3f + 1/3f + 0.5f;
        }
    }
}
