using System;
using UnityEngine;
namespace CromisDev.CardMatchingSystem
{
    public class ComboTracker
    {
        private readonly float maxComboInterval;
        private float lastMatchTime;
        private uint comboCount;

        public uint CurrentCombo => comboCount;

        public static event Action<uint> OnComboIncreased;

        public ComboTracker(float maxComboInterval)
        {
            this.maxComboInterval = maxComboInterval;
        }

        public void StartListening()
        {
            CardMatchHandler.OnCardMatched += HandleMatch;
        }

        public void StopListening()
        {
            CardMatchHandler.OnCardMatched -= HandleMatch;
        }

        private void HandleMatch(uint _)
        {
            float currentTime = Time.time;

            if (currentTime - lastMatchTime <= maxComboInterval)
            {
                comboCount++;
            }
            else
            {
                comboCount = 1;
            }

            lastMatchTime = currentTime;

            OnComboIncreased?.Invoke(comboCount);
        }

        public void ResetCombo()
        {
            comboCount = 0;
            lastMatchTime = 0f;
        }
    }
}
