using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using URandom = UnityEngine.Random;

namespace CromisDev.CardMatchingSystem
{
    [CreateAssetMenu(fileName = "NewCardDeck", menuName = "Card Game/Card Deck Data")]
    public class CardDeckDataSO : ScriptableObject
    {
        [Header("Card Backs")]
        [SerializeField] private List<Sprite> cardBacks = new();

        [Header("Card Fronts")]
        [SerializeField] private List<Sprite> cardFronts = new();

        public Sprite GetRandomCardBack()
        {
            if (cardBacks == null || cardBacks.Count == 0)
            {
                throw new InvalidOperationException("Card backs list is null or empty");
            }
            return cardBacks[URandom.Range(0, cardBacks.Count)];
        }

        public List<Sprite> GetUniqueFronts(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero", nameof(count));
            }

            if (cardFronts == null || cardFronts.Count < count)
            {
                throw new InvalidOperationException(
                    $"Not enough card fronts. Requested: {count}, Available: {cardFronts?.Count ?? 0}");
            }

            return cardFronts.OrderBy(x => URandom.value).Take(count).ToList();
        }
    }
}