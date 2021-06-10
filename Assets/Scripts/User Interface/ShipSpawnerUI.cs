using System;
using System.Collections;
using System.Globalization;
using ManyTools.UnityExtended;
using ManyTools.Variables;
using SketchFleets.Data;
using SketchFleets.Entities;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace SketchFleets.UI
{
    /// <summary>
    /// A class that handles the UI elements of spawning a ship
    /// </summary>
    public class ShipSpawnerUI : MonoBehaviour
    {
        #region Private Fields

        [Header("Button Config")]
        [SerializeField]
        private UnityDictionary<SpawnableShipAttributes, ShipSpawnButtonElements> spawnButtons;
        [SerializeField]
        private FloatReference buttonUpdateInterval = new FloatReference(0.2f);
        [SerializeField]
        private FloatReference timeScaleWhenActive = new FloatReference(0.3f);

        private Mothership player;
        private Coroutine buttonUpdateRoutine;

        public Mothership Player
        {
            get => player;
            set => player = value;
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            Player ??= LevelManager.Instance.Player;
            buttonUpdateRoutine = StartCoroutine(UpdateButtonsSlow());
            Time.timeScale = timeScaleWhenActive;
        }

        private void OnDisable()
        {
            if (buttonUpdateRoutine != null)
            {
                StopCoroutine(buttonUpdateRoutine);
            }

            Time.timeScale = 1f;
        }

        private void Update()
        {
            UpdateAllButtonFills();            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates all fill elements of all buttons
        /// </summary>
        private void UpdateAllButtonFills()
        {
            foreach (var button in spawnButtons)
            {
                UpdateButtonFill(button.Key);
            }
        }

        /// <summary>
        /// Updates a button's price tag
        /// </summary>
        /// <param name="button">The button to update the price tag of</param>
        private void UpdateButtonPriceTag(SpawnableShipAttributes button)
        {
            spawnButtons[button].priceTag.text = Player.GetSpawnCost(button).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Updates a button's interactability
        /// </summary>
        /// <param name="button">The button to update the interactability of</param>
        private void UpdateButtonInteractability(SpawnableShipAttributes button)
        {
            spawnButtons[button].button.interactable = Player.CanSpawnShip(button);
        }

        /// <summary>
        /// Updates a button's fill
        /// </summary>
        /// <param name="button">The button to update the fill of</param>
        private void UpdateButtonFill(SpawnableShipAttributes button)
        {
            float fill = (Player.GetMaxSpawnCooldown(button) - Player.GetSpawnCooldown(button)) / Player.GetMaxSpawnCooldown(button);
            spawnButtons[button].summonReadinessFill.fillAmount = fill;
        }

        /// <summary>
        /// Updates all buttons at a slower pace, to avoid spending too much processing on the UI
        /// </summary>
        private IEnumerator UpdateButtonsSlow()
        {
            WaitForSecondsRealtime cachedInterval = new WaitForSecondsRealtime(buttonUpdateInterval);

            while (gameObject.activeSelf)
            {
                foreach (var button in spawnButtons)
                {
                    UpdateButtonPriceTag(button.Key);
                    UpdateButtonInteractability(button.Key);
                }

                yield return cachedInterval;
            }
        }

        #endregion

        #region Private Structs

        /// <summary>
        /// A struct containing all elements relating to an individual button responsible for summoning ships
        /// </summary>
        [Serializable]
        private struct ShipSpawnButtonElements
        {
            #region Public Fields

            [SerializeField]
            public TMP_Text priceTag;
            [SerializeField]
            public Button button;
            [SerializeField]
            public Image summonReadinessFill;

            #endregion

            #region Constructor

            public ShipSpawnButtonElements(TMP_Text priceTag, Button button, Image summonReadinessFill)
            {
                this.priceTag = priceTag;
                this.button = button;
                this.summonReadinessFill = summonReadinessFill;
            }

            #endregion
        }

        #endregion
    }
}