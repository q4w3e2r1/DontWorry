using UnityEngine;

namespace SQL_Quest.Components.Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;

        private void Start()
        {
            if (_backgroundMusic != null)
                PlayMusic(_backgroundMusic);
        }

        public void LoadSettings()
        {
            AudioHandler.Instance.LoadSettings();
        }

        public void PlaySound(AudioClip clip)
        {
            AudioHandler.Instance.PlaySound(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            AudioHandler.Instance.PlayMusic(clip);
        }
    }
}
