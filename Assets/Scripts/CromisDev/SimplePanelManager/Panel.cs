using UnityEngine;

namespace CromisDev.SimplePanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private bool startHidden = true;

        private CanvasGroup canvasGroup;

        private void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public virtual void Initialize()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (startHidden)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            SetActive(false);
        }
    }
}
