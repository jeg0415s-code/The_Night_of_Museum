using UnityEngine;

namespace Runtime.Player
{
    public class PlayerControl
    {
        Rigidbody rb;
        Transform camera;

        /// <summary>
        /// 초기 바인딩
        /// <param name="body"> Rigidbody 컴포넌트
        /// <param name="cam"> 플레이어 카메라 트랜스폼
        /// </summary>
        public void Bind(Rigidbody body, Transform cam)
        {
            rb = body;
            camera = cam;

            if (rb == null) return;
        }

        /// <summary>
        /// 카메라 시아의 정면 받아오기
        /// <param name="horizontal"> 플레이어 좌우 이동 키 입력 값
        /// <param name="vertical"> 플레이어 앞뒤 이동 키 입력 값
        /// </summary>
        private Vector3 CameraRelative(float horizontal, float vertical)
        {
            if (camera == null) return Vector3.zero;

            // 카메라 시점을 기준으로 앞뒤좌우 정하기
            Vector3 forward = camera.forward;
            Vector3 right = camera.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // 마우스 입력값을 반영한 정규화된 앞뒤좌우값 반환
            return (forward * vertical + right * horizontal).normalized;
        }

        /// <summary>
        /// 플레이어 이동
        /// <param name="canMove"> 플레이어 이동 가능 여부
        /// <param name="isMoving"> 플레이어가 움직이고 있는지
        /// <param name="vertical"> 앞뒤 이동 입력 값
        /// <param name="horizontal"> 좌우 이동 입력 값
        /// <param name="speed"> 플레이어 이동 속도
        public void Movement(bool canMove, bool isMoving, float vertical, float horizontal, float speed)
        {
            // 카메라 시점 기준 앞뒤좌우 받아오기
            Vector3 direction = CameraRelative(horizontal, vertical);

            if (rb == null) return;

            if (canMove && isMoving)
            {
                // 플레이어 이동 구현
                Vector3 velocity = direction * speed;
                rb.linearVelocity = new Vector3(velocity.x, 0.01f, velocity.z);
            }
            else
            {
                // 입력이 없을 땐 이동속도를 0으로
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}
