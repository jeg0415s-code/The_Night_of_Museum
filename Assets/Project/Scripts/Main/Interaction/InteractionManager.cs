using UnityEngine;
using System.Collections.Generic;

using Runtime.Interaction;
using Runtime.Player;

public class InteractionManager : MonoBehaviour
{
    // 오브젝트의 상호작용 타입 Enum
    public enum InteractionType
    {
        Battery,
        Collection,
        Cabinet,
        Safe,
        Statue,
        ExitDoor
    }

    // namespace
    readonly InteractionControl Control = new InteractionControl();

    // 현재 활성화된 상호작용 오브젝트 목록
    private static readonly List<InteractionManager> instances = new List<InteractionManager>();
    public static IReadOnlyList<InteractionManager> Instances => instances;

    #region Variables
    [Header("Object Type")]
    [Tooltip("오브젝트의 상호작용 타입")]
    [SerializeField] private InteractionType type = InteractionType.Battery;
    
    [Header("UI Settings")]
    [Tooltip("상호작용 가능 표시 UI")]
    [SerializeField] GameObject interactionUI = null;

    // private 변수
    private bool canInteraction = true;
    private bool isCollectionInteract = false;

    [Header("Interaction Data")]
    [SerializeField] private BatteryData batteryData = new BatteryData();
    [SerializeField] private CollectionData collectionData = new CollectionData();
    [SerializeField] private CabinetData cabinetData = new CabinetData();
    [SerializeField] private SafeData safeData = new SafeData();

    public BatteryData Battery => batteryData;
    public CollectionData Collection => collectionData;
    public CabinetData Cabient => cabinetData;
    public SafeData Safe => safeData;
    #endregion

    private void OnEnable()
    {
        instances.Add(this); // 활성화된 상호작용 오브젝트 목록에 등록

        PlayerEvent.CanInteraction += OnCanInteraction;
    }
    private void OnDisable()
    {
        instances.Remove(this); // 활성화된 상호작용 오브젝트 목록에서 제외

        PlayerEvent.CanInteraction -= OnCanInteraction;
    }

    private void Start()
    {
        // 초기화
        DataInit();
    }

    private void Update()
    {
        // 만약 상호작용 UI가 활성화 돼있을 때 canInteract가 false가 되면 상호작용 UI를 비활성화
        if (!canInteraction && interactionUI.activeSelf) { interactionUI.SetActive(false); }
    }

    #region Functions
    private void OnCanInteraction(bool enabled) => canInteraction = enabled;

    public void HandleInteraction()
    {
        if (!canInteraction && type != InteractionType.Collection) return;
        

        // 오브젝트 타입에 따른 상호작용 기능 다르게 하기
        switch(type)
        {
        case InteractionType.Battery:
            Control.HandleBattery(batteryData.chargeAmount);
            Destroy(this.gameObject);
            break;
        case InteractionType.Collection:
            if (isCollectionInteract) isCollectionInteract = false;
            else isCollectionInteract = true;

            Control.HandleCollection(
                Camera.main,
                collectionData.collectionObj,
                collectionData.collectionLayerName,
                collectionData.offset,
                isCollectionInteract);
            break;
        case InteractionType.Cabinet:
            Debug.Log("캐비넷 상호작용");
            break;
        case InteractionType.Safe:
            Debug.Log("금고 상호작용");
            break;
        case InteractionType.Statue:
            Debug.Log("석상 상호작용");
            break;
        case InteractionType.ExitDoor:
            Debug.Log("출구 상호작용");
            break; 
        }
    }

    // 상호작용 UI 활성/비활성화
    public void SetActiveInteractUI()
    {
        if (!canInteraction) return;

        interactionUI.SetActive(!interactionUI.activeSelf);
    }

    private void DataInit()
    {
        canInteraction = true;
        isCollectionInteract = false;
        
        // 상호작용 UI 비활성화
        interactionUI.SetActive(false);

        // 콜렉션 데이터 초기화
        if (collectionData.collectionObj != null) 
            collectionData.collectionObj.layer = 0;
    }
    #endregion
}
