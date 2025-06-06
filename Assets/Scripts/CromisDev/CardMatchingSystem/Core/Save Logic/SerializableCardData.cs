using System;
using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    [Serializable]
    public class SerializableCardData
    {
        public string cardId;
        public Vector3 position;
        public bool isMatched;
    }
}
