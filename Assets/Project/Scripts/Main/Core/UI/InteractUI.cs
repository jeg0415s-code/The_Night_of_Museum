using UnityEngine;

public class InteractUI : MonoBehaviour
{
    #region Variables
    [Header("UI Settings")]
    [Tooltip("원래 크기가 되는 카메라와의 거리")]
    [SerializeField, Min(0.01f)] float maxScaleDistance = 5f;
    [Tooltip("거리에 따라 작아질 수 있는 최소값")]
    [SerializeField, Min(0.01f)] float promptMinScale = 0.35f;

    // 컴포넌트
    private Transform camera = null;

    // private 값
    private float distance = float.MaxValue;
    private Vector3 baseScale = Vector3.one;
    #endregion

    private void Start()
    {
        camera = Camera.main.transform;
        CacheScale();
    }

    private void Update()
    {
        // 메인 카메라 쳐다보기
        transform.LookAt(transform.position + camera.rotation * Vector3.forward, camera.rotation * Vector3.up);

        // 메인 카메라와 가까워질수록 작아지기
        distance = Vector3.Distance(transform.position, camera.position);
        var t = Mathf.Clamp01(distance / maxScaleDistance);
        var minScale = Mathf.Clamp(promptMinScale, 0.01f, 1f);
        transform.localScale = baseScale * Mathf.Lerp(minScale, 1f, t);
    }

    private void CacheScale()
    {
        // 기본 스케일 값 초기화
        baseScale = transform.localScale;
        if (baseScale.sqrMagnitude < 0.0001f) baseScale = Vector3.one;
    }
}
