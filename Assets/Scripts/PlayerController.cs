using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_DefaultFootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_WaterFootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_GrassFootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip[] m_WoodFootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    private Dictionary<string, AudioClip[]> dictOfStepSounds;
    private string currentFlootMaterial = "Default";
    [SerializeField] private float feetCheckDistance = 1f;

    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private float flashlightLag = 7.5f;


    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;
    private bool m_isSwiming = false;
    public bool playerStuck = false;

    private bool gameOver = false;
    public bool gamePaused = false;
    [SerializeField]
    private GameObject gameOverScreen, normalScreen, pauseScreen;

    private float Stamina = 100.0f;
    [SerializeField]
    private float MaxStamina = 100.0f;

    [SerializeField]
    private float jumpStaminaDrain = 20f;

    [SerializeField]
    private RectTransform StaminaBar;
    private Vector3 staminaBarScale = Vector3.one;
    private UnityEngine.UI.RawImage staminaBarSprite;

    private const float StaminaDecreasePerFrame = 10.0f;
    private const float StaminaIncreasePerFrame = 5.0f;
    

    private bool isTired = false;
    private float crouchOffset = 0f; // Vai de 0 a 1
    public float crouchSpeed = 10f; // Velocidade com que a garota abaixa
    private Vector3 cameraLocalPosition; // Guarda posicao local da camera do personagem
    private float defaultColliderHeight; // Guarda altura padrao da capsula de colisao
    private Vector3 crouchBoxExtents;

    private Transform lightTransform;
    private Vector3 oldForward;

    public void DeclareGameOver()
    {
        gameOver = true;
        Cursor.SetCursor(null, new Vector2(0.5f, 0.5f), CursorMode.Auto);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void startGameOverScreen()
    {
        gameOverScreen?.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void TogglePause()
    {
        if (gameOver) return;

        gamePaused = !gamePaused;
        normalScreen.SetActive(!gamePaused);
        pauseScreen.SetActive(gamePaused);
        Cursor.SetCursor(null, new Vector2(0.5f, 0.5f), CursorMode.Auto);

        if (gamePaused)
        {                       
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = gamePaused;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red + Color.yellow;
        Gizmos.DrawLine(leftFoot.position, leftFoot.position + Vector3.down * feetCheckDistance);
        Gizmos.DrawLine(rightFoot.position, rightFoot.position + Vector3.down * feetCheckDistance);
    }

    private void CheckFloorMaterial()
    {
        RaycastHit leftHit, rightHit, choosenHit;
        bool b_leftHit, b_rightHit;

        var layerMask = ~LayerMask.GetMask("Player");
        
        b_leftHit = Physics.Raycast(leftFoot.position, Vector3.down, out leftHit, feetCheckDistance, layerMask);
        b_rightHit = Physics.Raycast(leftFoot.position, Vector3.down, out rightHit, feetCheckDistance, layerMask);

        if( b_leftHit || b_rightHit)
        {
            if (b_leftHit && b_rightHit) // Caso mais dificil
            {
                choosenHit = leftHit.distance <= rightHit.distance ? leftHit : rightHit;
            }
            else
            {
                choosenHit = b_leftHit ? leftHit : rightHit;
            }

            currentFlootMaterial = choosenHit.collider.sharedMaterial is null ? "Default" : choosenHit.collider.sharedMaterial.name;

        }
        else
        {
            // Caso trivial onde nao se toca o chao
            //currentFlootMaterial = "Default";
        }

        m_isSwiming = currentFlootMaterial.Equals("Water");
    }

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);


        dictOfStepSounds = new Dictionary<string, AudioClip[]>()
        {
            { "Default" , m_DefaultFootstepSounds },
            { "Water", m_WaterFootstepSounds },
            { "Grass", m_GrassFootstepSounds },
            { "Wood", m_WoodFootstepSounds}
        };

        cameraLocalPosition = m_Camera.transform.localPosition;
        defaultColliderHeight = m_CharacterController.height;
        crouchBoxExtents = new Vector3(m_CharacterController.radius, defaultColliderHeight/2, m_CharacterController.radius);


        if (StaminaBar) staminaBarSprite = StaminaBar.gameObject.GetComponent<UnityEngine.UI.RawImage>();

        lightTransform = GetComponentInChildren<Light>().transform;

    }


    // Update is called once per frame
    private void Update()
    {
        if (gameOver || gamePaused) return;

        CheckFloorMaterial();

        RotateView();

        if (lightTransform != null) RotateLight();

        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        CheckCrouch();

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());

            if (!m_isSwiming) 
                PlayLandingSound();

            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }

    private void RotateLight()
    {
        lightTransform.forward = Vector3.Slerp(oldForward, m_Camera.transform.forward, Time.deltaTime * flashlightLag);
        /*Quaternion.Slerp(
            lightTransform.rotation,
            m_Camera.transform.rotation,
             Time.deltaTime * 0.025f);*/
        /*lightTransform.localRotation =  Quaternion.FromToRotation(
        lightTransform.forward, m_Camera.transform.forward
        );*/
        // lightTransform.localEulerAngles = Vector3.Slerp(lightTransform.localEulerAngles, m_Camera.transform.localEulerAngles , Time.deltaTime * 2f);
    }

    private void CheckCrouch()
    {
        bool crouchPressed = Input.GetButton("Crouch");

        bool underCover; //= Physics.BoxCast(m_Camera.transform.position, crouchBoxExtents, transform.up, Quaternion.identity, defaultColliderHeight, LayerMask.GetMask("Furniture"));

        Collider[] triste = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * defaultColliderHeight, m_CharacterController.radius*1.25f,LayerMask.GetMask("Furniture"));

        Debug.Log(triste.Length);

        if (crouchPressed || triste.Length > 0)
            crouchOffset += crouchSpeed * Time.deltaTime;
        else
            crouchOffset -= crouchSpeed * Time.deltaTime;

        crouchOffset = Mathf.Clamp(crouchOffset, 0, 1);

        m_CharacterController.height = defaultColliderHeight - 0.8f * crouchOffset;
        m_CharacterController.center = Vector3.up*-0.5f*crouchOffset;

    }


    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }


    private void FixedUpdate()
    {
        if (gameOver || playerStuck || gamePaused) return;

        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                            m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		
        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;
	

        if (m_CharacterController.isGrounded )
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump && !isTired)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
                Stamina = Mathf.Clamp(Stamina - jumpStaminaDrain, 0.0f, MaxStamina);

                isTired = (Stamina == 0);
                if (isTired) staminaBarSprite.color = Color.red;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }


    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                            Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        var currentDict = dictOfStepSounds.ContainsKey(currentFlootMaterial)? dictOfStepSounds[currentFlootMaterial] : dictOfStepSounds["Default"];
        int n = Random.Range(1, currentDict.Length);

        m_AudioSource.clip = currentDict[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        currentDict[n] = currentDict[0];
        currentDict[0] = m_AudioSource.clip;
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                    (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        Debug.Log(m_CharacterController.velocity.magnitude);
        m_Camera.transform.localPosition = newCameraPosition - Vector3.up*crouchOffset*1f;
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        if (m_IsWalking || Stamina == 0 || isTired)
        {
            speed = m_WalkSpeed;
            Stamina = Mathf.Clamp(Stamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            if (isTired && Stamina == MaxStamina)
            {
                isTired = false;
                m_Jump = false;
                staminaBarSprite.color = Color.white;
            }
        }
        else
        {
            speed = m_RunSpeed;
            Stamina = Mathf.Clamp(Stamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            isTired = (Stamina == 0);
            if (isTired) staminaBarSprite.color = Color.red;
        }

        if (StaminaBar)
        {
            staminaBarScale.x =  Stamina/MaxStamina;
            StaminaBar.localScale = staminaBarScale;
        }

        // Player esta nadando
        
        if (m_isSwiming) speed *= 0.8f; 

        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        oldForward = lightTransform.forward;
        m_MouseLook.LookRotation(transform, m_Camera.transform);        
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        

        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
