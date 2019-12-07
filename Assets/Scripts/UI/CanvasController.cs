using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
	[SerializeField] PropController.PlayerIndex _playerIndex;
	[SerializeField] GameObject _disableObj = null;
	[SerializeField] GameObject _joinObj = null;
	[SerializeField] GameObject _selectionObj = null;
	[SerializeField] TextMeshProUGUI _winObj = null;
	[SerializeField] GameObject _loseObj = null;

	Service<PropController.GuiUpdate> _GUIService;
	private Image[] _images = null;
	private bool _startedChecked = false;
	private bool _winLoseChecked = false;

	static int _winCount = 0;

	void Start()
	{
		_winCount = 0;
		_images = _selectionObj.GetComponentsInChildren<Image>(true);
		ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.OnServiceAdded += X_PlayerJoined;
	}

	void Update()
	{
		if (GameManager.Instance.GameStarted && !_startedChecked)
		{
			_joinObj.SetActive(false);
			_startedChecked = true;
		}
		if (_GUIService != null)
		{
			PropController.GuiUpdate data = _GUIService.GetData();
			if(data.DidLose || data.DidWin)
			{
				if (_winLoseChecked)
					return;
				_winCount += data.DidWin ? 1 : 0;
				_winObj.text = $"YOU CAME IN {_winCount}!";
				_winObj.transform.parent.gameObject.SetActive(data.DidWin);
				_loseObj.SetActive(data.DidLose);
				_winLoseChecked = true;
			}
			else if (data.ButtonPressed)
			{
				_selectionObj.SetActive(true);
			}
			else
			{
				_selectionObj.SetActive(false);
			}
		}
	}

	private void X_PlayerJoined(PropController.PlayerIndex index)
	{
		if (index == _playerIndex)
		{
			_disableObj.SetActive(false);
			_joinObj.SetActive(false);
			_GUIService = ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.GetService(_playerIndex);
			for (int i = 0; i < _GUIService.GetData().Icons.Length; i++)
			{
				_images[i].sprite = _GUIService.GetData().Icons[i];
			}
		}

	}
}
