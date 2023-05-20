using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Line
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private UnityEvent _onCreateLine;
        [SerializeField] private UnityEvent _onDestroyLine;
        [SerializeField] private int _maxLines;

        public void CreateLine()
        {
            if (GetComponentsInChildren<Line>().Length - 1 == _maxLines)
                return;

            var text = GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

            Instantiate(_linePrefab, transform);
            _onCreateLine?.Invoke();

            text.transform.SetAsLastSibling();
        }

        public void DestroyLine()
        {
            if (GetComponentsInChildren<Line>().Length == 1)
                return;

            Destroy(GetComponentsInChildren<Line>()[^1].gameObject);
            _onDestroyLine?.Invoke();
        }
    }
}