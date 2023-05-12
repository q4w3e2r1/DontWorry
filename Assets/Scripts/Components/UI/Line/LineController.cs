using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SQL_Quest.Components.UI.Line
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _linesCount;
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private UnityEvent _onCreateLine;
        [SerializeField] private UnityEvent _onDestroyLine;

        public void CreateLine()
        {
            var text = GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

            Instantiate(_linePrefab, transform);
            _onCreateLine?.Invoke();

            text.transform.SetAsLastSibling();
        }

        public void DestroyLine()
        {
            Destroy(GetComponentsInChildren<Line>()[^1].gameObject);
            _onDestroyLine?.Invoke();
        }
    }
}