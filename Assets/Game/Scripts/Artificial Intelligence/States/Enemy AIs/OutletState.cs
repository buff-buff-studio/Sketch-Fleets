using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SketchFleets.AI
{
    /// <summary>
    /// An AI state that aims and fires
    /// </summary>
    public sealed class OutletState : BaseEnemyAIState
    {
        #region Private Fields

        [Header("Effects")]
        [SerializeField]
        private Sprite connectedSprite;
        
        private Sprite _defaultSprite;
        private SpriteRenderer _spriteRenderer;

        #endregion
        
        #region Properties

        public bool IsConnected { get; private set; }

        #endregion

        #region Unity Callbacks

        private void OnDisable()
        {
            Unplug();
        }

        #endregion

        #region State Implementation

        public override void Enter()
        {
            CacheComponents();
            base.Enter();
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Runs at every update of the state
        /// </summary>
        public override void StateUpdate()
        {
            if (!ShouldBeActive() || !IsConnected) return;
            AI.Ship.Fire();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Plugs into the outlet
        /// </summary>
        public void Plug()
        {
            if (_spriteRenderer == null) return;

            IsConnected = true;
            _spriteRenderer.sprite = connectedSprite;
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Unplugs the outlet
        /// </summary>
        private void Unplug()
        {
            if (_spriteRenderer == null || _defaultSprite == null) return;
            
            _spriteRenderer.sprite = _defaultSprite;
            IsConnected = false;
        }

        /// <summary>
        /// Caches all necessary components
        /// </summary>
        private void CacheComponents()
        {
            TryGetComponent(out _spriteRenderer);
            _defaultSprite = _spriteRenderer.sprite;
        }

        #endregion
    }
}