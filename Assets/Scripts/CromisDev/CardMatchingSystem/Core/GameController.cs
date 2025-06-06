using System.Linq;
using System.Threading.Tasks;
using CromisDev.AudioSystem;
using CromisDev.SimplePanelSystem;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        private const float ON_GAME_COMPLETED_FEEDBACK_DELAY = 0.5f;
        private const int START_DELAY = 500;
        public static GameController Instance { get; private set; }
        [SerializeField] private GameSettingsSO gameSettingsSO;
        [SerializeField] private PanelManager panelManager;
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

            if (SaveCardGameManager.Exists())
            {
                LoadGame();
            }
            else
            {
                RequestNewGame();
            }
        }

        private void OnDestroy()
        {
            cardMatchHandler.StopListening();
            scoreController.StopListening();
        }

        private Vector2Int LoadSavedLayout(string key, Vector2Int defaultValue)
        {
            if (!PlayerPrefs.HasKey(key))
                return defaultValue;

            string saved = PlayerPrefs.GetString(key);
            string[] parts = saved.Split(',');

            if (parts.Length != 2)
                return defaultValue;

            if (int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                return new Vector2Int(x, y);

            return defaultValue;
        }

        public void RequestNewGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            Vector2Int gridSize = LoadSavedLayout(GridLayoutDropdownGenerator.LAYOUT_PREFKEY, Vector2Int.one * 2);
            BoardLayoutController.GenerateBoard(gridSize);
        }

        public async void LoadGame()
        {
            GameSaveData data = await SaveCardGameManager.LoadAsync();

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

        private void OnCardMatched(uint matchedCardId)
        {
            AudioManager.PlaySFX(AudioClipID.MATCHING, volume: 1f);
            bool isGameComplete = BoardLayoutController.Cards.All(card => card.IsMatched);

            if (isGameComplete)
            {
                ShouldInteract = false;

                Invoke(nameof(OnGameCompleted), ON_GAME_COMPLETED_FEEDBACK_DELAY);
            }
        }


        private void OnGameCompleted()
        {
            AudioManager.PlaySFX(AudioClipID.GAMEOVER, 1);
            panelManager.ShowPanel(PanelType.IN_GAME_GAMEOVER);
        }
        #region Testing methods
#if UNITY_EDITOR
        [ContextMenu(nameof(TriggerNewGame))] public void TriggerNewGame() => RequestNewGame();
        [ContextMenu(nameof(SaveGame))] public void SaveGame() => SaveCardGameManager.SaveGame();
        [ContextMenu(nameof(TriggerLoadGame))] public void TriggerLoadGame() => LoadGame();
#endif

        #endregion
    }
}
