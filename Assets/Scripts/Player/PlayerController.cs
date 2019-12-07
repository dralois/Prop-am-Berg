using UnityEngine;
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

	private PlayerIndex _playerIndex = PlayerIndex.None;
	private PlayerInput _inputModule = null;

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;

	#endregion

	#region Methods

	private void SetAnimation(bool isMoving)
	{

	}

	public void LookAction(InputAction.CallbackContext ctx)
	{
		_lookInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		_lookInput.Normalize();
	}

	public void MoveAction(InputAction.CallbackContext ctx)
	{
		_moveInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		_moveInput.Normalize();
	}

	#region Overrides

	public void SetData(AxisUpdate data)
	{
		// Rotation je nach dem updaten
		if(_playerIndex == PlayerIndex.Seeker)
		{
			_cameraLook = Quaternion.Euler(0f, data.X, 0f);
		}
		else
		{
			_cameraLook = Quaternion.LookRotation(new Vector3(transform.position.x - data.X,
																												0f,
																												transform.position.z - data.Y),
																						Vector3.up);
		}
	}

	public void SetData(Target data) { }

	public AxisUpdate GetData() => new AxisUpdate(_lookInput.x, _lookInput.y);

	Target Service<Target>.GetData() => new Target(transform);

	#endregion

	#region Unity

	private void Start()
	{
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
		transform.Translate(new Vector3(_moveInput.x, 0f, _moveInput.y) * Time.deltaTime * _moveSpeed, Space.Self);
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
