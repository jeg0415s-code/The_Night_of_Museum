using System;

namespace Runtime.UI
{
    /// <summary>
    /// UI 관련 이벤트 클래스
    /// </summary>
    public static class UIEvent
    {
        public static event Action<string, bool> PanelChanged; // 판넬 활성/비활성화

        //--------------------------------------------------------------------------------//

        public static void SetPanelChanged(string name, bool active) => PanelChanged?.Invoke(name, active);
    }
}
