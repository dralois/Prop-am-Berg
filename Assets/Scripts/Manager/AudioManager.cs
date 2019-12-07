using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Managers/AudioManager")]
public class AudioManager : Singleton<AudioManager>
{

	#region Enums

	public enum AudioType : int
	{
		Music = 0,
		Sound
	}

	#endregion

	#region Fields

	[Header("Settings")]
	[Range(0f, 1f)] [SerializeField] private float _maxPitchOffset = 0.1f;
	[Range(0f, 0.5f)] [SerializeField] private float _maxVolumeOffset = 0.25f;

	private ObjectPool<AudioSource>[] _pools;
	private List<AudioSource>[] _playingSources;
	private AudioSource[] _oneShotSources;

	#endregion

	#region Methods

	private float X_GetVolume(AudioType type)
	{
		switch (type)
		{
			case AudioType.Music:
				return /*SettingsManager.Instance.CurrentSettings.MusicVolume*/ 1f;
			case AudioType.Sound:
				return /*SettingsManager.Instance.CurrentSettings.SoundVolume*/ 1f;
			default:
				Debug.LogErrorFormat(this, "No volume setting for type {0}", type);
				return 0f;
		}
	}

	private AudioSource X_GetNewSource(AudioType type, AudioClip clip)
	{
		// Source aus dem Pool holen
		AudioSource source = _pools[(int)type].GetObject();
		// Einrichten
		source.clip = clip;
		source.pitch = 1f;
		source.loop = false;
		source.playOnAwake = false;
		source.volume = X_GetVolume(type);
		// Aktivieren
		source.gameObject.SetActive(true);
		// In Liste eintragen
		_playingSources[(int)type].Add(source);
		// Source zurueck
		return source;
	}

	private bool X_FindActiveSource(AudioType type, AudioClip clip, out AudioSource source)
	{
		// Finde die aktive Source
		source = _playingSources[(int)type].Find(val => val.clip == clip);
		// Zurueck ob Source gefunden
		return source != null;
	}

	private void X_SetupSource(AudioSource source, bool looping, bool randomize)
	{
		// Pitch und Volume wird randomisiert
		source.pitch = randomize ? Random.Range(1f - _maxPitchOffset, 1f + _maxPitchOffset) : 1f;
		source.volume = randomize ? Random.Range(1f - _maxVolumeOffset, 1f) : 1f;
		source.loop = looping;
	}

	public void Play(AudioType type, AudioClip clip, bool looping = false, bool randomize = false, bool noDuplicate = false)
	{
		AudioSource source;
		// Falls Duplikate verboten und Clip wird abgespielt, dann verlassen
		if (noDuplicate && X_FindActiveSource(type, clip, out _))
		{
			return;
		}
		else
		{
			// Source holen
			source = X_GetNewSource(type, clip);
		}
		// Einrichten und abspielen
		X_SetupSource(source, looping, randomize);
		source.Play();
	}

	public void PlayOneShot(AudioType type, AudioClip clip, bool looping = false, bool randomize = false)
	{
		// Source holen
		AudioSource source = _oneShotSources[(int)type];
		// Einrichten
		X_SetupSource(source, looping, randomize);
		// Clip und Volume ueberschreiben
		source.volume *= X_GetVolume(type);
		source.clip = clip;
		// Dann abspielen
		source.Play();
	}

	public double Enqueue(AudioType type, AudioClip clip, double playAfter = 0d, bool looping = false, bool randomize = false)
	{
		AudioSource source = X_GetNewSource(type, clip);
		// Einrichten
		X_SetupSource(source, looping, randomize);
		// Momentane Spitze bestimmen
		double playTime = AudioSettings.dspTime > playAfter ? AudioSettings.dspTime : playAfter;
		// An Spitze schedulen
		source.PlayScheduled(playTime);
		// Neue Spitze zurueck
		return playTime + ((double)clip.samples / clip.frequency) / Mathf.Abs(source.pitch);
	}

	public void Pause(AudioType type, AudioClip clip)
	{
		// Falls Source noch aktiv
		if (X_FindActiveSource(type, clip, out AudioSource source))
		{
			// Source pausieren
			source.Pause();
		}
	}

	public void UnPause(AudioType type, AudioClip clip)
	{
		// Falls Source noch aktiv
		if (X_FindActiveSource(type, clip, out AudioSource source))
		{
			// Source wieder abspielen
			source.UnPause();
		}
	}

	public void Stop(AudioType type, AudioClip clip)
	{
		// Falls Source noch aktiv
		if (X_FindActiveSource(type, clip, out AudioSource source))
		{
			// Source stoppen
			source.Stop();
		}
	}

	public void StopAll(AudioType type)
	{
		// Alle Sources
		foreach (AudioSource source in _playingSources[(int)type])
		{
			// Stoppen
			source.Stop();
		}
	}

	public void PauseAll(AudioType type)
	{
		// Alle Sources
		foreach (AudioSource source in _playingSources[(int)type])
		{
			// Pausieren
			source.Pause();
		}
	}

	public void UnPauseAll(AudioType type)
	{
		// Alle Sources
		foreach (AudioSource source in _playingSources[(int)type])
		{
			// Wieder abspielen
			source.UnPause();
		}
	}

	#region Unity

	private void Update()
	{
		// Alle Typen
		for (int i = 0; i < _playingSources.Length; i++)
		{
			// Alle aktiven Sources
			for (int j = _playingSources[i].Count - 1; j >= 0; j--)
			{
				// Falls nicht mehr aktiv
				if (!_playingSources[i][j].isPlaying)
				{
					// Zurueck in den Pool
					_playingSources[i][j].gameObject.SetActive(false);
					_pools[i].ReturnToPool(_playingSources[i][j]);
					_playingSources[i].RemoveAt(j);
				}
			}
		}
	}

	protected override void AwakeInstance()
	{
		// Anzahl an Typen bestimmen
		int typeCount = System.Enum.GetValues(typeof(AudioType)).Length;
		// Caches & Pools initialisieren
		_pools = new ObjectPool<AudioSource>[typeCount];
		_playingSources = new List<AudioSource>[typeCount];
		_oneShotSources = new AudioSource[typeCount];
		// Initial befuellen
		for (int i = 0; i < typeCount; i++)
		{
			// Pools befuellen
			_pools[i] = new ObjectPool<AudioSource>(hideInInspector: true, initialCapacity: 5, addCapacity: 2);
			// Listen erstellen
			_playingSources[i] = new List<AudioSource>();
			// One Shot Sources erstellen
			_oneShotSources[i] = _pools[i].GetObject();
			_oneShotSources[i].gameObject.SetActive(true);
		}
	}

	protected override void DestroyInstance()
	{
		// Alles Anhalten
		foreach (AudioType type in System.Enum.GetValues(typeof(AudioType)))
		{
			StopAll(type);
		}
		// Aufraeumen
		for (int i = 0; i < _pools.Length; i++)
		{
			_pools[i].Dispose();
		}
	}

	protected override void ApplicationQuitInstance() { }

	#endregion

	#endregion

}
