using UnityEngine;

/// <summary>
/// A base for all singletons
/// </summary>
/// <typeparam name="T">The type T inheritor of SingletonBase</typeparam>
public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
	#region Private Fields

	// The global instance reference of the singleton
	private static T instance;

	#endregion

	#region Properties

	public static T Instance
	{
		get => instance;
	}

	// An accessor to check whether the singleton has been initialized
	public static bool IsInitialized
	{
		get => instance != null;
	}

	#endregion

	#region Unity Callbacks

	// Overrideable check if another instance exists on awake
	protected virtual void Awake()
	{
		if (instance != null)
		{
			Debug.LogError($"A second instance of singleton {(T) this} has been initialized!");
		}
		else
		{
			// Set the instance to this class (typecasted to the given type)
			instance = (T) this;
		}
	}

	// An overrideable clear instance action
	protected virtual void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	#endregion
}