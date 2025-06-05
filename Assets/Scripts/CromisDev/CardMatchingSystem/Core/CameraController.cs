using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cameraComponent;
        [SerializeField] private Vector2 cameraMargin = Vector2.one * 0.5f;
        private void Awake()
        {
            BoardLayoutController.OnBoardCreated += BoardLayoutController_OnBoardCreated;
        }

        private void BoardLayoutController_OnBoardCreated()
        {
            BoardLayoutController.OnBoardCreated -= BoardLayoutController_OnBoardCreated;
            AdjustCameraToBoard();
        }

        private void AdjustCameraToBoard()
        {
            float aspectRatio = (float)Screen.width / Screen.height;

            float neededHeight = BoardLayoutController.Height / 2f + cameraMargin.y;
            float neededWidth = BoardLayoutController.Width / (2f * aspectRatio) + cameraMargin.x;
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = Mathf.Max(neededHeight, neededWidth);
        }
    }
}
