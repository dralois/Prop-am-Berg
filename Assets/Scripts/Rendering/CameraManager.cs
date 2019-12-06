using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	private InputSystem _controls;
	private Vector2 _lookDelta;

	private void Awake() => _controls = new InputSystem();

	private void OnEnable()
	{
		// Control Scheme aktivieren
		_controls.Ingame.Enable();
		// Delegate zuweisen
		CinemachineCore.GetInputAxis = GetAxisCustom;
	}
	private void OnDisable()
	{
		// Control Scheme deaktivieren
		_controls.Ingame.Disable();
		// Delegate entfernen
		CinemachineCore.GetInputAxis = null;
	}

	public float GetAxisCustom(string axisName)
	{
		// Delta holen
		_lookDelta = _controls.Ingame.Look.ReadValue<Vector2>();
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
