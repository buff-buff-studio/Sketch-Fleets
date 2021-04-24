using System.Collections.Generic;
using UnityEngine;

namespace ManyTools.UnityExtended
{
	/// <summary>
	/// A script that controls basic functions relating to a menu
	/// </summary>
	public class Menu : MonoBehaviour
	{
		#region Private Fields

		[SerializeField]
		[Tooltip("The click sound that happens on the UI")]
		private AudioClip clickSound;

		[SerializeField]
		[Tooltip("The AudioSource used to play interface sounds.")]
		private AudioSource menuAudioSource;

		[SerializeField]
		private UnityDictionary<string, GameObject> menuObjects = new UnityDictionary<string, GameObject>();

		#endregion

		#region Custom Methods

		/// <summary>
		/// Switches to a given menu
		/// </summary>
		/// <param name="menuKey">The desired menu's key</param>
		public void SwitchToMenu(string menuKey)
		{
			// For each menu in the menus dictionary
			foreach (GameObject menu in menuObjects.Values)
			{
				menu.SetActive(menu == menuObjects[menuKey]);
			}
		}

		/// <summary>
		/// Overlays a given menu
		/// </summary>
		/// <param name="desiredMenu">The menu to be overlayed</param>
		public void OverlayMenu(string desiredMenu)
		{
			// Sets the given menu active
			menuObjects[desiredMenu].SetActive(true);
		}

		/// <summary>
		/// Disables a menu
		/// </summary>
		/// <param name="desiredMenu">The menu to be disabled</param>
		public void DisableMenu(string desiredMenu)
		{
			// Sets the given menu inactive
			menuObjects[desiredMenu].SetActive(false);
		}

		/// <summary>
		/// Switches to a given scene through the Game Manager
		/// </summary>
		/// <param name="sceneName"></param>
		public void SwitchToScene(string sceneName)
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.LoadScene(sceneName);
			}
			else
			{
				Debug.LogError("The game manager is not initialized! Please initialize the game manager" +
				               "before switching scenes.");
			}
		}

		/// <summary>
		/// Enables or disables the mouse
		/// </summary>
		/// <param name="isEnabled">Whether to enable or disable the mouse</param>
		public void EnableMouse(bool isEnabled)
		{
			if (isEnabled == false)
			{
				// Locks the cursor and makes it invisible;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				// Unlocks the cursor and makes it visible;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		// Gets UI volume and plays sound
		private void PlayClickSound()
		{
			menuAudioSource.Play();
		}

		#endregion
	}
}
