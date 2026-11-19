using UnityEngine;

/// <summary>
/// 플레이어 스텟 관련 변수를 가진 클래스
/// </summary>
[System.Serializable]
public class PlayerStats
{
    [Header("Battery")]
    [Tooltip("플레이어 최대 배터리 양")]
    public float battery = 100f;
    [Tooltip("초당 배터리 소모량")]
    public float usePerBattery = 0.5f;

    [Header("Stamina")]
    [Tooltip("플레이어 최대 스태미너 양")]
    public float stamina = 50f;
    [Tooltip("초당 스태미너 소모량")]
    public float usePerStamina = 5f;
    [Tooltip("초당 스태미너 회복량")]
    public float recoverPerStamina = 10f;
}

/// <summary>
/// 플레이어 조작 관련 변수를 가진 클래스
/// </summary>
[System.Serializable]
public class PlayerControl
{
    [Header("Movement")]
    [Tooltip("플레이어 기본 이동 속도")]
    public float moveSpeed = 5f;
    [Tooltip("플레이어 달리기 이동 속도")]
    public float sprintSpeed = 12f;

    [Header("Camera")]
    [Tooltip("마우스 감도")]
    public float mouseSensitivity = 1.5f;
    [Tooltip("카메라 상하 회전 최대 제한 각도")]
    public float maxLookX = 45f;
    [Tooltip("카메라 상하 회전 최소 제한 각도")]
    public float minLookX = 25f;
}

/// <summary>
/// 플레이어 상호작용 관련 변수를 가진 클래스
/// </summary>
[System.Serializable]
public class PlayerInteraction
{
    [Tooltip("상호작용 가능 범위")]
    public float interactionRange = 6f;
}

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Serializable Fields
    [Header("Player Stats")]
    public PlayerStats Stats;
    [Header("Player Control")]
    public PlayerControl Control;
    [Header("Player Interaction")]
    public PlayerInteraction Interaction;

    // Component References
    private Rigidbody rb = null;
    private Camera cam = null;

    // Private Fields
    private float maxBattery = 0f;
    private float maxStamina = 0f;
    private Vector3 moveDir = Vector3.zero;
    private float rotationX = 0f;

    // Checking Fields
    private bool isHiding = false;
    private bool isExhausted = false;
    private bool isDepletBattery = false;
    private bool isDead = false;
    private bool canMove = true;
    private bool isMoving = false;
    private bool isSprinting = false;
    private bool canInteract = true;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        // 컴포넌트 받아오기
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void Start()
    {
        // 최대 배터리와 스태미너 초기화
        maxBattery = Stats.battery;
        maxStamina = Stats.stamina;
    }

    private void Update()
    {
        Movement();
        LookAt();
    }
    #endregion

    #region Stats Functions
    private void UseBattery() // 배터리 소모
    {
        Stats.battery = Mathf.Clamp(
            Stats.battery - Stats.usePerBattery * Time.deltaTime,
            0f,
            maxBattery);
    }

    public void ChargeBattery(float amount) // 배터리 충전
    {
        Stats.battery = Mathf.Clamp(Stats.battery + amount, 0f, maxBattery);
    }

    public void UseStamina() // 스태미너 소모
    {
        
    }
    #endregion

    #region Control Functions
    private void Movement() // 플레이어 이동
    {
        // 이동 키 입력 받아오기
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        moveDir = transform.forward * vertical + transform.right * horizontal; // 이동 방향 계산
        moveDir.Normalize(); // 이동 방향 정규화

        // 플레이어가 이동 중이면 isMoving을 true, 아니면 false
        if (moveDir.magnitude > 0f) isMoving = true;
        else isMoving = false;

        isSprinting = Input.GetKey(KeyCode.LeftShift) && !isExhausted && !isDepletBattery && isMoving; // 달리기 중이면 isSprinting을 true, 아니면 false
        var speed = isSprinting ? Control.sprintSpeed : Control.moveSpeed; // 플레이어 이동 속도 변환

        // 플레이어 이동
        rb.linearVelocity = moveDir * speed;
    }

    private void LookAt() // 플레이어 시점 회전
    {
        // 마우스 입력 받아오기
        var mouseX = Input.GetAxis("Mouse X") * Control.mouseSensitivity;
        var mouseY = Input.GetAxis("Mouse Y") * Control.mouseSensitivity;

        // 플레이어 시점 회전 값
        rotationX -= mouseY;
        // 카메라 x축 회전 각도 제한
        rotationX = Mathf.Clamp(rotationX, -Control.maxLookX, Control.minLookX);

        cam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f); // 카메라 x축 회전
        transform.Rotate(Vector3.up * mouseX); // 플레이어 회전
    }
    #endregion
}
