using UnityEngine;

public class ScanController : MonoBehaviour
{

	//private UIController _uiController;
	[SerializeField]private InputController _inputController;
	private Transform _cameraObject;
	private Transform _objectHit;
	private Camera _camera;
	private Animator _scannerAnimator;
	private Animator _officerAnimator;
	private Animator[] _swatAnimators;
	public GameObject _scanner;
	private Vector3 _startInput;
	private Vector3 _startRotationVector;
	private Vector3 _rotationVector;
	private Vector3 _cameraGoalRotation;
	private Vector3 _cameraGoalPosition;
	private Vector3 _cameraStartPosition;
	private float _startFieldOfView;
	private float _scanProgress;
	private float _smallFieldOfViewSpeed = 0.4f;
	private float _bigFieldOfViewSpeed = 0.75f;
	private float _goalFieldOfView;
	private bool _isCharacterSelected;
	private bool _isAiming;
	private bool _startingAiming;
	private bool _isActive;
	private bool _needToMoveCameraToCharacter;
	private bool _needToMoveCameraBack;
	private bool _isSWAT;
	private bool _isArrested;

	private void Start()
	{
		//_uiController = GetComponent<UIController>();
		//_scannerAnimator = _scanner.GetComponent<Animator>();
		//_camera = GameObject.FindGameObjectWithTag(TagManager.GetTag(TagType.CameraObject)).GetComponentInChildren<Camera>();
		_camera = Camera.main;
        _cameraObject = _camera.transform;
		_rotationVector = new Vector3();
		//_cameraObjectNew.localRotation = Quaternion.Euler(new Vector3(90f, 0, 0));
		_startFieldOfView = _camera.fieldOfView;
		_goalFieldOfView = _startFieldOfView;
		_cameraStartPosition = _cameraObject.transform.position;

		ActivateController();

    }
	public void ActivateController()
	{
		_isActive = true;
	}
	private void FixedUpdate()
	{
		if (_isActive)
		{
			if (_inputController.DragingStarted)
			{
				if (!_isAiming && !_startingAiming)
				{
					//_scannerAnimator.SetTrigger("StartAim");
					_startingAiming = true;
					Invoke(nameof(StartAiming), 0.4f);
				}
				else if (_isAiming && _startingAiming)
				{
					_rotationVector = _inputController.TouchPosition - _startInput;
					_cameraGoalRotation = _startRotationVector;


					_cameraGoalRotation.x -= _rotationVector.y;
					_cameraGoalRotation.y += _rotationVector.x;
					_cameraObject.localRotation = Quaternion.Euler(_cameraGoalRotation);
				}
				/*if (_cameraObject.localRotation.eulerAngles.y < 180f)
				{
					if (_cameraObject.localRotation.eulerAngles.y < 15f)
						_cameraGoalRotation.y -= _rotationVector.x;
					else
						_cameraGoalRotation = _cameraObject.localRotation.eulerAngles;
				}
				else if (_cameraObject.localRotation.eulerAngles.y > 180f)
				{
					if (_cameraObject.localRotation.eulerAngles.y > 345f)
						_cameraGoalRotation.y -= _rotationVector.x;
					else
						_cameraGoalRotation = _cameraObject.localRotation.eulerAngles;
				}*/
			}
			else
			{
				if (_isAiming && _startingAiming)
				{
					_scanner.SetActive(true);
					//_scannerAnimator.SetTrigger("StopAim");
					_startingAiming = false;
					_scanProgress = 0;
					//_uiController.OpenInGameUI();
					//_uiController.CloseScanUI();
					Invoke(nameof(StopAiming), 0.35f);
				}
			}
			if (_isAiming)
			{
				LookForCharacter();
				ChangeFieldOfView(_bigFieldOfViewSpeed);
				_goalFieldOfView = _startFieldOfView;
				//_uiController.SetSpinning(_scanProgress / 100f);
			}
			else
			{
				ChangeFieldOfView(_bigFieldOfViewSpeed);
			}
		}
		if (_needToMoveCameraBack)
		{
			MoveCameraBack();
		}
	}
	public void StartAiming()
	{
		_startInput = _inputController.TouchPosition;
		_startRotationVector = _cameraObject.localRotation.eulerAngles;
		_scanner.SetActive(false);
		_isAiming = true;
		//_uiController.CloseInGameUI();
		//_uiController.OpenScanUI();
	}
	public void StopAiming()
	{
		_isAiming = false;
	}
	private void LookForCharacter()
	{

	}
	private void MoveCameraBack()
	{
		_cameraObject.position = Vector3.MoveTowards(_cameraObject.position, _cameraStartPosition, 0.15f);
		if (_cameraObject.position == _cameraStartPosition)
		{
			_needToMoveCameraBack = false;
		}
	}
	private void MoveCameraToCharacter()
	{
		_cameraObject.position = Vector3.MoveTowards(_cameraObject.position, _cameraGoalPosition, 0.15f);
		if (_cameraObject.position == _cameraGoalPosition)
		{
			if (_isSWAT)
			{
				_needToMoveCameraToCharacter = false; for (int i = 0; i < _swatAnimators.Length; i++)
				{
					_swatAnimators[i].SetTrigger("SecureTarget");
				}
				Invoke(nameof(ShowUI), 1.5f);
				Invoke(nameof(HideUI), 3.5f);
			}
			else
			{
				_needToMoveCameraToCharacter = false;
				_officerAnimator.SetTrigger("Arrest");
				Invoke(nameof(ShowUI), 1.5f);
				Invoke(nameof(HideUI), 3.5f);
			}
		}
	}
	private void ShowUI()
	{
	}
	private void HideUI()
	{
		//_uiController.CloseFinalPanel();
	}
	private void ChangeFieldOfView(float speed)
	{
		if (_goalFieldOfView != _camera.fieldOfView)
		{
			if (_goalFieldOfView > _camera.fieldOfView)
			{
				if (_camera.fieldOfView + speed < _goalFieldOfView)
				{
					_camera.fieldOfView += speed;
				}
				else
				{
					_camera.fieldOfView = _goalFieldOfView;
				}
			}
			else if (_goalFieldOfView < _camera.fieldOfView)
			{
				if (_camera.fieldOfView - speed > _goalFieldOfView)
				{
					_camera.fieldOfView -= speed;
				}
				else
				{
					_camera.fieldOfView = _goalFieldOfView;
				}
			}
		}
	}
}