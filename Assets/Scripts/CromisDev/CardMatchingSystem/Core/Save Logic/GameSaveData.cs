using System;
using System.Collections.Generic;

namespace CromisDev.CardMatchingSystem
{
    [Serializable]
    public class GameSaveData
    {
        public uint score;
        public bool shouldInteract;
        public List<SerializableCardData> cards = new();
        public DateTime saveTime;

        public float GridWidth;
        public float GridHeight;
    }
}
