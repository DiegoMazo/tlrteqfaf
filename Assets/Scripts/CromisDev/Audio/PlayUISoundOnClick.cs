using UnityEngine;
using UnityEngine.UI;

namespace CromisDev.AudioSystem
{
    [RequireComponent(typeof(Button))]
    public class PlayUISoundOnClick : MonoBehaviour
    {
        [SerializeField] private string soundID;
        [SerializeField] private float volume = 1f;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlaySound);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            try
            {
                AudioManager.PlayUISFX(soundID, volume);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[PlayUISoundOnClick] {ex.Message}");
            }
        }
    }
}
