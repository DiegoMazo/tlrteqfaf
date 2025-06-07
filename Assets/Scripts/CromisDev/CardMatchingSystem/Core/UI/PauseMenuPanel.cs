using CromisDev.CardMatchingSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CromisDev.SimplePanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PauseMenuPanel : Panel
    {
        private const string GAME_SAVED_TEXT = "Game Saved!";
        private const string CANT_SAVE_TEXT = "Can't save - game already completed!";

        [Header("UI Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button saveGameButton;
        [SerializeField] private Button returnToMenuButton;
        [SerializeField] private TextMeshProUGUI feedbackLabel;
        private void OnDestroy()
        {
            resumeButton.onClick.RemoveListener(OnResumeClicked);
            saveGameButton.onClick.RemoveListener(OnSaveGameClicked);
            returnToMenuButton.onClick.RemoveListener(OnReturnToMenuClicked);
        }

        public override void Initialize()
        {
            base.Initialize();

            resumeButton.onClick.AddListener(OnResumeClicked);
            saveGameButton.onClick.AddListener(OnSaveGameClicked);
            returnToMenuButton.onClick.AddListener(OnReturnToMenuClicked);
        }

        private void OnResumeClicked()
        {
            GameController.PanelManager.ShowPanel(PanelType.IN_GAME_MAIN);
            GameController.SetPauseState(false);
        }

        private void OnSaveGameClicked()
        {
            if (GameController.GameComplete)
            {
                feedbackLabel.color = Color.red;
                feedbackLabel.text = CANT_SAVE_TEXT;
            }
            else
            {

                SaveCardGameManager.SaveGame();
                feedbackLabel.color = Color.green;
                feedbackLabel.text = GAME_SAVED_TEXT;
            }

            saveGameButton.interactable = false;
        }

        private void OnReturnToMenuClicked()
        {
            SceneManager.LoadScene(GameConstants.Scenes.MENU);
        }

        public override void Show()
        {
            feedbackLabel.color = Color.white;
            feedbackLabel.text = string.Empty;
            saveGameButton.interactable = !GameController.GameComplete;
            base.Show();
        }
    }
}