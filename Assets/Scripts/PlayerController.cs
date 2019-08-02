using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GlobalVariables globals;
    [SerializeField] private Transform cameraParent;
    public Gun equippedGun;
    public AnimationCurve jumpFallOff;

    private Animator anim;

    CharacterController characterController;
    private bool isJumping;
    Vector3 verticalMovement, horizontalMovement;
    private float moveSpeed;

    private float xRot, yRot;
    Quaternion newPlayerRotation;
    Quaternion newRotation;

    void Start()
    {

        // Init
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        SetMoveSpeed(globals.playerMoveSpeed);

        // Give player some ammo to start with
        SingleBullet ammo = new SingleBullet();
        //ExplodingBullet ammo = new ExplodingBullet();
        ammo.Init(globals);
        equippedGun.SetAmmo(ammo, 30);

        // Hide mouse cursor
        Cursor.visible = false;
    }

    

    void Update()
    {
        PlayerMovement();
        PlayerRotation();
        PlayerFire();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis("Horizontal") * moveSpeed;
        float vertInput = Input.GetAxis("Vertical") * moveSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;
        // Clamp to prevent extra movement speed when both using both inputs 
        Vector3 movement = Vector3.ClampMagnitude(forwardMovement + rightMovement, moveSpeed);

        characterController.SimpleMove(movement);

        JumpInput();

    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        // Set slope limit so we can jump ontop of things
        characterController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        isJumping = true;

        do
        {
            // Evaluate how much force we are adding according to our time in the air
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            characterController.Move(Vector3.up * jumpForce * globals.jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;

            // Repeat while we are still in the air and didnt hit something above, like a roof
        } while (!characterController.isGrounded && characterController.collisionFlags != CollisionFlags.Above);

        // Reset slop limit to not be able to walk up too steep slopes
        characterController.slopeLimit = 45.0f;
        isJumping = false;
    }

    private void PlayerRotation()
    {
        xRot += Input.GetAxisRaw("Mouse X") * globals.mouseXRotationSpeed;

        // Invert yRotation and clamp angle to prevent rotating too far
        yRot -= Input.GetAxisRaw("Mouse Y") * globals.mouseYRotationSpeed;
        yRot = Mathf.Clamp(yRot, -90, 90);

        // Rotate y-component of player object only
        newPlayerRotation = Quaternion.Euler(0, xRot, 0);
        transform.rotation = newPlayerRotation;

        // Rotate x- and y-component of camera and gun
        newRotation = Quaternion.Euler(yRot, xRot, 0);
        cameraParent.rotation = newRotation;
    }

    private void PlayerFire()
    {
        // Fire when mouse button 0 (left click) is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            if (equippedGun == null)
            {
                Debug.LogError("No gun attached to player");
                return;
            }

            equippedGun.Fire();
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }




}
