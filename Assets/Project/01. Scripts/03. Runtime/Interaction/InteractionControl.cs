using UnityEngine;

using Runtime.Gameplay;
using Runtime.Player;
using Runtime.UI;

/// <summary>
/// 상호작용 오브젝트 타입별 기능들 모음 namespace
/// </summary>
namespace Runtime.Interaction
{
    /// <summary>
    /// 배터리 오브젝트
    /// </summary>
    public class InteractionControl
    {
        readonly InteractCollection interactCollection = new InteractCollection();

        /// <summary>
        /// 플레이어 배터리 충전
        /// <param name="amount"> 배터리 충전량
        /// </summary>
        public void HandleBattery(float amount)
        {
            PlayerEvent.SetChargeBattery(amount);
            Debug.Log("플레이어 배터리 충전");
        }

        /// <summary>
        /// 콜렉션 상호작용
        /// <param name="camera"> 카메라 트랜스폼
        /// <param name="obj"> 상호작용 오브젝트 트랜스폼
        /// <param name="colLayer"> 상호작용 시 바뀔 레이어 이름
        /// <param name="offset"> 상호작용 시 오브젝트가 배치될 위치
        /// <param name="isInteract"> 콜렉션 오브젝트를 상호작용 중인지
        /// </summary>
        public void HandleCollection(
            Camera camera,
            GameObject obj,
            string colLayer,
            Vector3 offset,
            bool isInteract)
        {
            if (camera == null) return;

            if (isInteract)
            {
                interactCollection.StartCollectionInteract(camera, obj, colLayer, offset, isInteract);
            }
            else
            {
                interactCollection.StopCollectionInteract(obj);
                Debug.Log("Test");
            }
        }

        /// <summary>
        /// 캐비넷 상호작용
        /// </summary>
        public void HandleCabient()
        {

        }

        /// <summary>
        /// 금고 상호작용
        /// </summary>
        public void HandleSafe()
        {

        }

        /// <summary>
        /// 석상 상호작용
        /// </summary>
        public void HandleStatue()
        {

        }

        /// <summary>
        /// 출구 상호작용
        /// </summary>
        public void HandleExitDoor()
        {

        }
    }

    public class InteractCollection
    {
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Transform originalParent;
        private int originalLayer;
        
        public void StartCollectionInteract(
            Camera camera,
            GameObject obj,
            string colLayer,
            Vector3 offset,
            bool isInteract)
        {
            originalPosition = obj.transform.position;
            originalRotation = obj.transform.rotation;
            originalParent = obj.transform.parent;
            originalLayer = obj.layer;
            
            // 게임 일시정지 이벤트를 true로 변경
            GameplayEvent.SetGamePause(true);
            // 상호작용 활성/비활성화 이벤트를 false로 변경
            PlayerEvent.SetCanInteraction(false);
            // Collection UI 활성화
            UIEvent.SetPanelChanged("Collection", true);

            // 콜렉션 오브젝트의 레이어를 colLayer로 변경
            if (obj != null) { obj.gameObject.layer = LayerMask.NameToLayer(colLayer); }

            // 콜렉션 오브젝트의 위치를 카메라 앞으로 고정
            Vector3 objOffset = offset.sqrMagnitude < 0.01f
                ? new Vector3(-1.5f, 0f, 1.5f)
                : offset;

            obj.transform.SetParent(camera.transform);
            obj.transform.localPosition = objOffset;
            obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        public void StopCollectionInteract(
            GameObject obj)
        {
            // 게임 일시정지 이벤트를 false로 변경
            GameplayEvent.SetGamePause(false);
            // 상호작용 활성/비활성화 이벤트를 true로 변경
            PlayerEvent.SetCanInteraction(true);
            // Collection UI 비활성화
            UIEvent.SetPanelChanged("Collection", false);

            obj.transform.SetParent(originalParent);
            obj.transform.SetPositionAndRotation(originalPosition, originalRotation);
            obj.layer = originalLayer;
        }
    }
}
