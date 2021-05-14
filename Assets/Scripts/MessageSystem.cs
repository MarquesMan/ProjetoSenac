using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{

    [SerializeField]
    TMPro.TextMeshProUGUI textMessage;

    [SerializeField]
    private CanvasGroup panelPointer;

    [SerializeField]
    float maxMessageTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        ClearText();
    }

    private void SetText(string newText)
    {
        textMessage?.SetText(newText);
    }

    private void ClearText()
    {
        textMessage?.SetText("");
    }

    public void SetMessageText(string newText)
    {
        ClearText();
        StopAllCoroutines();
        SetText(newText);
        StartCoroutine("RemoveMessage");
    }

    public void SetMessageText(string newText, float newMessageTime)
    {
        ClearText();
        StopAllCoroutines();
        SetText(newText);
        StartCoroutine("RemoveMessageWithNewTime", newMessageTime);
    }

    IEnumerator RemoveMessage()
    {
        yield return new WaitForSeconds(maxMessageTime);
        ClearText();
    }

    IEnumerator RemoveMessageWithNewTime(float newMessageTime)
    {
        yield return new WaitForSeconds(newMessageTime);
        ClearText();
    }

}
