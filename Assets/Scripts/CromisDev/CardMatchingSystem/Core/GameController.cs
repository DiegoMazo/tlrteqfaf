using CromisDev.AudioSystem;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        private const string SAVE_FILE_NAME = "save_data";
        public static GameController Instance { get; private set; }
        [SerializeField] private GameSettingsSO gameSettingsSO;
        [SerializeField] private AudioLibrarySO audioLibrarySO;
        private static GameSettingsData gameSettingsData;
        public static GameSettingsData Settings => gameSettingsData;
        public static bool ShouldInteract { get; private set; }
        public static AudioLibrarySO AudioLibrary => Instance.audioLibrarySO;
        private CardMatchHandler cardMatchHandler;
        private ScoreController scoreController;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            gameSettingsData = gameSettingsSO.Data;
            audioLibrarySO.Initialize();
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

        public void StartNewGame()
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
                StartNewGame();
            }
            else
            {
                StartGame();
            }
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            BoardLayoutController.OnBoardCreated -= BoardLayoutController_OnBoardCreated;

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
        }

        #region Testing methods
#if UNITY_EDITOR
        [ContextMenu(nameof(TriggerNewGame))] public void TriggerNewGame() => StartNewGame();
        [ContextMenu(nameof(SaveGame))] public void SaveGame() => SaveCardGameManager.SaveGame(SAVE_FILE_NAME);
        [ContextMenu(nameof(TriggerLoadGame))] public void TriggerLoadGame() => LoadGame();
#endif

        #endregion
    }
}
