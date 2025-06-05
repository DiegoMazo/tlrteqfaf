using UnityEngine;
using System.Threading.Tasks;
using System;

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

        public static Action<Card> OnCardFlipped;

        public Vector3 GetSize => spriteRenderer.bounds.size;

        private void OnMouseDown()
        {
            if (!GameController.ShouldInteract) return;
            _ = Flip();
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

        public async Task PeekAsync(float revealTime = 0.5f)
        {
            await Flip();
            await Task.Delay((int)(revealTime * 1000));
            await Flip();
        }

        public async Task Flip()
        {
            float flipTime = flipDuration * 0.5f;

            if (IsFlipping || IsMatched) return;

            IsFlipping = true;

            await ScaleCardAsync(1f, 0f);
            CardState = CardState == CardState.Hidden ? CardState.Revealed : CardState.Hidden;
            UpdateSprite();
            await ScaleCardAsync(0f, 1f);

            IsFlipping = false;

            if (CardState == CardState.Revealed && !IsMatched)
            {
                OnCardFlipped?.Invoke(this);
            }
        }

        private async Task ScaleCardAsync(float startScale, float endScale)
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
                await Task.Yield();
            }

            transform.localScale = targetScale;
        }
    }
}