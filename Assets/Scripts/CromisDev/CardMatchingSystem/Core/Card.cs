using UnityEngine;
using System.Collections;
using UnityEngine.XR.WSA;

namespace CromisDev.CardMatchingSystem
{
    public enum CardState { Hidden, Revealed, Flipping }

    public class Card : MonoBehaviour
    {
        [SerializeField] private Sprite frontSprite;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private AnimationCurve flipCurve;
        [SerializeField] private float flipDuration = 0.5f;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public CardState CardState { get; private set; } = CardState.Hidden;
        public bool IsFlipping { get; private set; }
        public string CardId { get; private set; }
        public bool IsMatched { get; private set; }

        public Vector3 GetSize => spriteRenderer.bounds.size;

        private void OnMouseDown()
        {
            if (!GameController.ShouldInteract) return;
            Flip();
        }

        public void SetMatched()
        {
            IsMatched = true;
            spriteRenderer.color = Color.green;
        }

        private void UpdateSprite()
        {
            spriteRenderer.sprite = CardState == CardState.Hidden ? backSprite : frontSprite;
        }

        public void Initialize(CardState initialState, Sprite frontSprite, Sprite backSprite)
        {
            this.frontSprite = frontSprite;
            this.backSprite = backSprite;
            CardId = frontSprite.name;
            CardState = initialState;
            UpdateSprite();
        }

        public void Flip()
        {
            if (!IsFlipping && !IsMatched)
            {
                StartCoroutine(FlipAnimation());
            }
        }

        private IEnumerator FlipAnimation()
        {
            IsFlipping = true;

            yield return ScaleCard(1f, 0f);
            CardState = CardState == CardState.Hidden ? CardState.Revealed : CardState.Hidden;
            UpdateSprite();
            yield return ScaleCard(0f, 1f);

            IsFlipping = false;

            if (CardState == CardState.Revealed && !IsMatched)
            {
                GameController.OnCardFlipped(this);
            }
        }
        private IEnumerator ScaleCard(float startScale, float endScale)
        {
            float flipTime = flipDuration * 0.5f;
            float elapsedTime = 0f;
            Vector3 originalScale = new(startScale, transform.localScale.y, transform.localScale.z);
            Vector3 targetScale = new(endScale, transform.localScale.y, transform.localScale.z);

            while (elapsedTime < flipTime)
            {
                float t = elapsedTime / flipTime;
                float curveValue = flipCurve.Evaluate(t);
                transform.localScale = Vector3.Lerp(originalScale, targetScale, curveValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}