using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    [Header("Jumping")]
    public float JumpForce;
    public float JumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYscale;
    bool crouching;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        crouching = false;
        startYscale = transform.localScale.y;
    }



    // Update is called once per frame
    void Update()
    {
        //Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f  + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        stateHandler();
        //handle drag
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;


    }

    private void FixedUpdate() 
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When to Jump
        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }

        //Start Crouching
        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            crouching = true;
        }

        //Stop Crouching
        if (Input.GetKeyUp(crouchKey))
        {
             transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
             crouching = false;
        }
    }

    private void stateHandler()
    {
        //Mode - Crouching
        if (Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            playerHeight = 0.5f;
        }
        else
        {
            playerHeight = 2f;
        }

        //Mode - Sprinting
        if(grounded && Input.GetKeyDown(sprintKey) && !crouching)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //Mode - Air
        else 
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        //Calculate movement direciton
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
             rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //Reset y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(crouching)
        {
            rb.AddForce(transform.up * JumpForce * 0.5f, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
        }

    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}

