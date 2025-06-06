using System.Collections.Generic;
using UnityEngine;

namespace CromisDev.AudioSystem
{
    [CreateAssetMenu(menuName = "Audio/Audio Library")]
    public class AudioLibrarySO : ScriptableObject
    {
        [SerializeField] private List<AudioClip> entries;
        private Dictionary<string, AudioClip> clipMap;

        public void Initialize()
        {
            clipMap = new Dictionary<string, AudioClip>();
            foreach (var entry in entries)
            {
                if (!clipMap.ContainsKey(entry.name))
                    clipMap[entry.name] = entry;
            }
        }

        public bool TryGetClip(string id, out AudioClip clip)
        {
            if (clipMap.TryGetValue(id, out  clip))
                return true;

            Debug.LogWarning($"Audio clip not found for id: {id}");
            return false;
        }
    }
}
