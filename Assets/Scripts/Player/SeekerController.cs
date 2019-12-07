using UnityEngine;
using UnityEngine.InputSystem;

public class SeekerController : MonoBehaviour, Service<PropController.AxisUpdate>, Service<PropController.Target>
{

	#region Fields

	[SerializeField] private float _moveSpeed = 5f;

	private PropController.PlayerIndex _playerIndex = PropController.PlayerIndex.None;
	private PlayerInput _inputModule = null;
	private Rigidbody _playerRB = null;

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;

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

	public void SetData(PropController.AxisUpdate data)
	{
		_cameraLook = Quaternion.Euler(0f, data.X, 0f);
	}

	public void SetData(PropController.Target data) { }

	public PropController.AxisUpdate GetData() => new PropController.AxisUpdate(_lookInput.x, _lookInput.y);

	PropController.Target Service<PropController.Target>.GetData() => new PropController.Target(transform);

	#endregion

	#region Unity

	private void Start()
	{
		// Cachen
		_playerRB = GetComponent<Rigidbody>();
		_inputModule = GetComponent<PlayerInput>();
		// Index updaten
		_playerIndex = (PropController.PlayerIndex)_inputModule.playerIndex;
		// Service anbieten
		ServiceLocator<PropController.AxisUpdate, PropController.PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<PropController.Target, PropController.PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
		Vector3 posNew;
		// Rotieren
		_playerRB.MoveRotation(_cameraLook);
		// Position updaten
		posNew = transform.TransformPoint(new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveInput.magnitude * Time.deltaTime * _moveSpeed);
		// Bewegen
		_playerRB.MovePosition(posNew);
	}

	private void OnDestroy()
	{
		// Service entfernen
		ServiceLocator<PropController.AxisUpdate, PropController.PlayerIndex>.WithdrawService(_playerIndex);
		ServiceLocator<PropController.Target, PropController.PlayerIndex>.WithdrawService(_playerIndex);
	}

	#endregion

	#endregion

}
