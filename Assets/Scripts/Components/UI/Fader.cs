using System;
using UnityEngine;

namespace SQL_Quest.Components.UI
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private const string FADER_PATH = "UI/Fader/Fader";
        private static Fader _instance;

        private Action _fadeInCallback;
        private Action _fadeOutCallback;

        private static readonly int Fade = Animator.StringToHash("Fade");

        public static Fader Instance
        {
            get
            {
                if (_instance == null)
                { 
                    var prefab = Resources.Load<Fader>(FADER_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public bool IsFading { get; private set; }

        public void FadeIn(Action fadeInCallback)
        {
            if (IsFading)
                return;

            IsFading = true;
            _fadeInCallback = fadeInCallback;
            _animator.SetBool(Fade, true);
        }

        public void FadeOut(Action fadedOutCallBack)
        {
            if (IsFading)
                return;

            IsFading = true;
            _fadeInCallback = fadedOutCallBack;
            _animator.SetBool(Fade, false); 
        }

        private void FadeInAnimationOver()
        {
            _fadeInCallback?.Invoke();
            _fadeInCallback = null;
            IsFading = false;
        }
        private void FadeOutAnimationOver()
        {
            _fadeOutCallback?.Invoke();
            _fadeOutCallback = null;
            IsFading = false;
        }
    }
}