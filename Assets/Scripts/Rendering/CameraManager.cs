using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	private InputSystem _defaultcontrols;
	private Vector2 _lookDelta;

	private void Awake() => _defaultcontrols = new InputSystem();

	private void OnEnable()
	{
		// Control Scheme aktivieren
		_defaultcontrols.Ingame.Enable();
		// Delegate zuweisen
		CinemachineCore.GetInputAxis = GetAxisCustom;
	}
	private void OnDisable()
	{
		// Control Scheme deaktivieren
		_defaultcontrols.Ingame.Disable();
		// Delegate entfernen
		CinemachineCore.GetInputAxis = null;
	}

	public float GetAxisCustom(string axisName)
	{
		// Delta holen
		_lookDelta = _defaultcontrols.Ingame.Look.ReadValue<Vector2>();
		_lookDelta.Normalize();
		// Delta je nach Achse zurückgeben
		if (axisName == "X")
		{
			return _lookDelta.x;
		}
		else if (axisName == "Y")
		{
			return _lookDelta.y;
		}
		else
		{
			return 0;
		}
	}

}
