using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LocalPlayer : MonoBehaviourPunCallbacks
{
    public bool IsActive = true;

    [Header("Head")]
    public Camera PlayerCamera;
    public float Sensitivity;
    [SerializeField] private float _zoomValue = 60f;
    [SerializeField] private float _zoomSpeed = 0.07f;

    [Header("Values")]
    public float PlayerHP;
    public float PlayerEndurance;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _runSpeed = 15f;
    [SerializeField] private float _jumpForce = 3f;
    [SerializeField] private float _gravityScale = -9.81f;

    [Header("Jump Ray")]
    [SerializeField] private float _rayLength;

    [Header("Visual Values")]
    public Canvas PlayerCanvas;
    [SerializeField] private Text _healthText;
    [SerializeField] private Image _healthFiledImage;
    [SerializeField] private Text _enduranceText;
    [SerializeField] private Image _enduranceFiledImage;

    [Header("Clamp Rotation")]
    [SerializeField] private float _maxRotation = 90f;
    [SerializeField] private float _minRotation = -90f;

    [Header("Damage System")]
    [SerializeField] private ParticleSystem _bloodEffect;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _screamSounds;

    [Header("Character Controller")]
    [SerializeField] private CharacterController _characterController;

    [Header("Buttons")]
    [SerializeField] private KeyCode _buttonRun;
    [SerializeField] private KeyCode _buttonJump;
    [SerializeField] private KeyCode _buttonZoom;

    private PhotonView _photonView;
    private bool _isGrounded;
    private float _standartZoom;
    private float _cachedSpeed;
    private Vector3 _velocity;
    private Vector3 _moveDirection;
    private Vector3 _clampVerticalRotation;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _cachedSpeed = _speed;
        _standartZoom = PlayerCamera.fieldOfView;
        _characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up * _rayLength, Color.blue);
            CheckGround();

            //Player Interface
            HealthBar();
            EnduranceBar();

            //Player Controll
            Run();
            Jump();
            Zoom();

            if (IsActive)
            {
                MouseRotation();
                Movement();
            }

            //Gravity
            _velocity.y += _gravityScale * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    [PunRPC]
    private void HealthBar()
    {
        _healthFiledImage.fillAmount = PlayerHP / 100;

        _healthText = _healthFiledImage.transform.GetChild(0).GetComponent<Text>();
        _healthText.text = string.Format("{0:0%}", PlayerHP / 100);
    }

    [PunRPC]
    private void EnduranceBar()
    {
        _enduranceFiledImage.fillAmount = PlayerEndurance / 100;

        _enduranceText = _enduranceFiledImage.transform.GetChild(0).GetComponent<Text>();
        _enduranceText.text = string.Format("{0:0%}", PlayerEndurance / 100);
    }

    private void MouseRotation()
    {
        //Mouse Rotation
        float mouseHorizontal = Input.GetAxis("Mouse X") * Sensitivity;
        float mouseVertical = Input.GetAxis("Mouse Y") * -Sensitivity;

        _clampVerticalRotation.x += mouseVertical;
        _clampVerticalRotation.x = Mathf.Clamp(_clampVerticalRotation.x, _minRotation, _maxRotation);

        PlayerCamera.transform.Rotate(_clampVerticalRotation.x, 0f, 0f);
        PlayerCamera.transform.localEulerAngles = _clampVerticalRotation;

        //Body Rotation
        transform.Rotate(0f, mouseHorizontal, 0f);
    }

    private void Movement()
    {
        //Movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        _moveDirection = transform.forward * vertical + transform.right * horizontal;

        _characterController.Move(_moveDirection * _speed * Time.deltaTime);
    }

    private void Zoom()
    {
        if (Input.GetKey(_buttonZoom))
        {
            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, _zoomValue, _zoomSpeed);
        }
        else
        {
            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, _standartZoom, _zoomSpeed);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(_buttonJump))
        {
            if (_isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * -2 * _gravityScale);
            }
        }
    }

    private void Run()
    {
        if (Input.GetKey(_buttonRun))
        {
            _speed = _runSpeed;
        }
        else
        {
            _speed = _cachedSpeed;
        }
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayLength))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }
}
