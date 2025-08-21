using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    #region UI
    public Camera activeCamera;

    Transform activeCameraTr
    {
        get { return activeCamera.transform; }
    }
    [Space]
    [SerializeField]
    [Tooltip("The script is currently active")]
    private bool _active = true;

    [Space]

    [SerializeField]
    [Tooltip("Camera rotation by mouse movement is active")]
    private bool _enableRotation = true;

    [SerializeField]
    [Tooltip("Sensitivity of mouse rotation")]
    private float _mouseSense = 1.8f;

    [Space]

    [SerializeField]
    [Tooltip("Camera zooming in/out by 'Mouse Scroll Wheel' is active")]
    private bool _enableTranslation = true;

    [SerializeField]
    [Tooltip("Velocity of camera zooming in/out")]
    private float _translationSpeed = 55f;

    [Space]

    [SerializeField]
    [Tooltip("Camera movement by 'W','A','S','D','Q','E' keys is active")]
    private bool _enableMovement = true;

    [SerializeField]
    [Tooltip("Camera movement speed")]
    private float _movementSpeed = 10f;

    [SerializeField]
    [Tooltip("Speed of the quick camera movement when holding the 'Left Shift' key")]
    private float _boostedSpeed = 50f;

    [SerializeField]
    [Tooltip("Boost speed")]
    private KeyCode _boostSpeed = KeyCode.LeftShift;

    [SerializeField]
    [Tooltip("Move up")]
    private KeyCode _moveUp = KeyCode.E;

    [SerializeField]
    [Tooltip("Move down")]
    private KeyCode _moveDown = KeyCode.Q;

    [Space]

    [SerializeField]
    [Tooltip("Acceleration at camera movement is active")]
    private bool _enableSpeedAcceleration = true;

    [SerializeField]
    [Tooltip("Rate which is applied during camera movement")]
    private float _speedAccelerationFactor = 1.5f;

    [Space]

    [SerializeField]
    [Tooltip("This keypress will move the camera to initialization position")]
    private KeyCode _initPositonButton = KeyCode.R;

    #endregion UI

    private CursorLockMode _wantedMode;

    private float _currentIncrease = 1;
    private float _currentIncreaseMem = 0;

    private Vector3 _initPosition;
    private Vector3 _initRotation;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_boostedSpeed < _movementSpeed)
            _boostedSpeed = _movementSpeed;
    }
#endif
    public void SetActiveCamera(Camera targetCamera)
    {
        activeCamera = targetCamera;
    }
    private void Start()
    {
        _initPosition = activeCameraTr.position;
        _initRotation = activeCameraTr.eulerAngles;
    }

    private void OnEnable()
    {
        if (_active)
            _wantedMode = CursorLockMode.Locked;
    }

    private void SetCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = _wantedMode = CursorLockMode.None;
        }

        Cursor.visible = true;
    }

    private void CalculateCurrentIncrease(bool moving)
    {
        _currentIncrease = Time.deltaTime;

        if (!_enableSpeedAcceleration || _enableSpeedAcceleration && !moving)
        {
            _currentIncreaseMem = 0;
            return;
        }

        _currentIncreaseMem += Time.deltaTime * (_speedAccelerationFactor - 1);
        _currentIncrease = Time.deltaTime + Mathf.Pow(_currentIncreaseMem, 3) * Time.deltaTime;
    }

    private void Movement()
    {

        if (_enableMovement)
        {
            Vector3 deltaPosition = Vector3.zero;
            float currentSpeed = _movementSpeed;

            if (Input.GetKey(_boostSpeed))
                currentSpeed = _boostedSpeed;

            if (Input.GetKey(KeyCode.W))
                deltaPosition += activeCameraTr.forward;

            if (Input.GetKey(KeyCode.S))
                deltaPosition -= activeCameraTr.forward;

            if (Input.GetKey(KeyCode.A))
                deltaPosition -= activeCameraTr.right;

            if (Input.GetKey(KeyCode.D))
                deltaPosition += activeCameraTr.right;

            if (Input.GetKey(_moveUp))
                deltaPosition += activeCameraTr.up;

            if (Input.GetKey(_moveDown))
                deltaPosition -= activeCameraTr.up;

            // Calc acceleration
            CalculateCurrentIncrease(deltaPosition != Vector3.zero);

            activeCameraTr.position += deltaPosition * currentSpeed * _currentIncrease;
        }

    }
    private void Translation()
    {
        if (_enableTranslation)
        {
            activeCameraTr.Translate(Vector3.forward * Input.mouseScrollDelta.y * Time.deltaTime * _translationSpeed);
        }

    }
    private void Rotation()
    {
        if (Input.GetMouseButtonDown(1))
        {

            _enableRotation = true;

        }
        if (Input.GetMouseButtonUp(1))
        {

            _enableRotation = false;
        }
        if (_enableRotation)
        {
            // Pitch
            activeCameraTr.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * _mouseSense, Vector3.right);
            // Paw
            Vector3 euler = activeCameraTr.eulerAngles;
            euler.y += Input.GetAxis("Mouse X") * _mouseSense;
            activeCameraTr.rotation = Quaternion.Euler(euler);

        }

    }
    private void Update()
    {
        if (!_active)
            return;
        if (activeCamera == null)
        {
            return;
        }
        SetCursorState();

        /*if (Cursor.visible)
            return;*/

        // Translation
        Translation();
        // Movement
        Movement();
        // Rotation
        Rotation();

        // Return to init position
        if (Input.GetKeyDown(_initPositonButton))
        {
            activeCameraTr.position = _initPosition;
            activeCameraTr.eulerAngles = _initRotation;
        }

    }

    public void CameraFocusOn(Transform targetTransform, float focustime = 0.5f)
    {
        if (activeCamera == null) { return; }
        Transform camera = activeCameraTr;
        Quaternion cameraRoation = camera.transform.rotation;
        Vector3 cameraPos = camera.transform.position;
        //目标观察点
        Vector3 viewPotin = targetTransform.position;
        //得到屏幕中心在世界坐标的位置
        Vector2 screnCenter = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
        //把这个点转换为世界坐标
        Ray ray = activeCamera.ScreenPointToRay(screnCenter);
        Vector3 getCenterPos = Vector3.zero;
        if (Physics.Raycast(ray, out var distance))
        {
            getCenterPos = ray.GetPoint(distance.distance);
        }
        else
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var enter))
            {
                getCenterPos = ray.GetPoint(enter);
            }

        }
        //得到了点之后，计算中心点和相机位置的ZX平面距离
        getCenterPos.y = cameraPos.y;
        viewPotin.y = cameraPos.y;
        Vector3 offset = viewPotin - getCenterPos;
        cameraPos = cameraPos + offset;
        if (offset.magnitude < 0.5f)
        {

        }
        else
        {
            camera.DOMove(cameraPos, focustime);
        }


    }
}
