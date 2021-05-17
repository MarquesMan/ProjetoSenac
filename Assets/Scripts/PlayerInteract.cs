using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    GameObject holdingObject;

    [SerializeField] GameObject pointerGraphic, grabGraphic;
    [SerializeField] TMPro.TextMeshProUGUI objectDescription;

    private int layerMask;
    private PlayerController playerController;
    private GameObject currentGameObjectDescription;

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = LayerMask.GetMask("Interactable", "Key", "Grabbable", "Door"); //1 << 8;
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (holdingObject != null && !holdingObject.CompareTag("Trap"))
        {
            var rigidBody = holdingObject.GetComponent<Rigidbody>();
            
            if (rigidBody == null) return;

            rigidBody.velocity = Vector3.zero;
            Vector3 holdPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            Vector3 toPos = holdPosition - rigidBody.transform.position;
            Vector3 forcePos = toPos / Time.fixedDeltaTime / rigidBody.GetComponent<Rigidbody>().mass;
            rigidBody.AddForce(forcePos, ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if ((Input.GetButtonUp("Fire1") && holdingObject != null))
        {
            holdingObject = null;
            grabGraphic.SetActive(false);
        }

        if (!playerController.gamePaused && Input.GetButtonDown("Cancel"))
        {
            playerController.TogglePause();
        }

        if (holdingObject == null && grabGraphic != null && grabGraphic.activeSelf) grabGraphic.SetActive(false);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2f, layerMask))
        {

            if (currentGameObjectDescription == null || !currentGameObjectDescription.Equals(hit.collider.gameObject))
            {
                currentGameObjectDescription = hit.collider.gameObject;
                var interactable = hit.collider.gameObject.GetComponentInParent<Interactable>();
                if (interactable)
                    objectDescription.SetText(interactable.itemDescription);
            }

            if (pointerGraphic != null && holdingObject == null)
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

            if (Input.GetButtonUp("Fire1"))
            {
                hit.collider.GetComponentInParent<Interactable>()?.Pressed();
                hit.collider.gameObject.GetComponent<Trap>()?.ResetTime();
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                string objectTag = hit.collider.gameObject.tag;

                switch (objectTag)
                {
                    case "Trap":
                        hit.collider.gameObject.GetComponent<Trap>()?.ResetTime();
                        holdingObject = hit.collider.gameObject;
                        break;

                    case "Key":
                        hit.collider.gameObject.GetComponent<Key>()?.PlayerPickup(transform);
                        break;

                    default:
                        if (holdingObject == null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabbable")) 
                            holdingObject = hit.collider.gameObject;
                        break;
                };
                                
            }
            else if (Input.GetButton("Fire1"))
            {
                if(holdingObject != null && holdingObject.CompareTag("Trap"))
                    hit.collider.gameObject.GetComponent<Trap>()?.AddTime(Time.deltaTime);
            }

        }
        else
        { // Nao esta olhando para o objeto de interacao
            currentGameObjectDescription = null;
            objectDescription?.SetText("");
            if (pointerGraphic != null && pointerGraphic.activeSelf)
            {
                pointerGraphic.SetActive(false);
            }
            if(holdingObject != null && holdingObject.CompareTag("Trap"))
            {
                holdingObject.GetComponent<Trap>()?.ResetTime();
                holdingObject = null;
            } 
        }
        /*else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/
    }
}
