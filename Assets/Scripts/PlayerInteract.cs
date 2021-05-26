using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    GameObject holdingObject;

    [SerializeField] GameObject pointerGraphic, grabGraphic;
    [SerializeField] TMPro.TextMeshProUGUI objectDescription;
    [SerializeField] bool startWithFlashlight = false;

    private int importantLayerMask, secondLayerMask;
    private PlayerController playerController;
    private Light flashLight;
    private GameObject currentGameObjectDescription;
    private bool hasFlashlight = false;

    // Start is called before the first frame update
    void Start()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        importantLayerMask = LayerMask.GetMask("Key", "Grabbable"); //1 << 8;
        secondLayerMask = LayerMask.GetMask("Interactable", "Door"); //1 << 8;
        playerController = GetComponent<PlayerController>();
        flashLight = GetComponentInChildren<Light>();
        flashLight.enabled = hasFlashlight;
        hasFlashlight = startWithFlashlight;

    }

    public void SetFlashLight(bool EnableFlashlight)
    {
        hasFlashlight = EnableFlashlight;
        flashLight.enabled = hasFlashlight;
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
            rigidBody.AddForce(Vector3.ClampMagnitude(forcePos,10), ForceMode.VelocityChange);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        // layerMask = ~layerMask;

        if (Input.GetButtonUp("Flashlight") && hasFlashlight) flashLight.enabled = !flashLight.enabled;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if ((Input.GetButtonUp("Fire1") && holdingObject != null))
        {
            holdingObject = null;
            grabGraphic.SetActive(false);
        }else if ((Input.GetButtonUp("Fire2") && holdingObject != null))
        {
            holdingObject.GetComponent<Rigidbody>()?.AddForce(Camera.main.transform.forward * 10f, ForceMode.Impulse );
            holdingObject = null;
            grabGraphic.SetActive(false);
        }

        if (!playerController.gamePaused && Input.GetButtonDown("Cancel"))
        {
            playerController.TogglePause();
        }

        if (holdingObject == null && grabGraphic != null && grabGraphic.activeSelf) grabGraphic.SetActive(false);

        // Traca o raio procurando por chaves ou objetos de interesse
        // Procura por objetos 
        bool hitSomething = Physics.Raycast( //
            Camera.main.transform.position, Camera.main.transform.forward,
            out hit, 2f, importantLayerMask + secondLayerMask);

        /*if (!hitSomething)
            hitSomething = Physics.Raycast(
                Camera.main.transform.position, Camera.main.transform.forward,
                out hit, 2f, secondLayerMask);*/

        if (hitSomething) // Acertou alguma coisa
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
