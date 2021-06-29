using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Credits : MonoBehaviour
{
    [SerializeField]
    float yFinish = 500f, creditsTime = 60f, waitForCredits = 5f;
    
    float ystart = 0f;

    [SerializeField]
    bool startOnAwake = false, shouldRepeat = false;

    [SerializeField]
    LeanTweenType leanType;

    [SerializeField]
    UnityEvent onCompleteEvents;

    private void Awake()
    {
        ystart = -73.15f;
    }

    // Start is called before the first frame update
    void Start()
    {        
        if (startOnAwake) StartFromBeginning();
    }

    public void StartFromBeginning()
    {
        LeanTween.cancel(gameObject);
        gameObject.LeanCancel();
        var tempPos = gameObject.transform.position;
        tempPos.y = ystart;
        gameObject.transform.position = tempPos;
        this.gameObject.LeanMoveY(yFinish, creditsTime).setEase(leanType).setRepeat(shouldRepeat? -1 : 0).setOnComplete(onCompleteFunction);
    }

    private void onCompleteFunction()
    {
        StartCoroutine(WaitCoroutine());
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitForCredits);
        onCompleteEvents.Invoke();
    }

    public void OnDisable()
    {
        LeanTween.cancel(gameObject);
        gameObject.LeanCancel();
    }

    /* public void StartFromWhereStopped()
    {
        this.gameObject.LeanMoveY(yFinish, 
           (gameObject.transform.position.y - ystart)/(yFinish - ystart)*creditsTime
        ).setEase(leanType).setRepeat(shouldRepeat ? -1 : 0);
    }*/

}
