using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameSettingsSO gameSettingsSO;

        public GameSettingsSO GameSettings => gameSettingsSO;
        public static bool ShouldInteract { get; private set; }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
            BoardLayoutController.GenerateBoard();
        }

        private async void BoardLayoutController_OnBoardCreated()
        {
            if (gameSettingsSO.Data.InitialRevealCards)
            {
                ShouldInteract = false;
                await BoardLayoutController.RevelaCardsAsync(gameSettingsSO.Data.RevealTime);
            }

            ShouldInteract = true;
        }
    }
}