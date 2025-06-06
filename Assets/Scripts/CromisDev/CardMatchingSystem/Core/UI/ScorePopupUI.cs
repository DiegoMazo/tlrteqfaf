using UnityEngine;
using TMPro;
using System.Threading.Tasks;

namespace CromisDev.CardMatchingSystem
{
    public class ScorePopupUI : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransformPanel;
        [SerializeField] private TextMeshProUGUI scoreTextPrefab;

        [SerializeField] private Vector2 startOffset = new(-100f, -50f);
        [SerializeField] private float riseDistance = 50f;
        [SerializeField] private float animationDuration = 1f;

        private void OnEnable()
        {
            CardMatchHandler.OnCardMatched += ShowScoreText;
        }

        private void OnDisable()
        {
            CardMatchHandler.OnCardMatched -= ShowScoreText;
        }

        private void ShowScoreText(uint points)
        {
            TextMeshProUGUI popup = Instantiate(scoreTextPrefab, rectTransformPanel.transform);
            popup.text = $"+{points}";

            RectTransform rect = popup.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = startOffset;

            _ = FadeAndMoveAsync(popup, rect);
        }

        private async Task FadeAndMoveAsync(TextMeshProUGUI popup, RectTransform rect)
        {

            float elapsed = 0f;
            Vector2 startPos = rect.anchoredPosition;
            Vector2 endPos = startPos + new Vector2(0f, riseDistance);

            Color startColor = popup.color;
            Color endColor = new(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsed < animationDuration)
            {
                float t = elapsed / animationDuration;
                rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                popup.color = Color.Lerp(startColor, endColor, t);
                elapsed += Time.deltaTime;
                await Task.Yield();
            }

            if (popup != null)
            {
                Destroy(popup.gameObject);
            }
        }
    }
}
