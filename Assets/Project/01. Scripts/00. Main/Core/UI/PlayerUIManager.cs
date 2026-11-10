using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Runtime.Player;

public class PlayerUIManager : MonoBehaviour
{
    // namespaces
    IPlayerPawn Pawn;

    #region Variables
    [Header("UI")]
    [Tooltip("배터리 슬라이더")]
    [SerializeField] private Slider batteryBarSlider = null;
    [Tooltip("스테미나 슬라이더(CanvasGroup 컴포넌트가 포함돼있어야함)")]
    [SerializeField] private Slider staminaBarSlider = null;
    [Tooltip("스테미나 Fill을 자르는 마스크(RectMask2D 컴포넌트가 포함돼있어야함)")]
    [SerializeField] RectMask2D staminaMask = null;
    [Tooltip("스테미나 바 Fadeinout 속도")]
    [SerializeField] float staminaBarFadeSpeed = 2f;
    [Tooltip("획득한 콜렉션 수 텍스트")]
    [SerializeField] private TextMeshProUGUI collectionCountText = null;

    private float staminaBarGroupAlpha = 0f;

    // 컴포넌트
    private CanvasGroup staminaBarGroup = null;
    #endregion

    private void Start()
    {
        // IPlayerPawn 받아오기
        Pawn = FindFirstObjectByType<Player>();

        // 스테미나 바의 CanvasGroup 컴포넌트 받아오기
        if (staminaBarSlider != null) staminaBarGroup = staminaBarSlider.gameObject.GetComponent<CanvasGroup>();
        else return;

        // 스테미나 바 안보이게 설정
        staminaBarGroupAlpha = 0f;
        staminaBarGroup.alpha = staminaBarGroupAlpha;
    }

    private void Update()
    {
        HandleSliders();
    }

    #region Functions
    private void HandleSliders()
    {
        if (Pawn == null) return;

        // hp/stamina 바 값 변경
        batteryBarSlider.value = Pawn.BatteryRatio;
        HandleStaminaBar(Pawn.StaminaRatio);
    }

    private void HandleStaminaBar(float value)
    {
        if (staminaBarSlider == null || staminaMask == null) return;

        // 스테미나 바의 값이 변경되거나, 1이 되면 자연스럽게 FadeInOut되기
        var target = Pawn.StaminaRatio < 1f ? 1f : 0f;
        if (staminaBarGroup != null)
            staminaBarGroup.alpha = Mathf.MoveTowards(staminaBarGroup.alpha, target, staminaBarFadeSpeed * Time.deltaTime);

        var clampValue = Mathf.InverseLerp(staminaBarSlider.minValue, staminaBarSlider.maxValue, value); // 0~1로 정규화

        // 양쪽에서 중앙으로 잘려 들어가도록 마스크의 좌우 padding 조절
        var width = staminaMask.rectTransform.rect.width;
        var pad = (1f - clampValue) * width * 0.5f;
        staminaMask.padding = new Vector4(pad, 0f, pad, 0f);
    }
    #endregion
}
