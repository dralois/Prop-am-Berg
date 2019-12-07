using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	[SerializeField] private PlayerController.PlayerIndex _boundPlayer = PlayerController.PlayerIndex.None;

	private Service<PlayerController.AxisUpdate> _targetService = null;
	private CinemachineFreeLook _freeLookComp = null;

	private void Awake()
	{
		// Free Look Component cachen
		_freeLookComp = GetComponent<CinemachineFreeLook>();
	}

	private void OnEnable()
	{
		// Service holen
		_targetService = ServiceLocator<PlayerController.AxisUpdate, PlayerController.PlayerIndex>.GetService(_boundPlayer);
	}

	private void OnDisable()
	{
		// Service entfernen
		_targetService = null;
	}

	private void Update()
	{
		// Free Look updaten
		_freeLookComp.m_XAxis.m_InputAxisValue = _targetService.GetData().X;
		_freeLookComp.m_YAxis.m_InputAxisValue = _targetService.GetData().Y;
		_freeLookComp.m_XAxis.Update(Time.deltaTime);
		_freeLookComp.m_YAxis.Update(Time.deltaTime);
		// Kamera Position zurückschreiben
		_targetService.SetData(new PlayerController.AxisUpdate(transform.position.x, transform.position.z));
	}

}
