using System;
using System.Threading.Tasks;
using CromisDev.AudioSystem;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "save_data";
        private const int START_DELAY = 500;
        public static GameController Instance { get; private set; }
        [SerializeField] private GameSettingsSO gameSettingsSO;
        private static GameSettingsData gameSettingsData;
        public static GameSettingsData Settings => gameSettingsData;
        public static bool ShouldInteract { get; private set; }
        private CardMatchHandler cardMatchHandler;
        private ScoreController scoreController;

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
        }

        private void OnDestroy()
        {
            cardMatchHandler.StopListening();
            scoreController.StopListening();
        }

        public void RequestNewGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            BoardLayoutController.GenerateBoard();
        }

        public async void LoadGame()
        {
            GameSaveData data = await SaveCardGameManager.LoadAsync(SAVE_FILE_NAME);

            ScoreController.LoadScore(data.score);
            ShouldInteract = data.shouldInteract;
            bool succed = BoardLayoutController.TryLoadData(data);

            if (!succed)
            {
                RequestNewGame();
            }
            else
            {
                StartGame();
            }
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            BoardLayoutController.OnBoardCreated -= BoardLayoutController_OnBoardCreated;

            await Task.Delay(START_DELAY);
            if (gameSettingsData.InitialRevealCards)
            {
                ShouldInteract = false;
                await BoardLayoutController.RevelaCardsAsync(gameSettingsData.RevealTime);
            }

            StartGame();
        }

        private void StartGame()
        {
            cardMatchHandler.StartListening();
            scoreController.StartListening();
            ShouldInteract = true;

            CardMatchHandler.OnCardMatched += OnCardMatched;
            CardMatchHandler.OnMismatched += OnMismatched;
        }

        private void OnMismatched()
        {
            AudioManager.PlaySFX(AudioClipID.MISMATCHING, 1);
        }

        private void OnCardMatched(uint _)
        {
            AudioManager.PlaySFX(AudioClipID.MATCHING, 1);
        }

        #region Testing methods
#if UNITY_EDITOR
        [ContextMenu(nameof(TriggerNewGame))] public void TriggerNewGame() => RequestNewGame();
        [ContextMenu(nameof(SaveGame))] public void SaveGame() => SaveCardGameManager.SaveGame(SAVE_FILE_NAME);
        [ContextMenu(nameof(TriggerLoadGame))] public void TriggerLoadGame() => LoadGame();
#endif

        #endregion
    }
}
