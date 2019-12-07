using UnityEngine;

/// <summary>
/// Singleton erbt von MonoBehaviour
/// </summary>
public abstract class MonoSingleton : MonoBehaviour
{
	internal static bool Quitting { get; private set; }

	protected abstract void ApplicationQuitInstance();

	protected void OnApplicationQuit()
	{
		// Setze bei Herunterfahren
		Quitting = true;
		// Singleton Quit aufrufen
		ApplicationQuitInstance();
	}
}

/// <summary>
/// Singleton Pattern Implementierung
/// </summary>
/// <typeparam name="T">Beliebiges MonoBehaviour</typeparam>
public abstract class Singleton<T> : MonoSingleton where T : MonoBehaviour
{

	#region Fields

	[SerializeField] private bool _persistent = true;
	private bool _wasAwoken = false;

	private static readonly object _lock = new object();
	private static T _instance;
	
	#endregion

	#region Properties

	/// <summary>
	/// Die Singleton Instanz
	/// </summary>
	public static T Instance
	{
		get
		{
			// Beim Herunterfahren keine Instanz mehr zurueckgeben
			if (Quitting)
			{
				return null;
			}
			else
			{
				// Ansonsten Instanz zureckgeben
				return _instance;
			}
		}
	}

	/// <summary>
	/// Ist Singleton Instanz (noch/bereits/bald) Null?
	/// </summary>
	public static bool IsNull
	{
		get
		{
			return _instance == null || Quitting;
		}
	}

	#endregion

	#region  Methods

	protected abstract void AwakeInstance();

	protected abstract void DestroyInstance();

	protected void Awake()
	{
		// Es darf nur eine Instanz geben
		if (_instance != null)
		{
			// Singleton wird daher geloescht
			Destroy(this);
		}
		else
		{
			// Threadsafe
			lock (_lock)
			{
				// Alle Instanzen in der Szene holen
				T[] allInstances = FindObjectsOfType<T>();
				int instanceCount = allInstances.Length;
				// Falls es Instanzen gibt
				if (instanceCount > 0)
				{
					// Eine Instanz -> Diese speichern und zurueckgeben
					if (instanceCount == 1)
					{
						_instance = allInstances[0];
					}
					// Zu viele Instanzen -> Die anderen zerstoeren
					else
					{
						for (int i = 1; i < allInstances.Length; i++)
						{
							Destroy(allInstances[i]);
						}
						// Instanz ist die erste gefundene
						_instance = allInstances[0];
					}
				}
				else
				{
					// Sonst Instanz erstellen
					_instance = new GameObject(typeof(T).ToString() + "_Singleton").AddComponent<T>();
				}
			}
			// Singleton Awake aufrufen
			AwakeInstance();
			_wasAwoken = true;
			// Lasse Objekt ggf. durch Szenen hinweg bestehend
			if (_persistent)
			{
				DontDestroyOnLoad(this);
			}
		}
	}

	protected void OnDestroy()
	{
		// ggf. Singleton Destroy aufrufen
		if (_wasAwoken)
		{
			DestroyInstance();
		}
	}

	#endregion

}