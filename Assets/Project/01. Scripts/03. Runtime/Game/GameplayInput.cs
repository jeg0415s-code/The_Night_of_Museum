using UnityEngine;

namespace Runtime.Gameplay
{
    public static class GameplayInput
    {
        // 앞뒤 입력값
        public static float VerticalInput =>
            (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);
        // 좌우 입력값
        public static float HorizontalInput =>
            (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);
        // 달리기 키 입력값
        public static bool SprintHeld => Input.GetKey(KeyCode.LeftShift);

        public static bool InteractDown => Input.GetKeyDown(KeyCode.E); // 상호작용 키 입력값
        public static bool MenuDown => Input.GetKeyDown(KeyCode.Escape); // 메뉴 키 입력값

        public static float MouseX => Input.GetAxis("Mouse X"); // X축 마우스 입력값
        public static float MouseY => Input.GetAxis("Mouse Y"); // Y축 마우스 입력값

        public static bool AltHeld => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt); // Alt키를 누르고 있을 때
        public static bool AltUp => Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt); // Alt키를 뗐을 때
    }
}
