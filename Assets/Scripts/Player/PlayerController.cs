using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Service<PlayerController.AxisUpdate>
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
		public Transform Target { get; private set; }
		public float X { get; private set; }
		public float Y { get; private set; }

		public AxisUpdate(float x, float y, Transform target)
		{
			Target = target;
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
	private Vector3 _cameraLook = Vector3.zero;

	#endregion

	#region Methods

	private void SetAnimation(bool isMoving)
	{
        if (isMoving)
        {
            _dwarfAnimator.SetBool("Walking", true);
        }
        else
            _dwarfAnimator.SetBool("Walking", false);
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
		_cameraLook = new Vector3(transform.position.x - data.X, 0f, transform.position.z - data.Y);
	}

	public AxisUpdate GetData() => new AxisUpdate(_lookInput.x, _lookInput.y, transform);

	#endregion

	#region Unity

	private void OnEnable()
	{
        _dwarfAnimator = GetComponent<Animator>();
        _inputModule = GetComponent<PlayerInput>();
		_playerIndex = (PlayerIndex)_inputModule.playerIndex;
		ServiceLocator<AxisUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
		// ggf. Rotieren
		if(_cameraLook != Vector3.zero && _playerIndex != PlayerIndex.Seeker)
		{
			transform.rotation = Quaternion.LookRotation(_cameraLook, Vector3.up);
		}
		// Bewegen
		transform.Translate(new Vector3(_moveInput.x, 0f, _moveInput.y) * Time.deltaTime * _moveSpeed, Space.Self);
	}

	private void OnDisable()
	{
		ServiceLocator<AxisUpdate, PlayerIndex>.WithdrawService(_playerIndex);
	}

	#endregion

	#endregion

}
