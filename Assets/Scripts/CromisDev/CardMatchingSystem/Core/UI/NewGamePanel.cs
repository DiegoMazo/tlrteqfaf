using CromisDev.CardMatchingSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CromisDev.SimplePanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class NewGamePanel : Panel
    {
        [Header("UI Buttons")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button backButton;
        [SerializeField] private PanelManager panelManager;

        public override void Initialize()
        {
            base.Initialize();

            startGameButton.onClick.AddListener(OnStartGameClicked);
            backButton.onClick.AddListener(OnBackClicked);
        }

        private void OnStartGameClicked()
        {
            SceneManager.LoadScene(GameConstants.Scenes.GAME);
        }

        private void OnBackClicked()
        {
            panelManager.ShowPanel(PanelType.MENU_MAIN);
        }

        private void OnDestroy()
        {
            startGameButton.onClick.RemoveListener(OnStartGameClicked);
            backButton.onClick.RemoveListener(OnBackClicked);
        }
    }
}