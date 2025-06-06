using CromisDev.CardMatchingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CromisDev.SimplePanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MainMenuPanel : Panel
    {
        [Header("UI Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;

        public override void Initialize()
        {
            base.Initialize();

            loadGameButton.gameObject.SetActive(SaveCardGameManager.Exists());

            newGameButton.onClick.AddListener(OnNewGameClicked);
            loadGameButton.onClick.AddListener(OnLoadGameClicked);
        }

        private void OnNewGameClicked()
        {
        }

        private void OnLoadGameClicked()
        {
        }

        private void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnNewGameClicked);
            loadGameButton.onClick.RemoveListener(OnLoadGameClicked);
        }
    }
}
