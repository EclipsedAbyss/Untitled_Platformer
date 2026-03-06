using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [Header("movement")]
    public float moveSpeed;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    public float grounded;
    Vector3 MovementDirection;
    public float groundDrag;

    public float jumpforce;
    public float jumpcooldown;
    public float airmultiplier;
    bool readyToJump;

    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        readyToJump = true;
    }

    private void Update()
    {
        myInput();
        SpeedControl();


        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }

    private void FixedUpdate()
    {
        moveDirection();    
    }
    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && readyToJump)
        {
            readyToJump = false;
            jump();
            Invoke(nameof(ResetJump), jumpcooldown);
        }
    }

    private void moveDirection()
    {
        MovementDirection = orientation.forward *verticalInput + orientation.right * horizontalInput;

        if (onGround)
        {
            rb.AddForce(MovementDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else if (!onGround)
        {
            rb.AddForce(MovementDirection.normalized * moveSpeed * 10 * airmultiplier, ForceMode.Force);
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
         Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);

            
        }
    }


    private void jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
