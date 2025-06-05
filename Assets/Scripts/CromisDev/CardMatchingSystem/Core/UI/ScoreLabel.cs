using TMPro;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class ScoreLabel : MonoBehaviour
    {
        private const string SCORE_FORMAT = "Score: {0}";
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField, Tooltip("Speed at which the score changes (points per second).")]
        private float updateSpeed = 100f;

        private float currentScore = 0f;  // <- usar float para animar suavemente
        private uint targetScore = 0;

        private void OnEnable()
        {
            ScoreController.OnScoreUpdated += OnScoreUpdated;
            label.text = string.Format(SCORE_FORMAT, (uint)currentScore);
        }

        private void OnDisable()
        {
            ScoreController.OnScoreUpdated -= OnScoreUpdated;
        }

        private void OnScoreUpdated(uint newScore)
        {
            targetScore = newScore;
        }

        private void Update()
        {
            if ((uint)currentScore == targetScore) return;

            // Calcula la diferencia y dirección
            float direction = Mathf.Sign(targetScore - currentScore);
            float delta = updateSpeed * Time.deltaTime * direction;

            currentScore += delta;

            // Clamp al target final
            if ((direction > 0f && currentScore >= targetScore) ||
                (direction < 0f && currentScore <= targetScore))
            {
                currentScore = targetScore;
            }

            label.text = string.Format(SCORE_FORMAT, (uint)currentScore);
        }
    }
}
