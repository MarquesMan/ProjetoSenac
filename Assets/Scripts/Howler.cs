using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Howler : MonoBehaviour
{
    [SerializeField]
    string messageText = "", secondMessageText = "";

    [SerializeField]
    float messageTime = 3f;

    [SerializeField]
    private float timeBeforeShout = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutine(WaitBeforeShout());
    }

    IEnumerator WaitBeforeShout()
    {
        var currentTime = Time.time;
        yield return new WaitForSeconds(timeBeforeShout);
        FindObjectOfType<MessageSystem>().SetMessageText(messageText, messageTime, currentTime);

        if (!secondMessageText.Equals(""))
        {
            yield return new WaitForSeconds(messageTime);
            FindObjectOfType<MessageSystem>().SetMessageText(secondMessageText, messageTime, currentTime);
        }

        Destroy(gameObject);
    }


}
