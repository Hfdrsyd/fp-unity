using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float walkSpeed;
    [SerializeField] float normalSpeed = 6f;
    [SerializeField] float gravity = -10f;
    [SerializeField] [Range(0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockedCursor = true;

    float cameraPitch = 0f;
    float velocityY = 0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currenMouceDeltaVelocity = Vector2.zero;

    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpMultiplier;
    [SerializeField] float sprintingMultiplier;
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode crouchKey;
    [SerializeField] KeyCode sprintKey;
    bool isJumping;
    bool isSprinting;

    [SerializeField] float normalHeight, crouchHeight;

    // Start is called before the first frame update
    void Start()
    {
        if(lockedCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        Crouch();
        Sprint();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currenMouceDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if(controller.isGrounded)
        {
            velocityY = 0f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if(isSprinting == true)
        {
            velocity *= 2;
        }

        JumpInput();
    }

    void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    IEnumerator JumpEvent()
    {
        controller.slopeLimit = 90f;
        float timeInAir = 0f;

        do
        {
            float jumpForce  = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;

            yield return null;
        } while(!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = 45f;
        isJumping = false;
    }

    void Crouch()
    {
        if(Input.GetKeyDown(crouchKey) && !isJumping)
        {
            controller.height = crouchHeight;
            walkSpeed /= 2;
        }
        
        if(Input.GetKeyUp(crouchKey))
        {
            // controller.height = Mathf.Lerp(normalHeight, controller.height, 0.5f * Time.deltaTime);
            controller.height = normalHeight;
            walkSpeed = normalSpeed;
        }
    }

    void Sprint()
    {
        if(Input.GetKeyDown(sprintKey) && !isJumping)
        {
            isSprinting = true;
            walkSpeed *= 2;
        }

        if(Input.GetKeyUp(sprintKey))
        {
            isSprinting = false;
            walkSpeed = normalSpeed;
        }

    }
}
