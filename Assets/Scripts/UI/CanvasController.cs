using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
	[SerializeField] PropController.PlayerIndex _playerIndex;

	Service<PropController.GuiUpdate> _GUIService;
	private Image[] _images = null;
	private Canvas _canvas = null;

	void Start()
	{
		_images = GetComponentsInChildren<Image>();
		_canvas = GetComponent<Canvas>();
		_canvas.enabled = false;
		ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.OnServiceAdded += AddService;
	}

	void Update()
	{
		if (_GUIService != null)
		{
			if (_GUIService.GetData()._buttonPressed)
			{
				_canvas.enabled = true;
			}
			else
			{
				_canvas.enabled = false;
			}
		}
	}

	void AddService(PropController.PlayerIndex index)
	{
		if (index == _playerIndex)
		{
			_GUIService = ServiceLocator<PropController.GuiUpdate, PropController.PlayerIndex>.GetService(_playerIndex);
			for (int i = 0; i < _GUIService.GetData()._spriteArray.Length; i++)
			{
				_images[i].sprite = _GUIService.GetData()._spriteArray[i];
				_images[i].enabled = true;
			}
		}

	}
}
