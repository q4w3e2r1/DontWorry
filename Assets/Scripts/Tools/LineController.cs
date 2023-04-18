using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LineController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _linesCount;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private UnityEvent _onCreateLine;
    [SerializeField] private UnityEvent _onDestroyLine;

    public void CreateLine()
    {
        var text = GetComponentsInChildren<TextMeshProUGUI>()[^1].gameObject;

        var line = Instantiate(_linePrefab, transform);
        _onCreateLine?.Invoke();

        text.transform.SetAsLastSibling();
    }

    public void DestroyLine()
    {
        Destroy(GetComponentsInChildren<Line>()[^1].gameObject);
        _onDestroyLine?.Invoke();
    }
}
