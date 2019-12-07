using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PropController : MonoBehaviour, Service<PropController.AxisUpdate>, Service<PropController.Target>, Service<PropController.GuiUpdate>
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
	public struct GuiUpdate
	{
		public bool _buttonPressed { get; private set; }

		public float X { get; private set; }
		public float Y { get; private set; }
		public Sprite[] _spriteArray { get; private set; }

		public GuiUpdate(float x, float y, bool pressed, Sprite[] s)
		{
			X = x;
			Y = y;
			_buttonPressed = pressed;
			_spriteArray = s;
		}

	}
	#endregion

	#region Fields

	[SerializeField] private float _moveSpeed = 5f;
	[SerializeField] private ScruptableObjects[] _propList;
	[SerializeField] private GameObject _model;


	private ScruptableObjects[] _availProps;
	private GameObject[] _props;
	private Sprite[] _icons;
	private int _currentProp;

	private PlayerIndex _playerIndex = PlayerIndex.None;
	private PlayerInput _inputModule = null;
	private Animator _dwarfAnimator = null;
	private Rigidbody _playerRB = null;

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;
	private bool _switchPressed = false;

	private bool _inAir = false;

	#endregion

	#region Methods

	private void X_SetAnimation(bool isMoving)
	{
		if (_playerIndex != PlayerIndex.Seeker)
		{
			if (isMoving)
			{
				_dwarfAnimator.SetBool("Walking", true);
			}
			else
				_dwarfAnimator.SetBool("Walking", false);
		}
	}

	private void X_SetProps()
	{
		int _propCount = _propList.Length;
		List<int> _availIndex = new List<int>();
		for (int i = 0; i < _propCount; i++)
		{
			_availIndex.Add(i);
		}
		if (_propCount > 0)
		{
			_availProps = new ScruptableObjects[_propCount > 3 ? 4 : _propCount];
			_props = new GameObject[_propCount > 3 ? 4 : _propCount];
			_icons = new Sprite[_propCount > 3 ? 4 : _propCount];

			for (int j = 0; j < _availProps.Length; j++)
			{
				int _currentIndex = Random.Range(0, _availIndex.Count);
				_availProps[j] = _propList[_currentIndex];
				_availIndex.Remove(_currentIndex);
			}
		}

		// Instanziieren der Props als Childs
		for (int k = 0; k < _availProps.Length; k++)
		{
			_props[k] = Instantiate(_availProps[k]._prop, transform);
			_icons[k] = _availProps[k]._img;
		}
	}

	private void X_Jump()
	{
		_playerRB.AddForce(Vector3.up * 10f, ForceMode.Impulse);
	}

	#region Input

	public void SwitchProp(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			if (ctx.ReadValue<float>() > 0)
			{
				_switchPressed = true;
			}
			else
			{
				_switchPressed = false;
			}
		}
		else
		{
			_switchPressed = false;
		}
	}

	public void SwitchToProp(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			if (ctx.ReadValue<float>() > 0)
			{
				_model.SetActive(false);
				_props[_currentProp].SetActive(true);
			}
		}
		else
		{
			_model.SetActive(true);
			_props[_currentProp].SetActive(false);

		}
	}

	public void JumpAction(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && ctx.ReadValue<float>() > 0f &&
			!_inAir && !IsInvoking("X_Jump"))
		{
			Invoke("X_Jump", .5f);
			_dwarfAnimator.SetTrigger("Jump");
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

	#endregion

	#region Overrides

	public void SetData(AxisUpdate data)
	{
		// Rotationen bestimmen
		var camRot = Quaternion.Euler(0f, data.X, 0f);
		var moveRot = Quaternion.LookRotation(new Vector3(_moveInput.x, 0f, _moveInput.y));
		// ggf. speichern
		_cameraLook = _moveInput.magnitude > 0f ? camRot * moveRot : _cameraLook;
	}

	public void SetData(Target data) { }

	public AxisUpdate GetData() => new AxisUpdate(_lookInput.x, _lookInput.y);

	Target Service<Target>.GetData() => new Target(transform);

	public void SetData(GuiUpdate data)
	{
		throw new System.NotImplementedException();
	}

	GuiUpdate Service<GuiUpdate>.GetData() => new GuiUpdate(_moveInput.x, _moveInput.y, _switchPressed, _icons);

	#endregion

	#region Unity

	private void Start()
	{
		// Cachen
		_playerRB = GetComponent<Rigidbody>();
		_inputModule = GetComponent<PlayerInput>();
		_dwarfAnimator = GetComponentInChildren<Animator>();
		// Index updaten
		_playerIndex = (PlayerIndex)_inputModule.playerIndex;
		// zufällige Zuweisung von Props
		X_SetProps();
		// Service anbieten
		ServiceLocator<AxisUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<Target, PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<GuiUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
		Vector3 posNew;
		// Rotieren
		_playerRB.MoveRotation(_cameraLook);
		// Position updaten
		posNew = transform.TransformPoint(Vector3.forward * _moveInput.magnitude * Time.deltaTime * _moveSpeed);
		// Bewegen
		_playerRB.MovePosition(posNew);
	}

	private void FixedUpdate()
	{
		// Prüfen ob Player fällt
		if (!Physics.Linecast(transform.position + new Vector3(0f, 1f, 0f),
													transform.position - new Vector3(0, 2f, 0)))
		{
			_inAir = true;
		}
		else
		{
			_inAir = false;
		}
		// Flag setzen
		_dwarfAnimator.SetBool("InAir", _inAir);
	}

	private void OnDestroy()
	{
		// Service entfernen
		ServiceLocator<AxisUpdate, PlayerIndex>.WithdrawService(_playerIndex);
		ServiceLocator<Target, PlayerIndex>.WithdrawService(_playerIndex);
		ServiceLocator<GuiUpdate, PlayerIndex>.WithdrawService(_playerIndex);
	}



	#endregion

	#endregion

}
