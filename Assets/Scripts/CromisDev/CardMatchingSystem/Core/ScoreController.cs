
using System;

namespace CromisDev.CardMatchingSystem
{
    public class ScoreController
    {
        public static uint score;
        public static uint Score
        {
            get => score;
            private set
            {
                if (score == value) return;

                score = value;
                OnScoreUpdated?.Invoke(score);
            }
        }

        public static Action<uint> OnScoreUpdated;

        public void StartListening()
        {
            CardMatchHandler.OnCardMatched += HandleCardMatched;
        }

        public void StopListening()
        {
            CardMatchHandler.OnCardMatched -= HandleCardMatched;
        }

        private void HandleCardMatched(uint points)
        {
            Score += points;
        }
    }
}
