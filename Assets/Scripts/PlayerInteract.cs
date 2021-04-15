using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    Rigidbody holdingObject;

    [SerializeField] GameObject pointerGraphic, grabGraphic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (holdingObject != null)
        {
            holdingObject.velocity = Vector3.zero;
            Vector3 holdPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            Vector3 toPos = holdPosition - holdingObject.transform.position;
            Vector3 forcePos = toPos / Time.fixedDeltaTime / holdingObject.GetComponent<Rigidbody>().mass;
            holdingObject.AddForce(forcePos, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = LayerMask.GetMask("Interactable","Key", "Grabbable"); //1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if (Input.GetButtonUp("Fire1") && holdingObject != null)
        {
            holdingObject = null;
            grabGraphic.SetActive(false);
        }

        if (holdingObject == null && grabGraphic != null && grabGraphic.activeSelf) grabGraphic.SetActive(false);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2f, layerMask))
        {
            if(pointerGraphic != null && holdingObject == null)
            // Em frente a um objeto interagivel 
            // e nao esta segurando nada.
            {
                pointerGraphic.SetActive(true);
            }
            else if (holdingObject != null && grabGraphic != null && !grabGraphic.activeSelf)
            {
                grabGraphic.SetActive(true);
                pointerGraphic?.SetActive(false);
            }

            // Vamos colocar um sprite no posicao e na normal do hit
            if (Input.GetButtonUp("Fire1"))
            {
                hit.collider.GetComponentInParent<Interactable>()?.Pressed();
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                                   
                if (holdingObject == null)
                {
                    holdingObject = hit.collider.gameObject.GetComponent<Rigidbody>();                     
                }
            }

        }
        else
        {
            if (pointerGraphic != null && pointerGraphic.activeSelf)
            {
                pointerGraphic.SetActive(false);
            }
        }
        /*else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/
    }
}
