using System;
using UnityEngine;
using CromisDev.Pooling;

namespace CromisDev.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("SFX Pooling")]
        [SerializeField] private PoolableAudioSource sfxPrefab;
        [SerializeField] private AudioSource uiAudioSource;
        [SerializeField] private AudioLibrarySO audioLibrarySO;
        [SerializeField] private Transform sfxParent;
        [SerializeField] private uint initialPoolSize = 5;

        private ObjectPooler<PoolableAudioSource> sfxPool;

        public static event Action OnInitialized;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
            audioLibrarySO.Initialize();
            sfxPool = new ObjectPooler<PoolableAudioSource>(sfxPrefab, initialPoolSize, sfxParent);
        }

        private void Start()
        {
            OnInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            OnInitialized = null;
        }

        public static void PlaySFX(AudioClip clip, float volume = 1f)
        {
            var source = Instance.sfxPool.GetPooledObject(true);
            if (source == null)
            {
                throw new InvalidOperationException("No audio source available from the pool.");
            }

            source.Show();
            source.Play(clip, volume);
        }

        public static void PlaySFX(string sfxID, float volume = 1f)
        {
            if (!Instance.audioLibrarySO.TryGetClip(sfxID, out AudioClip clip))
            {
                throw new ArgumentException($"SFX ID '{sfxID}' not found in AudioLibrary.");
            }
            PlaySFX(clip, volume);
        }

        public static void PlayUISFX(string sfxID, float volume = 1f)
        {
            if (!Instance.audioLibrarySO.TryGetClip(sfxID, out AudioClip clip))
            {
                throw new ArgumentException($"UI Sound ID '{sfxID}' not found in AudioLibrary.");
            }
            PlayUISFX(clip, volume);
        }

        public static void PlayUISFX(AudioClip clip, float volume = 1f)
        {
            Instance.uiAudioSource.volume = volume;
            Instance.uiAudioSource.PlayOneShot(clip);
        }
    }
}



