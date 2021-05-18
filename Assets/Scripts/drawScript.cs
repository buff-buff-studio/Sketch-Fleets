using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets
{
    public class drawScript : MonoBehaviour
    {
        private int colorNum;
        private int textureNum;
        private SpriteRenderer sr;
        private Ray mouseRay;

        public SpriteRenderer DrawPrefab;
        public List<Sprite> DrawTexture;
        public List<Color> DrawColor;
        [Range(.05f,1)]
        public float DrawSize;
        public Image ColorChange;
        public bool MouseOverButton;

        void Start()
        {
            sr = this.GetComponent<SpriteRenderer>();
            StartCoroutine(drawTimer());

            ColorChange.color = DrawColor[colorNum];

            sr.color = new Color(DrawColor[colorNum].r, DrawColor[colorNum].g, DrawColor[colorNum].b, .25f);
            sr.sprite = DrawTexture[textureNum];
            transform.localScale = Vector3.one * DrawSize;
        }

        private void Update()
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float pos = 0;

            transform.position = mouseRay.GetPoint(pos);

            if (Input.GetKey(KeyCode.LeftControl))
            {
                ChangeTexture();
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ChangeSize();
            }
        }
        private void LoadingDraw()
        {
            if (Input.GetKey(KeyCode.Mouse0) && !MouseOverButton)
            {
                float pos = 0;
                SpriteRenderer draw = (SpriteRenderer)Instantiate(DrawPrefab, mouseRay.GetPoint(pos), transform.rotation);
                draw.sprite = DrawTexture[textureNum];
                draw.color = DrawColor[colorNum];
                draw.transform.localScale = Vector3.one * DrawSize;
            }
            StartCoroutine(drawTimer());
        }

        private void ChangeTexture()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                textureNum = (textureNum < DrawTexture.Count - 1) ? textureNum + 1 : 0;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                textureNum = (textureNum == 0) ? DrawTexture.Count - 1 : textureNum - 1;
            }
            sr.sprite = DrawTexture[textureNum];
        }

        private void ChangeSize()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                DrawSize = (DrawSize >= 1) ? DrawSize = .05f : DrawSize + .05f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                DrawSize = (DrawSize <= .05f) ? DrawSize = 1 : DrawSize - .05f;
            }
            transform.localScale = Vector3.one * DrawSize;
        }

        public void ChangeColor()
        {
            colorNum = (colorNum < DrawColor.Count - 1) ? colorNum + 1 : 0;
            ColorChange.color = DrawColor[colorNum];
            sr.color = new Color(DrawColor[colorNum].r, DrawColor[colorNum].g, DrawColor[colorNum].b, .25f);
        }

        IEnumerator drawTimer()
        {
            yield return new WaitForFixedUpdate();
            LoadingDraw();
        }
    }
}
