
using System;
namespace CromisDev.CardMatchingSystem
{
    public class ScoreController
    {
        public uint score;
        public uint Score
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
            CardMatchHandler.OnCardMatched += AddPoints;
        }

        public void StopListening()
        {
            CardMatchHandler.OnCardMatched -= AddPoints;
        }

        public void AddPoints(uint points)
        {
            Score += points;
        }

        public void LoadScore(uint score)
        {
            Score = score;
        }
    }
}
