using UnityEngine;

namespace Runtime.Player
{
    public class PlayerHeadBob
    {
        float bobbingTimer = 0f;

        /// <summary>
        /// 플레이어가 이동 할 때 카메라 흔들림
        /// <param name="camera"> 플레이어 카메라 트랜스폼
        /// <param name="audio"> 플레이어 발소리 재생용 오디오 소스
        /// <param name="isMoving"> 플레이어가 움직이고 있는지
        /// <param name="defaultEyeHeight"> 기본 카메라 눈높이
        /// <param name="speed"> 플레이어 이동속도
        /// <param name="deltaTime"> Time.deltaTime
        /// </summary>
        public void HandleHeadBobbing(Transform camera, AudioSource audio, bool isMoving, float defaultEyeHeight, float speed, float deltaTime)
        {
            if (camera == null) return;

            if (isMoving)
            {
                // 속도에 따라 강도가 달라지는 카메라 흔들림
                var amount = speed * 0.01f;
                bobbingTimer += speed * deltaTime;

                Vector3 local = camera.localPosition;
                camera.localPosition = new Vector3(local.x, defaultEyeHeight + Mathf.Sin(bobbingTimer) * amount, local.z);

                // 이동 속도에 따른 발자국 소리 재생
                if (audio != null)
                {
                    audio.pitch = speed > 10f ? 1.75f : 1f;
                    if (!audio.isPlaying) audio.Play();
                }

                return;
            }

            // 발자국 소리 정지
            if (audio != null) audio.Stop();

            // 카메라 흔들림 정지 후 카메라를 원래 눈높이로 옮기기
            bobbingTimer = 0f;

            Vector3 current = camera.localPosition;
            Vector3 target = new Vector3(current.x, defaultEyeHeight, current.z);
            camera.localPosition = Vector3.Lerp(current, target, deltaTime * 5f);
        }
    }
}
