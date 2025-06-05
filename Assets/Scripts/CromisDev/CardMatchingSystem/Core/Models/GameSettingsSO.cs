using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Card Game/Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private GameSettingsData gameSettingsData;

        public GameSettingsData Data => gameSettingsData;
    }
}