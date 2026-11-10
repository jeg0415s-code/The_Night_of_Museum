using UnityEngine;

using Runtime.Gameplay;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager Instance { get; private set; }

    #region Variables
    // bool 변수
    private bool isGamePause = false;
    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        GameplayEvent.GamePause += OnGamePause;
    }

    private void OnDisable()
    {
        GameplayEvent.GamePause -= OnGamePause;
    }

    private void Start()
    {
    }

    private void Update()
    {

    }

    #region Functions
    // 이벤트 값 할당
    private void OnGamePause(bool enabled) => isGamePause = enabled;
    #endregion
}
