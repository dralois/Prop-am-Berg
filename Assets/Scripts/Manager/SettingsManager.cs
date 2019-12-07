using System.Reflection;
using UnityEngine;
using UnityEditor;

[AddComponentMenu("Scripts/Managers/SettingsManager")]
public class SettingsManager : Singleton<SettingsManager>
{

	#region Classes

	[System.Serializable]
	public class Settings : object
	{
		private const int MAXLIVES = 10;

		[SerializeField] private bool _itemsSpawn = true;
		[SerializeField] private bool _battleRoyale = false;
		[Range(1, MAXLIVES)] [SerializeField] private int _FFALives = 3;
		[Range(0, 1)] [SerializeField] private float _musicVolume = 1.0f;
		[Range(0, 1)] [SerializeField] private float _soundVolume = 1.0f;

		public bool ItemsSpawn { get => _itemsSpawn; set => _itemsSpawn = value; }
		public bool BattleRoyale { get => _battleRoyale; set => _battleRoyale = value; }
		public int FFALives
		{
			get => _FFALives;
			set
			{
				_FFALives = value < 1 ? 1 : value > MAXLIVES ? MAXLIVES : value;
			}
		}
		public float MusicVolume
		{
			get => _musicVolume;
			set
			{
				_musicVolume = value < 0f ? 0f : value > 1f ? 1f : value;
			}
		}
		public float SoundVolume
		{
			get => _soundVolume;
			set
			{
				_soundVolume = value < 0f ? 0f : value > 1f ? 1f : value;
			}
		}

		public int GetLives()
		{
			return BattleRoyale ? 1 : FFALives;
		}
	}

	#endregion

	#region Fields

	[SerializeField] private Settings _currSettings;
	private Settings _networkedSettings;

	#endregion

	#region Properties

	public bool SettingsLoaded { get; private set; }

	public Settings CurrentSettings
	{
		get
		{
			// Settings noch nicht geladen -> Laden
			if (_currSettings == null)
				CurrentSettings = LoadSettings();
			// Settings zurueck
			return _currSettings;
		}
		private set
		{
			_currSettings = value;
			SettingsLoaded = true;
		}
	}

	public Settings NetworkedSettings
	{
		get
		{
			// ggf. Default-Settings erstellen
			if (_networkedSettings == null)
				NetworkedSettings = new Settings();
			// Settings zurueck
			return _networkedSettings;
		}
		private set
		{
			_networkedSettings = value;
		}
	}

	#endregion

	#region Methods

	public void Save()
	{
		// Aktuelle Settings speichern
		SaveSettings(_currSettings);
	}

	private void SaveSettings(Settings toSave)
	{
		// Hole die Properties der Klasse
		PropertyInfo[] properties =  typeof(Settings).GetProperties();
		// Loope alle Properties
		foreach(PropertyInfo property in properties)
		{
			// Wert aus Save holen
			object propVal = property.GetValue(toSave);
			// Je nach Type parsen und speichern
			if (propVal is int)
			{
				PlayerPrefs.SetInt(property.Name, (int)propVal);
			}
			else if(propVal is bool)
			{
				PlayerPrefs.SetInt(property.Name, (bool)propVal ? 1 : 0);
			}
			else if(propVal is string)
			{
				PlayerPrefs.SetString(property.Name, (string)propVal);
			}
			else if(propVal is float)
			{
				PlayerPrefs.SetFloat(property.Name, (float)propVal);
			}
			else
			{
				Debug.LogErrorFormat("{0} can not be save, type {1} is not supported!", property.Name, propVal.GetType());
			}
		}
		// Alles flushen und speichern
		PlayerPrefs.Save();
	}

	public void Load()
	{
		// Lade Settings aus PlayerPrefs
		CurrentSettings = LoadSettings();
	}

	private Settings LoadSettings()
	{
		// Neue Default Settings erstellen
		Settings toLoad = new Settings();
		// Alle Properties holen
		PropertyInfo[] properties = typeof(Settings).GetProperties();
		// Properties loopen
		foreach (PropertyInfo property in properties)
		{
			// Default Wert holen
			object propVal = property.GetValue(toLoad);
			// Je nach Type Property aktualisieren
			if (propVal is int)
			{
				property.SetValue(toLoad, PlayerPrefs.GetInt(property.Name, (int)propVal));
			}
			else if (propVal is bool)
			{
				property.SetValue(toLoad, PlayerPrefs.GetInt(property.Name, (bool)propVal ? 1 : 0) == 1 ? true : false);
			}
			else if (propVal is string)
			{
				property.SetValue(toLoad, PlayerPrefs.GetString(property.Name, (string)propVal));
			}
			else if (propVal is float)
			{
				property.SetValue(toLoad, PlayerPrefs.GetFloat(property.Name, (float)propVal));
			}
			else
			{
				Debug.LogErrorFormat("{0} can not be loaded, type {1} is not supported!", property.Name, propVal.GetType());
			}
		}
		// Geladene und geparste Settings zurueck
		return toLoad;
	}

	#region Unity

	protected override void AwakeInstance() { }

	protected override void DestroyInstance() { }

	protected override void ApplicationQuitInstance()
	{
		// Speichern vorm Schliessen
		Save();
	}

	#endregion

	#endregion

}


#if UNITY_EDITOR
[CustomEditor(typeof(SettingsManager))]
public class SettingsManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		// Beim ersten Aufruf Settings laden
		if(!((SettingsManager)target).SettingsLoaded)
		{
			((SettingsManager)target).Load();
		}

		if (GUILayout.Button("Save"))
		{
			((SettingsManager)target).Save();
		}

		if (GUILayout.Button("Load"))
		{
			((SettingsManager)target).Load();
		}
	}
}
#endif