using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalMessage : MonoBehaviour
{

    public float waitFirstTime = 1f;

    public TMPro.TextMeshPro[] messageLine;
    public float[] waitForMessageLine;
    private int lastCleared = 0;

    public UnityEvent afterMessageEvents;

    // Start is called before the first frame update
    void Start()
    {
        if (messageLine.Length != waitForMessageLine.Length) return;

        StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        yield return new WaitForSeconds(waitFirstTime);

        for (int i = 0; i < messageLine.Length; ++i)
        {
            
            var textMeshGameObject = messageLine[i];
            
            if (textMeshGameObject == null) continue;

            textMeshGameObject.gameObject.SetActive(true);

            var textMeshcolor = textMeshGameObject.color;
            textMeshcolor.a = 0f;
            textMeshGameObject.color = textMeshcolor;
        }

        for (int i = 0; i < messageLine.Length; ++i)
        {
            var waitTime = waitForMessageLine[i];

            if (messageLine[i] == null)
            {
                for (int j = lastCleared; j < i; ++j)
                {
                    var textMeshGameObject = messageLine[j];
                    if(textMeshGameObject != null)
                        LeanTween.value(textMeshGameObject.gameObject, a => textMeshGameObject.color = new Color(textMeshGameObject.color.r, textMeshGameObject.color.g, textMeshGameObject.color.b, a), 1, 0, 1f);
                }
                lastCleared = i;
            }
            else
            {
                var textMeshGameObject = messageLine[i];
                LeanTween.value(textMeshGameObject.gameObject, a => textMeshGameObject.color = new Color(textMeshGameObject.color.r, textMeshGameObject.color.g, textMeshGameObject.color.b, a), 0, 1, 1f);
            }
            yield return new WaitForSeconds(waitTime);
        }

        afterMessageEvents.Invoke();
    }
   
}
