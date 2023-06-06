using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource SoundAudioSource;
        public AudioSource MusicAudioSource;

        private const string AUDIOCONTROLLER_PATH = "UI/AudioController";
        private static AudioController _instance;

        public static AudioController Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<AudioController>(AUDIOCONTROLLER_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
}
