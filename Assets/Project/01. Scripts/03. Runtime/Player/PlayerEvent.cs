using System;

namespace Runtime.Player
{
    /// <summary>
    /// 플레이어 관련 이벤트 클래스
    /// </summary>
    public static class PlayerEvent
    {
        public static event Action<bool> IsMovingNow; // 플레이어가 이동하고 있는지
        public static event Action<bool> MovementEnabledChanged; // 플레이어 이동 가능 이벤트

        public static event Action<bool> ExhaustedChanged; // 플레이어가 지쳤는지
        public static event Action BatteryDepleted; // 플레이어 배터리가 고갈 됐는지

        public static event Action<float> ChargeBattery; // 플레이어 배터리 충전 이벤트

        public static event Action<bool> CanInteraction; // 상호작용 가능 상태인지

        //--------------------------------------------------------------------------------//

        public static void SetIsMovingNow(bool enabled) => IsMovingNow?.Invoke(enabled);
        public static void SetMovementEnabled(bool enabled) => MovementEnabledChanged?.Invoke(enabled);

        public static void SetExhausted(bool enabled) => ExhaustedChanged?.Invoke(enabled);
        public static void RaiseBatteryDepleted() => BatteryDepleted?.Invoke();

        public static void SetChargeBattery(float amount) => ChargeBattery?.Invoke(amount);

        public static void SetCanInteraction(bool enabled) => CanInteraction?.Invoke(enabled);
    }
}
