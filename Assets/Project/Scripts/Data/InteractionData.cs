using System;
using UnityEngine;

/// <summary>
/// 상호작용 오브젝트 타입별 변수값
/// </summary>

[Serializable]
public class BatteryData
{
    [Tooltip("배터리 충전량")]
    public float chargeAmount = 20f;
}

[Serializable]
public class CollectionData
{
    public enum CollectionType
    {
        Real,
        Fake
    }

    [Header("Type")]
    [Tooltip("콜렉션 타입")]
    public CollectionType collectionType = CollectionType.Real;

    [Header("Obj Settings")]
    [Space(10)]
    [Tooltip("상호작용 시 바뀔 오브젝트 레이어 이름")]
    public string collectionLayerName;
    [Tooltip("콜렉션 상호작용 시 회전할 오브젝트")]
    public GameObject collectionObj = null;
    [Tooltip("상호작용 시 콜렉션이 배치될 위치")]
    public Vector3 offset = new Vector3(-1.5f, 0f, 1.5f);
}

[Serializable]
public class CabinetData
{
    [Tooltip("플레이어가 숨겨지는 캐비넷 위치")]
    public Transform hidePoint = null;
}

[Serializable]
public class SafeData
{
}

[Serializable]
public class StatueData
{
}

[Serializable]
public class ExitDoorData
{
}
