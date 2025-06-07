using UnityEngine;
using UnityEngine.UI;
using CromisDev.SimplePanelSystem;

namespace CromisDev.CardMatchingSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InGameMainPanel : Panel
    {
        [Header("UI Elements")]
        [SerializeField] private Button pauseButton;
        private void OnDestroy()
        {
            pauseButton.onClick.RemoveListener(OnPauseClicked);
        }

        public override void Initialize()
        {
            base.Initialize();
            pauseButton.onClick.AddListener(OnPauseClicked);
        }

        private void OnPauseClicked()
        {
            GameController.PanelManager.ShowPanel(PanelType.IN_GAME_PAUSE);
            GameController.SetPauseState(true);
        }
    }
}