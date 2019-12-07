using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    [SerializeField] private ScruptableObjects[] _propList;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject[] _props;
    [SerializeField] private Sprite[] _icons;

    private Animator _dwarfAnimator;

    private int _currentProp;
    private ScruptableObjects[] _availProps;

    private PlayerIndex _playerIndex = PlayerIndex.None;
	private PlayerInput _inputModule = null;
	private Animator _dwarfAnimator = null;
	private Rigidbody _playerRB = null;

	private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;

	private bool _inAir = false;

	#endregion

	#region Methods

	private void X_CheckGround()
	{
		if(_playerIndex != PlayerIndex.Seeker)
		{
			// Prüfen ob Player fällt
			if (Physics.Raycast(_playerRB.position, Vector3.down))
			{
				_inAir = false;
			}
			else
			{
				_inAir = true;
			}
			// Flag setzen
			_dwarfAnimator.SetBool("InAir", _inAir);
		}
	}

    public void SwitchProp(InputAction.CallbackContext xdg)
    {
        if (xdg.performed)
        {
            //xdg.ReadValue<>
        }
    }
    public void SwitchToProp(InputAction.CallbackContext xdg)
    {
        if (xdg.performed)
        {
           if(xdg.ReadValue<float>() > 0)
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

    public void SetProps()
    {
        int _propCount = _propList.Length;
        List<int> _availIndex = new List<int>();
        for (int i = 0; i < _propCount; i++)
        {
            _availIndex.Add(i);
        }
        if (_propCount > 0)
        {
            if (_propCount > 3)
            {
                _availProps = new ScruptableObjects[4];
            }
            else
            {
                _availProps = new ScruptableObjects[_propCount];
                _props = new GameObject[_propCount];
                _icons = new Sprite[_propCount];
            }
            for (int j = 0; j < _availProps.Length; j++)
            {
                int _currentIndex = Random.Range(0, _availIndex.Count + 1);
                _availProps[j] = _propList[_currentIndex];
                _availIndex.Remove(_currentIndex);
            }
        }

        // Instanziieren der Props als Childs
        for (int k = 0; k < _availProps.Length; k++)
        {
            _props[k] = Instantiate(_availProps[k]._prop, transform.position, Quaternion.identity, transform);
            _icons[k] = _availProps[k]._img;
        }
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
		// Cachen
		_playerRB = GetComponent<Rigidbody>();
		_inputModule = GetComponent<PlayerInput>();
		_dwarfAnimator = GetComponentInChildren<Animator>();
		// Index updaten
		_playerIndex = (PlayerIndex)_inputModule.playerIndex;
		// Service anbieten
		ServiceLocator<AxisUpdate, PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<Target, PlayerIndex>.ProvideService(this, _playerIndex);
        // zufällige Zuweisung von Props
        SetProps();



    }

    private void Update()
	{
		Vector3 posNew;
		// Rotieren
		_playerRB.MoveRotation(_cameraLook);
		// Position updaten
		if(_playerIndex == PlayerIndex.Seeker)
		{
			posNew = transform.TransformPoint(new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveInput.magnitude * Time.deltaTime * _moveSpeed);
		}
		else
		{
			posNew = transform.TransformPoint(Vector3.forward * _moveInput.magnitude * Time.deltaTime * _moveSpeed);
		}
		// Bewegen
		_playerRB.MovePosition(posNew);
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
