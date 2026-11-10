using UnityEngine;

using Runtime.Gameplay;
using Runtime.Player;

public class PlayerController : MonoBehaviour
{
    // namespaces
    readonly PlayerControl Control = new PlayerControl();
    readonly PlayerHeadBob HeadBob = new PlayerHeadBob();
    readonly PlayerInteraction Interaction = new PlayerInteraction();

    #region Variables
    [Header("Movement Settings")]
    [Tooltip("걷기 이동 속도")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("달리기 이동 속도")]
    [SerializeField] private float sprintSpeed = 12f;

    [Header("Camera Settings")]
    [Tooltip("카메라 민감도")]
    [SerializeField] private float cameraSensitivity = 1.5f;
    [Tooltip("카메라 X축 최소 각도")]
    [SerializeField] private float minXLook = -45f;
    [Tooltip("카메라 X축 최대 각도")]
    [SerializeField] private float maxXLook = 45f;
    [Tooltip("플레이어 시아 높이")]
    [SerializeField] private float eyeHeight = 2f;

    [Header("Interaction Settings")]
    [Tooltip("상호작용 가능 범위")]
    [SerializeField] private float interactionRange = 6f;
    [Tooltip("상호작용 가능 시야각")]
    [SerializeField, Range(1f, 180f)] private float viewAngle = 60f;
    [Tooltip("상호작용을 막는 벽 오브젝트 레이어")]
    [SerializeField] private LayerMask obstacleLayer;

    // bool 변수
    private bool isGamePause = false;
    private bool canMove = true;
    private bool isExhausted = false;
    private bool isMoving = false;
    private bool isSprinting = false;

    // 컴포넌트
    private Rigidbody rb = null;
    private Transform camera = null;
    private AudioSource audio = null;

    // 카메라 private 변수
    private float xRotation = 0f;
    private float lookYawVelocity = 0f;
    private float lookPitchVelocity = 0f;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        GameplayEvent.GamePause += OnGamePause;
        PlayerEvent.MovementEnabledChanged += OnMovementEnabledChanged;
        PlayerEvent.ExhaustedChanged += OnExhaustedChanged;
    }

    private void OnDisable()
    {
        GameplayEvent.GamePause -= OnGamePause;
        PlayerEvent.MovementEnabledChanged -= OnMovementEnabledChanged;
        PlayerEvent.ExhaustedChanged -= OnExhaustedChanged;
    }

    private void Start()
    {
        if (Camera.main != null) { camera = Camera.main.transform; }
        else { Debug.LogError("Main camera is null!"); return; }

        if (audio == null) { audio = GetComponent<AudioSource>(); }
        else { Debug.LogError("Player audio source is null!"); }

        // PlayerControl에 값 할당
        Control.Bind(rb, camera);

        // 카메라 눈 높이 설정
        camera.position = new Vector3(0f, eyeHeight, 0f);
    }

    private void Update()
    {
        HandleInteraction();

        if (!canMove || isGamePause) return;
        HandleMovement();
    }

    private void LateUpdate()
    {
        if (!canMove || isGamePause) return;
        HandleCamera();
    }

    #region Functions
    // 이벤트 값 할당
    private void OnGamePause(bool enabled) => isGamePause = enabled;
    private void OnMovementEnabledChanged(bool enabled) => canMove = enabled;
    private void OnExhaustedChanged(bool enabled) => isExhausted = enabled;

    private void HandleMovement()
    {
        // 달리기 가능 여부 확인
        if (!isExhausted) isSprinting = GameplayInput.SprintHeld;
        else isSprinting = false;

        // 이동키 입력값 받기
        var vertical = GameplayInput.VerticalInput;
        var horizontal = GameplayInput.HorizontalInput;
        isMoving = vertical != 0f || horizontal != 0f;
        PlayerEvent.SetIsMovingNow(isMoving);

        // 이동
        var curSpeed = isSprinting ? sprintSpeed : moveSpeed;
        Control.Movement(canMove, isMoving, GameplayInput.VerticalInput, GameplayInput.HorizontalInput, curSpeed);

        // 이동에 따른 카메라 흔들림
        HeadBob.HandleHeadBobbing(camera, audio, isMoving, eyeHeight, curSpeed, Time.deltaTime);
    }

    private void HandleCamera()
    {
        // 마우스 입력값 받기
        var mouseX = GameplayInput.MouseX * cameraSensitivity;
        var mouseY = GameplayInput.MouseY * cameraSensitivity;

        // 마우스 입력값에 따른 카메라 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxXLook, minXLook);
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleInteraction()
    {
        // 상호작용 실행
        Interaction.HandleInteraction(
            transform,
            camera,
            viewAngle,
            interactionRange,
            obstacleLayer,
            GameplayInput.InteractDown);
    }
    #endregion

    // 상호작용 가능 범위와 시야각 표시
    private void OnDrawGizmosSelected()
    {
        // 상호작용 가능 범위 표시
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // 시야각 표시
        var halfAngle = viewAngle * 0.5f;
        var left = Quaternion.AngleAxis(-halfAngle, Vector3.up) * transform.forward;
        var right = Quaternion.AngleAxis(halfAngle, Vector3.up) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + left * interactionRange);
        Gizmos.DrawLine(transform.position, transform.position + right * interactionRange);
    }
}
