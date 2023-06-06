using UnityEngine;

namespace SQL_Quest.Components.Audio
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;

        private static AudioHandler _instance;
        private const string AUDIOHANDLER_PATH = "Audio/AudioHandler";

        public static AudioHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<AudioHandler>(AUDIOHANDLER_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        private void Start()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey("SoundVolume") && PlayerPrefs.HasKey("MusicVolume"))
            {
                _soundAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
                _musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
            }
        }

        public void PlaySound(AudioClip clip)
        {
            _soundAudioSource.Stop();
            _soundAudioSource.clip = clip;
            _soundAudioSource.Play();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (_musicAudioSource.clip == clip)
                return;
            _musicAudioSource.Stop();
            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }
    }
}
