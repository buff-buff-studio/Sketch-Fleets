using UnityEngine;

namespace ManyTools.UnityExtended.Parallax
{
    /// <summary>
    /// A class that controls and defines a Parallax Layer
    /// </summary>
    [System.Serializable]
    public class ParallaxLayer
    {
        #region Private Fields

        [SerializeField]
        [Tooltip("The layer's transform component")]
        private SpriteRenderer layerRenderer;
        [SerializeField]
        [Tooltip("The layer's parallax factor. It multiplies the movement of the layer relative to the camera. " +
                 "Objects at the focus should have a factor of 0, objects infinitely away should have a factor" +
                 " of 0, and objects at the same depth of the camera should have a factor of -1.")]
        private Vector2 parallaxFactor = new Vector2(0f, 0f);
        [SerializeField]
        [Tooltip("Whether the layer should be repeated horizontally to allow for infinite scrolling")]
        private bool infiniteHorizontalScrolling = true;
        [SerializeField]
        [Tooltip("Whether the layer should be repeated vertically to allow for infinite scrolling")]
        private bool infiniteVerticalScrolling = false;

        private Vector2 textureUnitSize;

        #endregion

        #region Properties

        public GameObject LayerOriginal => layerRenderer.gameObject;
        public GameObject[] HorizontalClones { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up layer clones as necessary
        /// </summary>
        public void Setup(Transform layerParent)
        {
            if (!infiniteHorizontalScrolling && !infiniteVerticalScrolling) return;

            // Set mode to tiled to allow for seamless infinite scrolling
            layerRenderer.drawMode = SpriteDrawMode.Tiled;

            // Cache repeatedly accessed variables
            Sprite sprite = layerRenderer.sprite;
            Texture2D layerTexture = sprite.texture;
            Vector2 size = layerRenderer.size;
            Vector3 localScale = layerRenderer.transform.localScale;

            // Get pixels per unit
            if (infiniteHorizontalScrolling)
            {
                textureUnitSize.x = layerTexture.width / sprite.pixelsPerUnit * localScale.x;
                size = new Vector2(size.x * 3f, size.y);
            }

            if (infiniteVerticalScrolling)
            {
                textureUnitSize.y = layerTexture.height / sprite.pixelsPerUnit * localScale.y;
                size = new Vector2(size.x, size.y * 3f);
            }

            // Automatically extends the sprite to the necessary width
            layerRenderer.size = size;
        }

        /// <summary>
        /// Moves the layer and all existing clones
        /// </summary>
        public void Move(Vector2 deltaMovement, Vector2 cameraPosition)
        {
            // Cache repeatedly accessed data
            Transform transform = layerRenderer.transform;
            Vector3 position = transform.position;

            // Translates layer based on parallax factor
            transform.Translate(deltaMovement * parallaxFactor);

            if (infiniteHorizontalScrolling)
            {
                // If the texture hasn't fully looped, don't seamlessly move it yet
                if (Mathf.Abs(cameraPosition.x - position.x) >= textureUnitSize.x)
                {
                    // Get how much the texture moved beyond a full loop
                    float offset = (cameraPosition.x - position.x) % textureUnitSize.x;
                    // Seamlessly reposition the texture
                    position = new Vector3(cameraPosition.x + offset, position.y);
                    transform.position = position;
                }
            }

            if (infiniteVerticalScrolling)
            {
                // If the texture hasn't fully looped, don't seamlessly move it yet
                if (Mathf.Abs(cameraPosition.y - position.y) >= textureUnitSize.y)
                {
                    // Get how much the texture moved beyond a full loop
                    float offset = (cameraPosition.y - position.y) % textureUnitSize.y;
                    // Seamlessly reposition the texture
                    position = new Vector3(position.x, cameraPosition.y + offset);
                    transform.position = position;
                }
            }
        }

        #endregion
    }
}