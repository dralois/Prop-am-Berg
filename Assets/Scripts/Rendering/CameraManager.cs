using Cinemachine;
using UnityEngine;
using Target = PlayerController.Target;
using AxisUpdate = PlayerController.AxisUpdate;
using PlayerIndex = PlayerController.PlayerIndex;

public class CameraManager : MonoBehaviour
{

	#region Fields

	[SerializeField] private PlayerIndex _boundPlayer = PlayerIndex.None;

	private Service<AxisUpdate> _axisService = null;
	private CinemachineFreeLook _freeLookComp = null;
	private CinemachineVirtualCamera _seekerCam = null;
	private CinemachinePOV _seekerPOV = null;
	private bool _isActive = false;

	#endregion

	#region Methods

	private void X_AxisRemoved(PlayerIndex index)
	{
		if (index == _boundPlayer)
		{
			_axisService = null;
		}
	}

	private void X_AxisAdded(PlayerIndex index)
	{
		if (index == _boundPlayer)
		{
			_axisService = ServiceLocator<AxisUpdate, PlayerIndex>.GetService(_boundPlayer);
		}
	}

	private void X_TargetAdded(PlayerIndex index)
	{
		if(index == _boundPlayer)
		{
			// Service holen
			var targetService = ServiceLocator<Target, PlayerIndex>.GetService(_boundPlayer);
			// Target setzen
			if(_boundPlayer == PlayerIndex.Seeker)
			{
				_seekerCam.Follow = targetService.GetData().Player;
				_seekerCam.LookAt = targetService.GetData().Player;
			}
			else
			{
				_freeLookComp.Follow = targetService.GetData().Player;
				_freeLookComp.LookAt = targetService.GetData().Player;
			}
			// Kamera ist aktiv
			_isActive = true;
		}
	}

	private void X_TargetRemoved(PlayerIndex index)
	{
		if (index == _boundPlayer)
		{
			// Kamera nicht mehr aktiv
			_isActive = false;
			// Target entfernen
			if (_boundPlayer == PlayerIndex.Seeker)
			{
				_seekerCam.Follow = null;
				_seekerCam.LookAt = null;
			}
			else
			{
				_freeLookComp.Follow = null;
				_freeLookComp.LookAt = null;
			}
		}
	}

	#region Unity

	private void Awake()
	{
		if(_boundPlayer == PlayerIndex.Seeker)
		{
			// POV Kamera Components cachen
			_seekerCam = GetComponent<CinemachineVirtualCamera>();
			_seekerPOV = (CinemachinePOV)_seekerCam.GetCinemachineComponent(CinemachineCore.Stage.Aim);
		}
		else
		{
			// Free Look Component cachen
			_freeLookComp = GetComponent<CinemachineFreeLook>();
		}
		// Events abonnieren
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceAdded += X_AxisAdded;
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceRemoved += X_AxisRemoved;
		ServiceLocator<Target, PlayerIndex>.OnServiceAdded += X_TargetAdded;
		ServiceLocator<Target, PlayerIndex>.OnServiceRemoved += X_TargetRemoved;
	}

	private void OnDestroy()
	{
		// Events entfernen
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceAdded -= X_AxisAdded;
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceRemoved -= X_AxisRemoved;
		ServiceLocator<Target, PlayerIndex>.OnServiceAdded -= X_TargetAdded;
		ServiceLocator<Target, PlayerIndex>.OnServiceRemoved -= X_TargetRemoved;
	}

	private void Update()
	{
		if (_isActive)
		{
			if(_boundPlayer == PlayerIndex.Seeker)
			{
				// POV updaten
				_seekerPOV.m_HorizontalAxis.m_InputAxisValue = _axisService.GetData().X;
				_seekerPOV.m_VerticalAxis.m_InputAxisValue = _axisService.GetData().Y;
				_seekerPOV.m_HorizontalAxis.Update(Time.deltaTime);
				_seekerPOV.m_VerticalAxis.Update(Time.deltaTime);
				// Kamera Rotation zurückschreiben
				_axisService.SetData(new AxisUpdate(transform.rotation.eulerAngles.y, 0f));
			}
			else
			{
				// Free Look updaten
				_freeLookComp.m_XAxis.m_InputAxisValue = _axisService.GetData().X;
				_freeLookComp.m_YAxis.m_InputAxisValue = _axisService.GetData().Y;
				_freeLookComp.m_XAxis.Update(Time.deltaTime);
				_freeLookComp.m_YAxis.Update(Time.deltaTime);
				// Kamera Position zurückschreiben
				_axisService.SetData(new AxisUpdate(transform.position.x, transform.position.z));
			}
		}
	}

	#endregion

	#endregion

}
