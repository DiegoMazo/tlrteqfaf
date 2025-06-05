using UnityEngine;
using System.Collections;

namespace CromisDev.CardMatchingSystem
{
    public enum CardState { Hidden, Revealed, Flipping }

    public class Card : MonoBehaviour
    {
        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private AnimationCurve flipCurve;
        [SerializeField] private float flipDuration = 0.5f;

        private SpriteRenderer spriteRenderer;
        private CardState currentState = CardState.Hidden;
       
        public bool IsFlipping { get; private set; }
        public string CardId { get; private set; }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = backSprite;
        }

        private void UpdateSprite()
        {
            spriteRenderer.sprite = currentState == CardState.Hidden ? backSprite : frontSprite;
        }

        public void Initialize(CardState initialState, Sprite frontSprite, Sprite backSprite)
        {
            this.frontSprite = frontSprite;
            this.backSprite = backSprite;
            CardId = frontSprite.name;
            currentState = initialState;
            UpdateSprite();
        }

        public void Flip()
        {
            if (!IsFlipping)
            {
                StartCoroutine(FlipAnimation());
            }
        }

        private IEnumerator FlipAnimation()
        {
            IsFlipping = true;
            yield return ScaleCard(1f, 0f);
            UpdateSprite();
            yield return ScaleCard(0f, 1f);

            currentState = currentState == CardState.Hidden ? CardState.Revealed : CardState.Hidden;
            IsFlipping = false;
        }

        private IEnumerator ScaleCard(float startScale, float endScale)
        {
            float elapsedTime = 0f;
            Vector3 originalScale = new(startScale, transform.localScale.y, transform.localScale.z);
            Vector3 targetScale = new(endScale, transform.localScale.y, transform.localScale.z);

            while (elapsedTime < flipDuration)
            {
                float t = elapsedTime / flipDuration;
                float curveValue = flipCurve.Evaluate(t);
                transform.localScale = Vector3.Lerp(originalScale, targetScale, curveValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}