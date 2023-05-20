using SQL_Quest.Creatures.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private float _textSpeed = 0.05f;
        [Space]
        [SerializeField] protected DialogContent _left;
        [SerializeField] protected DialogContent _right;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];
        protected virtual DialogContent CurrentContent => CurrentSentence.Side == Side.Left ? _left : _right;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private static readonly string _playerNamePlacer = "%PlayerName%";
        private string _playerName;

        private DialogData _data;
        private int _currentSentence;
        private Coroutine _typingRoutine;

        private UnityEvent _onFinishDialog;

        

        private void Start()
        => _playerName = PlayerDataHandler.PlayerData.Name;

        public void ShowDialog(DialogData data, UnityEvent onStart, UnityEvent onFinish)
        {
            onStart?.Invoke();
            _onFinishDialog = onFinish;

            _data = data;
            _currentSentence = 0;
            CurrentContent.Name.text = CurrentSentence.Name == _playerNamePlacer ? _playerName : CurrentSentence.Name;
            CurrentContent.Text.text = "";
            CurrentContent.TrySetIcon(CurrentSentence.Icon);

            _container.SetActive(true);
            _animator.SetBool(IsOpen, true);
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty;
            CurrentContent.Name.text = CurrentSentence.Name == _playerNamePlacer ? _playerName : CurrentSentence.Name;
            CurrentContent.TrySetIcon(CurrentSentence.Icon);

            var text = CurrentSentence.Value.Replace(_playerNamePlacer, _playerName);
            foreach (var letter in text)
            {
                CurrentContent.Text.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine == null)
                return;

            StopTypeAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentence].Value.Replace(_playerNamePlacer, _playerName);
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
                OnCompleteStartDialogAnimation();
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

        protected virtual void OnBeginStartDialogAnimation()
        {
            _left.gameObject.SetActive(CurrentSentence.Side == Side.Left);
            _right.gameObject.SetActive(CurrentSentence.Side == Side.Right);
        }

        protected virtual void OnCompleteStartDialogAnimation()
        {
            OnBeginStartDialogAnimation();
            Cursor.visible = true;
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        protected virtual void OnCompleteCloseAnimation()
        {
            _onFinishDialog?.Invoke();
            Cursor.visible = false;
        }
    }
}