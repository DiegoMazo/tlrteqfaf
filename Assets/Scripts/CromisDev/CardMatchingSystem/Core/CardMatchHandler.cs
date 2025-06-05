using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class CardMatchHandler
    {
        private readonly MonoBehaviour coroutineRunner;
        private readonly float delay;
        private readonly List<Card> activeCards = new();
        private readonly HashSet<Card> pendingComparisons = new();

        public CardMatchHandler(MonoBehaviour coroutineRunner, float delay = 0.5f)
        {
            this.coroutineRunner = coroutineRunner;
            this.delay = delay;
        }

        public void OnCardFlipped(Card card)
        {
            if (pendingComparisons.Contains(card) || card.IsMatched)
                return;

            activeCards.Add(card);

            List<Card> unmatched = activeCards.FindAll(c => !pendingComparisons.Contains(c) && !c.IsMatched);
            while (unmatched.Count >= 2)
            {
                Card a = unmatched[0];
                Card b = unmatched[1];
                unmatched.RemoveRange(0, 2);

                pendingComparisons.Add(a);
                pendingComparisons.Add(b);

                coroutineRunner.StartCoroutine(CheckMatchCoroutine(a, b));
            }
        }

        private IEnumerator CheckMatchCoroutine(Card a, Card b)
        {
            yield return new WaitForSeconds(delay);

            if (a.CardId == b.CardId)
            {
                a.SetMatched();
                b.SetMatched();
            }
            else
            {
                a.Flip();
                b.Flip();
            }

            pendingComparisons.Remove(a);
            pendingComparisons.Remove(b);
            activeCards.Remove(a);
            activeCards.Remove(b);
        }
    }

}
