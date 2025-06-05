using System.Collections.Generic;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class BoardLayoutController : MonoBehaviour
    {
        [Header("Card and Layout Settings")]
        [SerializeField] private Card cardPrefab;
        [SerializeField] private Vector2Int gridSize = new(4, 4);
        [SerializeField] private float horizontalPadding = 0.2f;
        [SerializeField] private float verticalPadding = 0.2f;

        private readonly List<Card> cards = new();

        private void Start()
        {
            GenerateBoard();
        }

        public void GenerateBoard()
        {
            Vector3 cardSize = cardPrefab.GetSize;

            float totalWidth = gridSize.x * cardSize.x + (gridSize.x - 1) * horizontalPadding;
            float totalHeight = gridSize.y * cardSize.z + (gridSize.y - 1) * verticalPadding;


            Vector3 origin = new(-totalWidth / 2f + cardSize.x / 2f, 0f, totalHeight / 2f - cardSize.z / 2f);

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector3 position = origin + new Vector3(
                        x * (cardSize.x + horizontalPadding),
                        0f,
                        -y * (cardSize.z + verticalPadding)
                    );

                    Card card = Instantiate(cardPrefab, position, Quaternion.identity, transform);
                    cards.Add(card);
                }
            }

        }
    }
}
