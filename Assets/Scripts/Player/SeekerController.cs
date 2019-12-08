using UnityEngine;
using UnityEngine.InputSystem;

public class SeekerController : MonoBehaviour, Service<PropController.AxisUpdate>, Service<PropController.Target>, SeekerControls.IIngameActions
{

	#region Fields

	[SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private LayerMask _viewLayer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int _energyDrain = 1;
    [SerializeField] private Transform _rechargeStation;
    [SerializeField] private Canvas _droneUi;
    [SerializeField] private ParticleSystem  _laserBeam;

    private PropController.PlayerIndex _playerIndex = PropController.PlayerIndex.None;
	private PlayerInput _inputModule = null;
	private Rigidbody _playerRB = null;
	private SeekerControls _controls = null;
    public int _energy = 0;
    private bool _recharging = true;
    private Animator _uiAnimator;

    private Vector2 _moveInput = Vector2.zero;
	private Vector2 _lookInput = Vector2.zero;
	private Quaternion _cameraLook = Quaternion.identity;
    private Camera _mainCam;

	#endregion

	#region Methods

	#region Input

	public void OnLook(InputAction.CallbackContext ctx)
	{
		_lookInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		_lookInput.Normalize();
	}

	public void OnMove(InputAction.CallbackContext ctx)
	{
		_moveInput = ctx.performed ? ctx.ReadValue<Vector2>() : Vector2.zero;
		_moveInput.Normalize();
    }

	public void OnShowProps(InputAction.CallbackContext ctx)
	{
		throw new System.NotImplementedException();
	}

	public void OnShoot(InputAction.CallbackContext ctx)
	{
        if (ctx.ReadValue<float>() > 0 && ctx.performed)
        {
            if ( _energy > 10 && !_recharging)
            {
                _laserBeam.Play();
                AudioManager.Instance.Play(AudioManager.AudioType.Sound, _audioClip, false, true, false);
                _energy = 70;
                RaycastHit hit;
                if(Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out hit, Mathf.Infinity, _viewLayer))
                {
                    if (hit.transform.CompareTag("Player")){
                        Debug.Log("Player got hit");
                        //hit.transform.gameObject.GetComponent<PropController>().KillPlayer();
                    }
                }
            }
        }
	}

	#endregion

	#region Overrides

	public void SetData(PropController.AxisUpdate data)
	{
		_cameraLook = Quaternion.Euler(0f, data.X, 0f);
	}

	public void SetData(PropController.Target data) { }

	public PropController.AxisUpdate GetData() => new PropController.AxisUpdate(_lookInput.x, _lookInput.y);

	PropController.Target Service<PropController.Target>.GetData() => new PropController.Target(transform, transform);

	#endregion

	#region Unity

	private void Start()
	{
        // Cachen
        _uiAnimator = _droneUi.GetComponentInChildren<Animator>();
        _mainCam = Camera.main;
		_playerRB = GetComponent<Rigidbody>();
		_inputModule = GetComponent<PlayerInput>();
		// Control Callbacks setzen
		_controls = new SeekerControls();
		_controls.Ingame.SetCallbacks(this);
		_controls.Ingame.Enable();
		// Index updaten
		_playerIndex = (PropController.PlayerIndex)_inputModule.playerIndex;
		// Service anbieten
		ServiceLocator<PropController.AxisUpdate, PropController.PlayerIndex>.ProvideService(this, _playerIndex);
		ServiceLocator<PropController.Target, PropController.PlayerIndex>.ProvideService(this, _playerIndex);
	}

	private void Update()
	{
        _droneUi.GetComponentInChildren<UnityEngine.UI.Text>().text =(_energy/36).ToString()+"%" ;
        if (_energy < 10)
        {
            _recharging = true;
            _uiAnimator.SetBool("recharging", true);
        }
        if(_energy >= 3600)
        {
            _recharging = false;
            _uiAnimator.SetBool("recharging", false);
        }
		Vector3 posNew;
        if (_recharging)
        {
            if (Vector3.Distance(_rechargeStation.position , transform.position)>= 1f)
            {
                posNew = Vector3.Lerp(_rechargeStation.position, transform.position, 1-Time.deltaTime);
            }
            else
            {
                posNew = transform.position;
            }
        }
        else
        {
            // Rotieren
            _playerRB.MoveRotation(_cameraLook);
            // Position updaten
            posNew = transform.TransformPoint(new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveInput.magnitude * Time.deltaTime * _moveSpeed);
            if(posNew != transform.position)
            {
                _energy -= _energyDrain;
            }
        }
        // Bewegen
        _playerRB.MovePosition(posNew);
    }

	private void OnDestroy()
	{
		// Controls entfernen
		_controls.Disable();
		_controls.Dispose();
		// Service entfernen
		ServiceLocator<PropController.AxisUpdate, PropController.PlayerIndex>.WithdrawService(_playerIndex);
		ServiceLocator<PropController.Target, PropController.PlayerIndex>.WithdrawService(_playerIndex);
	}
    private void FixedUpdate()
    {
        if (Vector3.Distance(_rechargeStation.position, transform.position) <= 1f)
        {
            if (_energy < 3600)
            {
                _energy += 10;
            }
        }
        else
        {
            if(_energy > 0)
            {
                _energy -= 1;
            }
        }
    }

    #endregion

    #endregion

}
