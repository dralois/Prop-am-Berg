using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Service<PlayerController.AxisUpdate>
{

	#region Enums

	public enum PlayerIndex : int
	{
		None = -1,
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

	#endregion

	#region Fields

	[SerializeField] private PlayerIndex _playerIndex = PlayerIndex.None;

	private PlayerInput _inputModule = null;
	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;

	private Vector3 _cameraLook = Vector3.zero;

	#endregion

	#region Methods

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
		_cameraLook = new Vector3(transform.position.x - data.X, 0f, transform.position.z - data.Y);
	}

	public AxisUpdate GetData() => new AxisUpdate(_lookInput.x, _lookInput.y);

	#endregion

	#region Unity

	private void OnEnable()
	{
		_inputModule = GetComponent<PlayerInput>();
		_playerIndex = (PlayerIndex)_inputModule.user.index;
		ServiceLocator<AxisUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
		transform.rotation = Quaternion.LookRotation(_cameraLook, Vector3.up);
		transform.Translate(new Vector3(_moveInput.x, 0f, _moveInput.y) * Time.deltaTime * 5f, Space.Self);
	}

	private void OnDisable()
	{
		ServiceLocator<AxisUpdate, PlayerIndex>.WithdrawService(_playerIndex);
	}

	#endregion

	#endregion

}
