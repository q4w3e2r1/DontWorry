using Scripts.Extensions;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _helpButton;
    [SerializeField] private UnityEngine.UI.Button _completeButton;
    [Space]
    [SerializeField] private GameObject _messagePrefab;
    [Space]
    [SerializeField] private Message[] _messages;


    private void Start()
    {
        Send(_messages[0]);
    }

    public void CheckMessage(string message)
    {
        Debug.Log(message);
        var firstMessage = _messages.Where(message => !message.IsSent).First();
        if (firstMessage.ConditionsForSending != message)
            return;

        Send(firstMessage);
    }

    private void Send(Message message)
    {
        var messageGO = Instantiate(_messagePrefab);
        messageGO.GetComponentInChildren<TextMeshProUGUI>().text = message.Text;
        messageGO.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform);
        messageGO.transform.localScale = Vector3.one;

        message.OnSending?.Invoke();
        message.IsSent = true;

        messageGO.GetComponentInParent<ScrollRect>().ScrollToBottom(messageGO);
    }

    public void ChangeButtonToCompleteButton()
    {
        _helpButton.gameObject.SetActive(false);
        _completeButton.gameObject.SetActive(true);
    }
}
