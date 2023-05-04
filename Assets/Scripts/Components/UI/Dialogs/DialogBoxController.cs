using System.Collections;
using UnityEngine;

namespace SQL_Quest.Components.UI.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private float _textSpeed = 0.1f;
        [Space]
        [SerializeField] protected DialogContent _content;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private DialogData _data;
        private int _currentSentence;
        private Coroutine _typingRoutine;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentSentence = 0;
            CurrentContent.Name.text = CurrentSentence.Name;
            CurrentContent.Text.text = string.Empty;
            CurrentContent.TrySetIcon(CurrentSentence.Icon);

            _container.SetActive(true);
            _animator.SetBool(IsOpen, true);
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty;
            var sentence = CurrentSentence;
            CurrentContent.Name.text = sentence.Name;
            CurrentContent.TrySetIcon(sentence.Icon);

            foreach (var letter in sentence.Value)
            {
                CurrentContent.Text.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        protected virtual DialogContent CurrentContent => _content;

        public void OnSkip()
        {
            if (_typingRoutine == null)
                return;

            StopTypeAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentence].Value;
        }

        public void OnContinue()
        {
            if (_typingRoutine != null)
            {
                OnSkip();
                return;
            }

            StopTypeAnimation();
            _currentSentence++;

            var isDialogComplete = _currentSentence >= _data.Sentences.Length;
            if (isDialogComplete)
                HideDialogBox();
            else
                OnStartDialogAnimation();
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private void OnCloseAnimationComplete()
        {

        }
    }
}