using Cinemachine;
using System;
using UnityEngine;
using AxisUpdate = PlayerController.AxisUpdate;
using PlayerIndex = PlayerController.PlayerIndex;

public class CameraManager : MonoBehaviour
{

	[SerializeField] private PlayerIndex _boundPlayer = PlayerIndex.None;

	private Service<AxisUpdate> _targetService = null;
	private CinemachineFreeLook _freeLookComp = null;
	private CinemachineVirtualCamera _seekerCam = null;
	private CinemachinePOV _seekerPOV = null;
	private bool _isActive = false;

	private void Awake()
	{
		if(_boundPlayer == PlayerIndex.Seeker)
		{
			// POV Cam Components cachen
			_seekerCam = GetComponent<CinemachineVirtualCamera>();
			_seekerPOV = (CinemachinePOV)_seekerCam.GetCinemachineComponent(CinemachineCore.Stage.Aim);
			// Service hinzufügen
			_targetService = ServiceLocator<AxisUpdate, PlayerIndex>.GetService(_boundPlayer);
			_seekerCam.Follow = _targetService.GetData().Target;
			_seekerCam.LookAt = _seekerCam.Follow;
			_isActive = true;
		}
		else
		{
			// Free Look Component cachen
			_freeLookComp = GetComponent<CinemachineFreeLook>();
			// Events abonnieren
			ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceAdded += X_ServiceAdded;
			ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceRemoved += X_ServiceRemoved;
		}
	}

	private void X_ServiceRemoved(PlayerIndex index)
	{
		if (index == _boundPlayer)
		{
			_isActive = false;
			_targetService = null;
		}
	}

	private void X_ServiceAdded(PlayerIndex index)
	{
		if(index == _boundPlayer && _boundPlayer != PlayerIndex.Seeker)
		{
			_targetService = ServiceLocator<AxisUpdate, PlayerIndex>.GetService(_boundPlayer);
			_freeLookComp.Follow = _targetService.GetData().Target;
			_freeLookComp.LookAt = _freeLookComp.Follow;
			_isActive = true;
		}
	}

	private void OnDestroy()
	{
		// Events entfernen
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceAdded -= X_ServiceAdded;
		ServiceLocator<AxisUpdate, PlayerIndex>.OnServiceRemoved -= X_ServiceRemoved;
	}

	private void Update()
	{
		if (_isActive)
		{
			if(_boundPlayer == PlayerIndex.Seeker)
			{
				_seekerPOV.m_HorizontalAxis.m_InputAxisValue = _targetService.GetData().X;
				_seekerPOV.m_VerticalAxis.m_InputAxisValue = _targetService.GetData().Y;
				_seekerPOV.m_HorizontalAxis.Update(Time.deltaTime);
				_seekerPOV.m_VerticalAxis.Update(Time.deltaTime);
			}
			else
			{
				// Free Look updaten
				_freeLookComp.m_XAxis.m_InputAxisValue = _targetService.GetData().X;
				_freeLookComp.m_YAxis.m_InputAxisValue = _targetService.GetData().Y;
				_freeLookComp.m_XAxis.Update(Time.deltaTime);
				_freeLookComp.m_YAxis.Update(Time.deltaTime);
				// Kamera Position zurückschreiben
				_targetService.SetData(new AxisUpdate(transform.position.x, transform.position.z, transform));
			}
		}
	}

}
