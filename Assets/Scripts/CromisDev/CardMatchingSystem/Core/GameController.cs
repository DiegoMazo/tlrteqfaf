using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private GameSettingsSO gameSettingsSO;
        public GameSettingsSO GameSettings => gameSettingsSO;

        public static bool ShouldInteract { get; private set; }

        private CardMatchHandler cardMatchHandler;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            cardMatchHandler = new CardMatchHandler(this);
            StartGame();
        }

        private void OnDestroy()
        {
            Card.OnCardFlipped -= OnCardFlipped;
        }

        public void StartGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            BoardLayoutController.GenerateBoard();
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            BoardLayoutController.OnBoardCreated -= BoardLayoutController_OnBoardCreated;

            if (gameSettingsSO.Data.InitialRevealCards)
            {
                ShouldInteract = false;
                await BoardLayoutController.RevelaCardsAsync(gameSettingsSO.Data.RevealTime);
            }

            Card.OnCardFlipped += OnCardFlipped;
            ShouldInteract = true;
        }

        public static void OnCardFlipped(Card card)
        {
            Instance.cardMatchHandler.OnCardFlipped(card);
        }
    }
}
