using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToogleEvents : MonoBehaviour
{
    [SerializeField] UnityEvent onEvents, offEvents;

    public bool toogleOn = false;

    // Start is called before the first frame update
    public void FireEventsOnToogle()
    {
        toogleOn = !toogleOn;

        if (toogleOn)
            onEvents.Invoke();
        else
            offEvents.Invoke();
    }
}
