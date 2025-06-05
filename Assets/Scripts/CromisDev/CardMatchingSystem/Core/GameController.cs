using System;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        [SerializeField] private GameSettingsSO gameSettingsSO;

        private static GameSettingsData gameSettingsData;
        public static GameSettingsData Settings => gameSettingsData;

        public static bool ShouldInteract { get; private set; }

        private CardMatchHandler cardMatchHandler;
        private ScoreController scoreController;

        public static event Action<uint> OnCardMatched;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            gameSettingsData = gameSettingsSO.Data;
        }

        private void Start()
        {
            cardMatchHandler = new CardMatchHandler(this);
            scoreController = new();

            StartGame();
        }

        private void OnDestroy()
        {
            cardMatchHandler.StopListening();
            scoreController.StopListening();
        }

        public void StartGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            BoardLayoutController.GenerateBoard();
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            BoardLayoutController.OnBoardCreated -= BoardLayoutController_OnBoardCreated;

            if (gameSettingsData.InitialRevealCards)
            {
                ShouldInteract = false;
                await BoardLayoutController.RevelaCardsAsync(gameSettingsData.RevealTime);
            }

            cardMatchHandler.StartListening();
            scoreController.StartListening();
            ShouldInteract = true;
        }

        public static void HandleOnCardMatched()
        {
            OnCardMatched?.Invoke(gameSettingsData.PointsPerCardMatch);
        }
    }
}
