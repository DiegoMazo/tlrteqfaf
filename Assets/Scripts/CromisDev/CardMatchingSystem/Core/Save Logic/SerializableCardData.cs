using System;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    [Serializable]
    public class SerializableCardData
    {
        public string cardId;
        public CardState state;
        public Vector3 position;
        public bool isMatched;
    }
}
