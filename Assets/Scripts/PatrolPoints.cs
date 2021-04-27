using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Transform previousTransform = null;
        foreach(Transform childTransform in this.GetComponentsInChildren<Transform>())
        {
            if (childTransform.Equals(transform)) continue;

            if(previousTransform != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(previousTransform.position, childTransform.position);
                Gizmos.color = Color.green;
            }else
                Gizmos.color = Color.red;

            Gizmos.DrawSphere(childTransform.position, 0.5f);

            previousTransform = childTransform;
        }

        var firstTransform = transform.GetChild(0);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(previousTransform.position, firstTransform.position);

    }
}
