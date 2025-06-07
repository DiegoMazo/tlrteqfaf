using CromisDev.CardMatchingSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CromisDev.SimplePanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MainMenuPanel : Panel
    {
        [Header("UI Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private PanelManager panelManager;

        public override void Initialize()
        {
            base.Initialize();

            loadGameButton.gameObject.SetActive(SaveCardGameManager.Exists());

            newGameButton.onClick.AddListener(OnNewGameClicked);
            loadGameButton.onClick.AddListener(OnLoadGameClicked);
        }

        private void OnNewGameClicked()
        {
            if (SaveCardGameManager.Exists())
            {
                SaveCardGameManager.Delete();
            }

            panelManager.ShowPanel(PanelType.NEW_GAME);
        }

        private void OnLoadGameClicked()
        {
            SceneManager.LoadScene(GameConstants.Scenes.GAME);
        }

        private void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnNewGameClicked);
            loadGameButton.onClick.RemoveListener(OnLoadGameClicked);
        }
    }
}
