using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCastOnWater : MonoBehaviour
{
    //Fake cookie example by Morgan Adams - cast a spotlight cookie onto reflective water or other non-cookie-compatible flat surface
    //(Put this script on any object--doesn't matter. Put the cookie and its "holder" anywhere at scene root.)

    //Use shader Particles/Additive for cookie; adjust alpha to set max cookie strength

    [SerializeField] GameObject cookie; //The quad for the fake cookie (standard quad rotated 90 degrees on X to lie flat)

    Light lightSpot;
    Transform lightTransform;
    Vector3 lightPosHelper;
    private float lightRange;
    LayerMask waterLayer;

    void Start()
    {
        waterLayer = LayerMask.GetMask("Water");
        lightSpot = GetComponent<Light>();
        if (lightSpot != null && cookie != null)
        {
            lightTransform = GetComponent<Light>().transform;
            lightRange = GetComponent<Light>().range;
            lightPosHelper = cookie.transform.position;
        }
        else
            enabled = false;

    }

    /*
    private void OnDrawGizmos()
    {
        if(lightTransform)
            Gizmos.DrawLine(lightTransform.position, lightTransform.position + lightTransform.forward * lightRange);
    }*/

    void Update()
    {
        if (!lightSpot.enabled) return;

        RaycastHit raycastHit;
        


        if (Physics.Raycast(lightTransform.position, lightTransform.forward, out raycastHit, lightRange/2, waterLayer))
        {
            lightPosHelper.x = raycastHit.point.x;
            lightPosHelper.z = raycastHit.point.z;
            cookie.transform.position = lightPosHelper;
        }
        else
        {
            var projectForward = lightTransform.position + lightTransform.forward * lightRange / 2;
            lightPosHelper.x = projectForward.x;
            lightPosHelper.z = projectForward.z;
            cookie.transform.position = lightPosHelper;
        }
        

        /*transform.position = Vector3(lamp.position.x, waterHeight + waterOffset, lamp.position.z);
        transform.eulerAngles.y = lamp.eulerAngles.y;

        cookie.transform.localPosition.z = -(Mathf.Abs(lamp.position.y - waterHeight) / Mathf.Tan(Mathf.PI - lamp.eulerAngles.x * Mathf.PI / 180));
        var distanceFactor = Mathf.Abs(lamp.position.y - waterHeight) / 2;
        var angleFactor = cookie.transform.localPosition.z / 2;
        cookie.transform.localScale = Vector3(distanceFactor + angleFactor, .5 + distanceFactor + angleFactor, distanceFactor + angleFactor);

        cookieColor.a = cookieAlpha * (1 - Mathf.Min(1, distanceFactor / cookieFalloff)); //Alpha falloff
        cookie.renderer.material.SetColor("_TintColor", cookieColor);*/
    }
}
