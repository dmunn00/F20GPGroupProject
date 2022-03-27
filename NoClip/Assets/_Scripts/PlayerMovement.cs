using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject PlayerCam;

    [Header("Animation Settings")]
    public Animator PlayerAnimations;

    [Header("Gun Settings")]
    private Gun GunScript;
    private bool checkDropGun;
    public LayerMask dropGun;

    [Header("Glider Settings")]
    public LayerMask gliderPickup;
    public LayerMask gliderLand;
    public bool isGliding = false;
    public bool checkGliding;
    public bool finishGliding;

    [Header("WallRun Settings")]
    public Collider wallCheck; 

    
    [Header("Climb Settings")]
    public Transform climbCheck;
    public LayerMask climbMask;
    public LayerMask finishClimbingCheck;

    public Transform groundCheck;
    public LayerMask groundMask;

    public float velocity = 3f;
    public float baseVelocity = 3f;
    public float maxVelocity = 10f;
    public float CrouchingMaxVelocity = 2f;
    public float acceleration = 0.05f;

    public float jumpHeight = 10f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    public float mass = 5.0F; // defines the character mass
    Vector3 impact = Vector3.zero;


    public float mouseSens = 100f;
    float xRotation = 0f;

    //Wall Running Checks
    WallRunning wallRunning;
    public bool IsWallRunning() => wallRunning.IsWallRunning();

    public float timeSinceJump = 0.0f;
    public bool hasWallJumped = false;



    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1, 1);
    private float currMaxVelocity;

    public bool isGrounded;
    bool isClimbing;
    bool finishClimbing;
    bool noShift = true;
    Vector3 verticalVelocity;
    public Vector3 move;

    //Pause 
    private bool paused = false;

    [Header("HUD Settings")]
    public Text playerStatusText;

    public bool isFinished = false;

    public Image finishOneStar, finishTwoStar, finishThreeStar;
    public Text finishTime;
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
    private void Start()
    {
        isFinished = false;
        wallRunning = GetComponent<WallRunning>();
        currMaxVelocity = maxVelocity; //Set Value
        Cursor.lockState = CursorLockMode.Locked;
        PlayerCam.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        GunScript = GetComponent<Gun>();
        PlayerAnimations = GetComponent<Animator>();
        
        playerStatusText = playerStatusText.GetComponent<Text>();
        finishOneStar = finishOneStar.GetComponent<Image>();
        finishTwoStar = finishTwoStar.GetComponent<Image>();
        finishThreeStar = finishThreeStar.GetComponent<Image>();
        finishTime = finishTime.GetComponent<Text>();
        finishOneStar.enabled = false;
        finishTwoStar.enabled = false;
        finishThreeStar.enabled = false;
        finishTime.enabled = false;
    }

    public void finish(){
        isFinished = true;
    }

    void Update()
    {

        // //Animations
        // if(Input.GetButtonDown("Vertical")){
        //     PlayerAnimations.SetTrigger("Running");
        // }
        // if(Input.GetButtonUp("Vertical")){
        //     PlayerAnimations.SetTrigger("NotRunning");
        // }

        

        if(isFinished == false){

            //Pause
            if (Input.GetKey(KeyCode.Escape))
            {
                if (paused)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    paused = false;
                }
                else if (!paused)
                {
                    Cursor.lockState = CursorLockMode.None;
                    paused = true;
                }
            }

            //Drop Guns
            checkDropGun = Physics.CheckSphere(groundCheck.position, groundDistance, dropGun);
            if (checkDropGun == true)
            {
                PlayerAnimations.SetTrigger("DropGun");
                PlayerAnimations.ResetTrigger("PickUpPistol");
                PlayerAnimations.ResetTrigger("PickUpRifle");
                GunScript.DropGuns();
                checkDropGun = false;
            }


            //Gliding
            checkGliding = Physics.CheckSphere(groundCheck.position, groundDistance, gliderPickup);
            finishGliding = Physics.CheckSphere(groundCheck.position, groundDistance, gliderLand);
            if (checkGliding == true)
            {
                PlayerAnimations.SetTrigger("PickUpGlider");
                isGliding = true;
                verticalVelocity.y = Mathf.Sqrt((jumpHeight + 2.7f) * -2f * gravity);
            }
            if (finishGliding == true)
            {
                isGliding = false;
            }
            if (isGliding == true)
            {
                verticalVelocity.y += (-3f) * Time.deltaTime;
                playerStatusText.text = "GLIDING";
            }

            isClimbing = Physics.CheckSphere(climbCheck.position, groundDistance, climbMask);
            //Wall Climbing
            if (isClimbing == true)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity.y = Mathf.Sqrt((jumpHeight + 2) * -2f * gravity);
                }
                playerStatusText.text = "CLIMBING";
            }
            finishClimbing = Physics.CheckSphere(groundCheck.position, groundDistance, finishClimbingCheck);
            if (finishClimbing)
            {
                Debug.Log("Here");
                controller.Move((transform.forward * 5) * 0.5f * Time.deltaTime);
            }

            //Vetical Movement handling
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2f;
            }

            //Jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                if (isGrounded)
                {
                    playerStatusText.text = "JUMPING";
                    verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }

            //Wall Running
            if (wallRunning.IsWallRunning())
            {
                if (hasWallJumped == false)
                {
                    verticalVelocity.y = 0;

                }
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    hasWallJumped = true;
                }
                playerStatusText.text = "RUNNING";
            }
            if (hasWallJumped == true)
            {
                timeSinceJump += Time.deltaTime;
                if (timeSinceJump > 0.8f)
                {
                    hasWallJumped = false;
                    timeSinceJump = 0f;
                }
            }


            if (isGliding == false)
            {
                verticalVelocity.y += gravity * Time.deltaTime;
            }

            controller.Move(verticalVelocity * Time.deltaTime);

            //Movement handling
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            move = transform.right * x + transform.forward * z;

            if (Input.GetKeyDown("left shift")) { noShift = false; }
            if (Input.GetKeyUp("left shift")) { noShift = true; }

            if (((x > 0.01 || x < -0.01) || (z > 0.01 || z < -0.01)) && (noShift))
            {
                if (velocity < currMaxVelocity)
                { velocity += acceleration; }
                if (velocity > currMaxVelocity)
                { velocity -= acceleration; }

            }
            else { velocity = baseVelocity; }

            //Crouching
            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
            {
                AddImpact(move, 300f);
                transform.localScale = crouchScale;
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                if (isGrounded)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                }
                currMaxVelocity = CrouchingMaxVelocity;

                // apply the impact force:
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                transform.localScale = playerScale;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                currMaxVelocity = maxVelocity;
            }

            if (impact.magnitude > 0.2F) controller.Move(impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

            if (isGrounded)
            {
                if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
                {
                    playerStatusText.text = "RUNNING";
                }
            }
            // if(isGrounded && Input.GetButtonDown("Vertical") == false && Input.GetButtonDown("Vertical") == false && Input.GetButtonDown("Jump") == false){
            //     playerStatusText.text = "STOPPED";
            // }


            //move the player
            controller.Move(move * velocity * Time.deltaTime);

            //Mouse Lock toggle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Debug.Log("Lock");
                }
                else if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("Unlock");
                }
            }
            float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            PlayerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

}
