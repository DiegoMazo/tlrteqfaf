using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        [SerializeField] private GameSettingsSO gameSettingsSO;
        public GameSettingsSO GameSettings => gameSettingsSO;
        public static bool ShouldInteract { get; private set; }

        private static readonly List<Card> activeCards = new();
        private static readonly HashSet<Card> pendingComparisons = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            BoardLayoutController.GenerateBoard();
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            if (gameSettingsSO.Data.InitialRevealCards)
            {
                ShouldInteract = false;
                await BoardLayoutController.RevelaCardsAsync(gameSettingsSO.Data.RevealTime);
            }

            ShouldInteract = true;
        }

        public static void OnCardFlipped(Card card)
        {
            if (pendingComparisons.Contains(card) || card.IsMatched) return;

            activeCards.Add(card);

            if (activeCards.Count >= 2)
            {
                List<Card> unmatched = activeCards.FindAll(c => !pendingComparisons.Contains(c) && !c.IsMatched);
                while (unmatched.Count >= 2)
                {
                    Card a = unmatched[0];
                    Card b = unmatched[1];
                    unmatched.RemoveRange(0, 2);
                    pendingComparisons.Add(a);
                    pendingComparisons.Add(b);
                    Instance.StartCoroutine(Instance.CheckMatch(a, b));
                }
            }
        }

        private IEnumerator CheckMatch(Card a, Card b)
        {
            yield return new WaitForSeconds(0.5f);

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
