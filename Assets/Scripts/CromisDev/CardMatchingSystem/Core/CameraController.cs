using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cameraComponent;
        [SerializeField] private Vector2 marginPercentage = new Vector2(0.1f, 0.1f); // 10% de margen

        private void Awake()
        {
            BoardLayoutController.OnSizeCalculated += OnSizeCalculated;
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 1f;
        }

        private void OnDestroy()
        {
            BoardLayoutController.OnSizeCalculated -= OnSizeCalculated;
        }

        private void OnSizeCalculated(float boardWidth, float boardHeight)
        {
            Vector2 absoluteMargins = new(
                boardWidth * marginPercentage.x,
                boardHeight * marginPercentage.y);

            float requiredViewportWidth = boardWidth + absoluteMargins.x * 2;
            float requiredViewportHeight = boardHeight + absoluteMargins.y * 2;

            float horizontalSize = requiredViewportWidth / (2f * cameraComponent.aspect);
            float verticalSize = requiredViewportHeight / 2f;

            cameraComponent.orthographicSize = Mathf.Max(horizontalSize, verticalSize);
        }
    }
}