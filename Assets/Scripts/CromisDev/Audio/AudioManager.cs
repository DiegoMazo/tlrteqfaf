using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace CromisDev.AudioSystem
{

    public enum AudioChannelType { Global, UI, BGM, SFX }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource BGMSource, SFXSource, UISource;
        [SerializeField] private AudioChannel[] channels;
        public static event Action OnInitialized;
        private const float MUTED_DB = -80f;

        private Dictionary<AudioChannelType, AudioChannel> channelDictionary;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            channelDictionary = channels.ToDictionary(c => c.channelType, c => c);
        }

        private void Start()
        {
            OnInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            OnInitialized = null;
        }

        private bool TryGetChannel(AudioChannelType type, out AudioChannel audioChannel)
        {
            if (channelDictionary != null && channelDictionary.TryGetValue(type, out audioChannel))
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"Channel {type} not found!");
                audioChannel = null;
                return false;
            }
        }

        public static void SetVolume(AudioChannelType channelType, float volume)
        {
            if (Instance.TryGetChannel(channelType, out AudioChannel channel))
            {
                channel.SetVolume(Instance.audioMixer, volume);
            }
        }

        public static void SetMute(AudioChannelType channelType, bool mute)
        {
            if (Instance.TryGetChannel(channelType, out AudioChannel channel))
            {
                channel.SetMute(Instance.audioMixer, mute, MUTED_DB);
            }
        }

        public static void SetGlobalVolume(float volume) => SetVolume(AudioChannelType.Global, volume);
        public static void SetUIVolume(float volume) => SetVolume(AudioChannelType.UI, volume);
        public static void SetMusicVolume(float volume) => SetVolume(AudioChannelType.BGM, volume);
        public static void SetSoundEffectsVolume(float volume) => SetVolume(AudioChannelType.SFX, volume);
        public static void SetGlobalMute(bool mute) => SetMute(AudioChannelType.Global, mute);
        public static void SetUIMute(bool mute) => SetMute(AudioChannelType.UI, mute);
        public static void SetMusicMute(bool mute) => SetMute(AudioChannelType.BGM, mute);
        public static void SetSoundEffectsMute(bool mute) => SetMute(AudioChannelType.SFX, mute);

        public static void PlayUISFX(AudioClip sfx) => Instance.UISource.PlayOneShot(sfx);
        public static void PlaySFX(AudioClip sfx) => Instance.SFXSource.PlayOneShot(sfx);

        public static void PlayBackgroundMusic(AudioClip clip)
        {
            Instance.BGMSource.clip = clip;
            Instance.BGMSource.Play();
        }

        public static async Task TransitionToAsync(AudioClip newClip, float fadeDuration)
        {
            float startVolume = Instance.BGMSource.volume;
            for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
            {
                Instance.BGMSource.volume = Mathf.Lerp(startVolume, 0.0f, t / fadeDuration);
                await Task.Yield();
            }
            Instance.BGMSource.Stop();
            Instance.BGMSource.volume = startVolume;
            Instance.BGMSource.clip = newClip;
            Instance.BGMSource.Play();
        }
    }

    [Serializable]
    public class AudioChannel
    {
        public AudioChannelType channelType;
        public string mixerParameter;
        public float maxVolume = 1f;
        private float lastVolume = 1f;
        public float LastVolume => lastVolume;

        public void SetVolume(AudioMixer mixer, float volume)
        {
            lastVolume = Mathf.Min(volume, maxVolume);
            mixer.SetFloat(mixerParameter, Mathf.Log10(lastVolume) * 20);
        }

        public void SetMute(AudioMixer mixer, bool mute, float mutedDb = -80f)
        {
            if (mute)
            {
                mixer.SetFloat(mixerParameter, mutedDb);
            }
            else
            {
                lastVolume = Mathf.Min(lastVolume, maxVolume);
                mixer.SetFloat(mixerParameter, Mathf.Log10(lastVolume) * 20);
            }
        }
    }

}




