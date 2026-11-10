using UnityEngine;

namespace Runtime.Player
{
    public interface IPlayerPawn
    {
        Transform Transform { get; } // 플레이어의 transform
        bool IsHiding { get; } // 플레이어가 숨었는지
        bool IsNoBattery { get; } // 플레이어 손전등의 배터리가 고갈됐는지
        bool IsExhausted { get; } // 플레이어가 지쳤는지
        bool IsDead { get; } // 플레이어가 죽었는지

        float BatteryRatio { get; } // UI용 배터리 양
        float StaminaRatio { get; } // UI용 스테미나 양
    }
}
