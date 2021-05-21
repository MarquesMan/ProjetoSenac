using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 moveAmountByAxis = Vector3.zero;
    [SerializeField] float timeToMove = 1f;
    private float lastTimeMoved = -1;
    [SerializeField] bool boomerang = false;

    private void Start()
    {
        lastTimeMoved = Time.time;
    }

    public void MoveAction()
    {
        var now = Time.time;
        if (now <= lastTimeMoved + timeToMove) return;

        gameObject.LeanMoveLocal(transform.localPosition + moveAmountByAxis, timeToMove);
        
        if (boomerang)
            moveAmountByAxis *= -1;

        lastTimeMoved = now;
    }
}
