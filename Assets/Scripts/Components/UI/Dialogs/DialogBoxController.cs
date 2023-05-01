
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SQL_Quest.Components.UI.Dialogs
{
	public class DialogBoxController : MonoBehaviour
	{
		[SerializeField] private Text _text;
		[SerializeField] private GameObject _container;
		[SerializeField] private Animator _animator;
		[Space]
		[SerializeField] private float _textSpeed = 0.1f;

		private static readonly int IsOpen = Animator.StringToHash("IsOpen");

		private DialogData _data;
		private int _currentSentence;
		private Coroutine _typingRoutine;

		public void ShowDialog(DialogData data)
		{
			_data = data;
			_currentSentence = 0;
			_text.text = string.Empty;

			_container.SetActive(true);
			_animator.SetBool(IsOpen, true);
		}

		private IEnumerator TypeDialogText()
		{ 
			_text.text = string.Empty;
			var sentence = _data.Sentences[_currentSentence];

			foreach (var letter in sentence)
			{
				_text.text += letter;
				yield return new WaitForSeconds(_textSpeed);
			}

			_typingRoutine = null;
		}

		public void OnSkip()
		{
			if (_typingRoutine == null)
				return;

			StopTypeAnimation();
			_text.text = _data.Sentences[_currentSentence];
		}

		public void OnContinue()
		{
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

		private void OnStartDialogAnimation()
		{ 
			_typingRoutine = StartCoroutine(TypeDialogText());
		}

		private void OnCloseAnimationComplete()
		{ 
			
		}
    }
}