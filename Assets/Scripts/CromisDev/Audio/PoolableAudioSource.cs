using UnityEngine;
using CromisDev.Pooling;

namespace CromisDev.AudioSystem
{
    public class PoolableAudioSource : PoolableBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void Play(AudioClip clip, float volume = 1f)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Invoke(nameof(Hide), clip.length);
        }
    }
}
