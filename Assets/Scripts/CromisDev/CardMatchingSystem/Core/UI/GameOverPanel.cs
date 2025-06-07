using CromisDev.SimplePanelSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CromisDev.CardMatchingSystem
{
    public class GameOverPanel : Panel
    {
        [SerializeField] private Button newGameBtn;
        [SerializeField] private Button returnToMenuBtn;

        public override void Initialize()
        {
            base.Initialize();

            newGameBtn?.onClick.AddListener(OnNewGameClicked);
            returnToMenuBtn?.onClick.AddListener(OnReturnToMenuClicked);
        }

        private void OnDestroy()
        {
            newGameBtn?.onClick.RemoveListener(OnNewGameClicked);
            returnToMenuBtn?.onClick.RemoveListener(OnReturnToMenuClicked);
        }

        private void OnNewGameClicked()
        {
            SaveCardGameManager.Delete();
            SceneManager.LoadScene(GameConstants.Scenes.GAME);
        }

        private void OnReturnToMenuClicked()
        {
            SceneManager.LoadScene(GameConstants.Scenes.MENU);
        }
    }
}