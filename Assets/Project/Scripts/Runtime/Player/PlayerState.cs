using System;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerState
    {
        public float Battery; // 플레이어 배터리 양
        public float MaxBattery; // 최대 배터리 양
        public float Stamina; // 플레이어 스테미나 양
        public float MaxStamina; // 최대 스테미나 양

        public float UseBatteryPerSec; // 초당 배터리 사용량
        public float UseStaminaPerSec; // 초당 스테미나 사용량
        public float ChargeStaminaPerSec; // 초당 스테미나 충전량

        private bool IsExhausted; // 플레이어가 지쳤는지
        public bool IsNoBattery; // 플레이어 배터리가 다 떨어졌는지

        public event Action BatteryDepleted; // 배터리가 다 닳았을 때의 이벤트

        /// <summary>
        /// 배터리/스테미나 등의 변수값 초기화
        /// <param name="battery"> 플레이어 배터리량
        /// <param name="stamina"> 플레이어 스테미나량
        /// <param name="useBatteryPerSec"> 초당 배터리 사용량
        /// <param name="useStaminaPerSec"> 초당 스테미나 사용량
        /// <param name="chargeStaminaPerSec"> 초당 스테미나 충전량
        /// </summary>
        public void Initialize(
            float battery,
            float stamina,
            float useBatteryPerSec,
            float useStaminaPerSec,
            float chargeStaminaPerSec)
        {
            Battery = MaxBattery = battery;
            Stamina = MaxStamina = stamina;
            UseBatteryPerSec = useBatteryPerSec;
            UseStaminaPerSec = useStaminaPerSec;
            ChargeStaminaPerSec = chargeStaminaPerSec;

            IsExhausted = false;
            IsNoBattery = false;
        }

        /// <summary>
        /// 배터리 사용
        /// <param name="deltaTime"> Time.deltaTime
        /// </summary>
        public void UseBattery(float deltaTime)
        {
            if (Battery > 0)
            {
                Battery -= UseBatteryPerSec * deltaTime;
                return;
            }

            // 배터리가 0이하면 실행
            BatteryDepletion();
        }

        /// <summary>
        /// 배터리 충전
        /// <param name="amount"> 초당 충전량
        /// </summary>
        public void ChargeBattery(float amount)
        {
            if (IsNoBattery) return;

            // amount값 만큼 배터리 충전
            Battery = Mathf.Min(Battery + amount, MaxBattery);
        }

        /// <summary>
        /// 스테미나 사용
        /// <param name="deltaTime"> Time.deltaTime
        /// </summary>
        public void UseStamina(float deltaTime)
        {
            if (Stamina <= 0f && IsExhausted) return;

            // 스테미나 사용
            Stamina -= UseStaminaPerSec * deltaTime;
            if (Stamina > 0f) return;

            // 스테미나가 다 닳으면 지침 상태 돌입
            Stamina = 0f;
            IsExhausted = true;
            PlayerEvent.SetExhausted(true);
        }

        /// <summary>
        /// 스테미나 충전
        /// <param name="deltaTime"> Time.deltaTime
        /// </summary>
        public void ChargeStamina(float deltaTime)
        {
            // 자동 스테미나 충전
            Stamina = Mathf.Min(Stamina + (ChargeStaminaPerSec * deltaTime), MaxStamina);
            // 플레이어가 지침 상태이고, 스테미나 양이 30프로 이상 차면 지침 상태 취소
            if (IsExhausted && Stamina >= MaxStamina * 0.3f)
            {
                IsExhausted = false;
                PlayerEvent.SetExhausted(false);
            }
        }

        // 플레이어 배터리 고갈 상태
        private void BatteryDepletion()
        {
            Battery = 0f;
            if (IsNoBattery) return;

            IsNoBattery = true;
            BatteryDepleted?.Invoke();
        }
    }
}
