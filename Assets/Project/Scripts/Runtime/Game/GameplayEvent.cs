using System;

namespace Runtime.Gameplay
{
    /// <summary>
    /// 게임 관련 이벤트 클래스
    /// </summary>
    public static class GameplayEvent
    {
        public static event Action<bool> GamePause; // 게임 일시정지 이벤트

        //--------------------------------------------------------------------------------//

        public static void SetGamePause(bool enabled) => GamePause?.Invoke(enabled);
    }
}
