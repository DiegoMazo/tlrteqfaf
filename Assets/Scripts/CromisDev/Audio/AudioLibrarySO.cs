using System.Collections.Generic;
using UnityEngine;

namespace CromisDev.AudioSystem
{
    [CreateAssetMenu(menuName = "Audio/Audio Library")]
    public class AudioLibrarySO : ScriptableObject
    {
        [System.Serializable]
        public struct AudioEntry
        {
            public string id;
            public AudioClip clip;
        }

        [SerializeField] private List<AudioEntry> entries;

        private Dictionary<string, AudioClip> clipMap;

        public void Initialize()
        {
            clipMap = new Dictionary<string, AudioClip>();
            foreach (var entry in entries)
            {
                if (!clipMap.ContainsKey(entry.id))
                    clipMap[entry.id] = entry.clip;
            }
        }

        public AudioClip GetClip(string id)
        {
            if (clipMap.TryGetValue(id, out var clip))
                return clip;

            Debug.LogWarning($"Audio clip not found for id: {id}");
            return null;
        }
    }
}
