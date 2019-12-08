using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{

	[SerializeField] private TextMeshProUGUI _startText = null;

	private List<GameObject> _spawns = null;
	private int _countdown = 10;

	private int _deadPlayerCount = 0;
	private int _wonPlayerCount = 0;

	public bool GameStarted { get; private set; } = false;

	public int PlayerCount { get; private set; }

	public int DeadPlayerCount
	{
		get => _deadPlayerCount;
		set
		{
			_deadPlayerCount = value;
			if(PlayerCount == DeadPlayerCount + WonPlayerCount)
			{
				Invoke("X_LoadMain", 3f);
			}
		}
	}

	public int WonPlayerCount
	{
		get => _wonPlayerCount;
		set
		{
			_wonPlayerCount = value;
			if (PlayerCount == DeadPlayerCount + WonPlayerCount)
			{
				Invoke("X_LoadMain", 3f);
			}
		}
	}

	public void SpawnMe(Transform toSpawn)
	{
		GameObject _randomSpawn = _spawns[Random.Range(0, _spawns.Count)];
		toSpawn.position = _randomSpawn.transform.position;
		_spawns.Remove(_randomSpawn);
		PlayerCount++;
	}

	private void X_LoadMain()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void X_Countdown()
	{
		_startText.text = $"START IN {--_countdown}";
		if (_countdown == 0)
		{
			GameStarted = true;
			CancelInvoke();
			_startText.gameObject.SetActive(false);
			UnityEngine.InputSystem.PlayerInputManager.instance.DisableJoining();
		}
	}

	protected override void ApplicationQuitInstance() { }

	protected override void AwakeInstance()
	{
		// Quick & Dirty & Extra Dirty
		_spawns = new List<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));
		var goals = new List<GameObject>(GameObject.FindGameObjectsWithTag("Finish"));
		while (goals.Count > 1)
		{
			int toRemove = Random.Range(0, goals.Count);
			Destroy(goals[toRemove]);
			goals.RemoveAt(toRemove);
		}
		// Countdown starten
		InvokeRepeating("X_Countdown", 1f, 1f);
	}

	protected override void DestroyInstance() { }

}
