namespace CromisDev.CardMatchingSystem
{
    [System.Serializable]
    public class GameSettingsData
    {
        public bool InitialRevealCards = true; // Optionally show cards to player at the beginning for memorizing
        public float RevealTime;
        public uint PointsPerCardMatch;
        public float MaxComboInterval = 3;
        public uint ComboPoints = 5;
    }
}