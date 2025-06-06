using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CromisDev.Extensions.Collections;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class BoardLayoutController : MonoBehaviour
    {
        public static BoardLayoutController Instance { get; private set; }

        [Header("Card and Layout Settings")]
        [SerializeField] private Card cardPrefab;
        [SerializeField] private CardDeckDataSO deckDataSO;
        [SerializeField] private Vector2Int gridSize = new(4, 4);
        [SerializeField] private float horizontalPadding = 0.2f;
        [SerializeField] private float verticalPadding = 0.2f;
        private readonly List<Card> cards = new();
        public static Action OnBoardCreated;

        public static float Width { get; private set; }
        public static float Height { get; private set; }

        public static Action<float, float> OnSizeCalculated;

        public static IReadOnlyList<Card> Cards => Instance.cards;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static async Task RevelaCardsAsync(float revealTime)
        {
            await Task.WhenAll
            (
                Instance.cards.Where(c => c.CardState == CardState.Hidden && !c.IsMatched).Select(c => c.PeekAsync(revealTime))
            );
        }
        public static void GenerateBoard()
        {
            Vector2Int gridSize = Instance.gridSize;

            Vector3 cardSize = Instance.cardPrefab.GetSize;
            int totalCards = Instance.gridSize.x * Instance.gridSize.y;

            Width = gridSize.x * cardSize.x + (gridSize.x - 1) * Instance.horizontalPadding;
            Height = gridSize.y * cardSize.z + (gridSize.y - 1) * Instance.verticalPadding;

            OnSizeCalculated?.Invoke(Width, Height);

            Vector3 origin = new(-Width / 2f + cardSize.x / 2f, 0f, Height / 2f - cardSize.z / 2f);

            Sprite back = Instance.deckDataSO.GetRandomCardBack();
            List<Sprite> uniqueFronts = Instance.deckDataSO.GetUniqueFronts(totalCards / 2);

            List<Sprite> allFronts = new();

            foreach (var sprite in uniqueFronts)
            {
                allFronts.Add(sprite);
                allFronts.Add(sprite);
            }

            allFronts.Shuffle();

            int index = 0;
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector3 position = origin + new Vector3
                    (
                        x * (cardSize.x + Instance.horizontalPadding),
                        0f,
                        -y * (cardSize.z + Instance.verticalPadding)
                    );

                    Card card = Instantiate(Instance.cardPrefab, position, Quaternion.identity, Instance.transform);
                    card.Initialize(CardState.Hidden, allFronts[index], back);
                    Instance.cards.Add(card);
                    index++;
                }
            }

            OnBoardCreated?.Invoke();
        }

        public static bool TryLoadData(GameSaveData data)
        {
            Sprite back = Instance.deckDataSO.GetRandomCardBack();
            Width = data.GridWidth;
            Height = data.GridHeight;
            OnSizeCalculated?.Invoke(Width, Height);

            foreach (var cardData in data.cards)
            {
                if (!Instance.deckDataSO.TryGetFront(cardData.cardId, out Sprite front))
                {
                    Debug.Log("SaveData Corrupted");

                    Instance.cards.ForEach(c => DestroyImmediate(c.gameObject));
                    Instance.cards.Clear();
                    return false;
                }

                Card card = Instantiate(Instance.cardPrefab, cardData.position, Quaternion.identity, Instance.transform);
                card.Initialize(CardState.Hidden, front, back);

                if (cardData.isMatched)
                {
                    card.ForceSetMatched();
                }

                Instance.cards.Add(card);
            }


            OnBoardCreated?.Invoke();
            return true;
        }
    }
}