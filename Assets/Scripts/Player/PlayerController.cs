﻿using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Service<PlayerController.AxisUpdate>, Service<PlayerController.Target>
{

	#region Enums

	public enum PlayerIndex : int
	{
		None = -1,
		Seeker,
		One,
		Two,
		Three,
		Four
	}

	#endregion

	#region Structs

	public struct AxisUpdate
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		public AxisUpdate(float x, float y)
		{
			X = x;
			Y = y;
		}
	}

	public struct Target
	{
		public Transform Player { get; private set; }

		public Target(Transform target)
		{
			Player = target;
		}
	}

	#endregion

	#region Fields

	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private Animator _dwarfAnimator;

	private PlayerIndex _playerIndex = PlayerIndex.None;
	private PlayerInput _inputModule = null;

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;

	#endregion

	#region Methods

	private void X_SetAnimation(bool isMoving)
	{
		if(_playerIndex != PlayerIndex.Seeker)
		{
			if (isMoving)
			{
				_dwarfAnimator.SetBool("Walking", true);
			}
			else
				_dwarfAnimator.SetBool("Walking", false);
		}
	}

	public void LookAction(InputAction.CallbackContext ctx)
	{
		_lookInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		_lookInput.Normalize();
	}

	public void MoveAction(InputAction.CallbackContext ctx)
	{
		_moveInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		X_SetAnimation(ctx.performed);
		_moveInput.Normalize();
	}

	#region Overrides

	public void SetData(AxisUpdate data)
	{
		// Rotation je nach dem updaten
		if (_playerIndex == PlayerIndex.Seeker)
		{
			_cameraLook = Quaternion.Euler(0f, data.X, 0f);
		}
		else
		{
			// Rotationen bestimmen
			var camRot = Quaternion.Euler(0f, data.X, 0f);
			var moveRot = Quaternion.LookRotation(new Vector3(_moveInput.x, 0f, _moveInput.y));
			// ggf. speichern
			_cameraLook = _moveInput.magnitude > 0f ? camRot * moveRot : _cameraLook;
		}
	}

	public void SetData(Target data) { }

	public AxisUpdate GetData() => new AxisUpdate(_lookInput.x, _lookInput.y);

	Target Service<Target>.GetData() => new Target(transform);

	#endregion

	#region Unity

	private void Start()
	{
		_dwarfAnimator = GetComponentInChildren<Animator>();
		// Index updaten und cachen
		_inputModule = GetComponent<PlayerInput>();
		_playerIndex = (PlayerIndex)_inputModule.playerIndex;
		// Service anbieten
		ServiceLocator<AxisUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<Target, PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
		// Rotieren
		transform.rotation = _cameraLook;
		// Bewegen
		if(_playerIndex == PlayerIndex.Seeker)
		{
			transform.Translate(new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveInput.magnitude * Time.deltaTime * _moveSpeed, Space.Self);
		}
		else
		{
			transform.Translate(Vector3.forward * _moveInput.magnitude * Time.deltaTime * _moveSpeed, Space.Self);
		}
	}

	private void OnDestroy()
	{
		// Service entfernen
		ServiceLocator<AxisUpdate, PlayerIndex>.WithdrawService(_playerIndex);
		ServiceLocator<Target, PlayerIndex>.WithdrawService(_playerIndex);
	}

	#endregion

	#endregion

}
