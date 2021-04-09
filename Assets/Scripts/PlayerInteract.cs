using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    Rigidbody holdingObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (holdingObject != null)
        {

            //holdingObject.GetComponent<Rigidbody>().isKinematic = true;
            //holdingObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            holdingObject.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            holdingObject.velocity = Vector3.zero;
            //Vector3 holdPosition = transform.position + Camera.main.transform.forward * 2f;
            //Vector3 toPos = holdPosition - transform.position;
            //Vector3 forcePos = toPos / Time.fixedDeltaTime / holdingObject.GetComponent<Rigidbody>().mass;

            //holdingObject.GetComponent<Rigidbody>().AddForce(forcePos, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = LayerMask.GetMask("Interactable","Key"); //1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
  
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, 2f, layerMask))
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (holdingObject != null)
                {
                    //holdingObject.isKinematic = false;
                    holdingObject = null;

                }
                else
                {
                    hit.collider.GetComponentInParent<Interactable>()?.Pressed();
                }
            }else if (Input.GetButtonDown("Fire1"))
            {

                if (holdingObject == null && hit.collider.gameObject.CompareTag("Key"))
                {
                    holdingObject = hit.collider.gameObject.GetComponent<Rigidbody>();
                    //holdingObject.isKinematic = true;
                }

            }
        }
        /*else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/
    }
}
