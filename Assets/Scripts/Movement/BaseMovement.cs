using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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


    [Header("QuickBoost")]
    public float QBForce;
    public float QBForceMult;
    public float QBCoolDown;
    public float QBMemoryTime;
    public float airmultiplier;
    public float dashCount;
    public float dashDirection;
    [HideInInspector] public float DashCountStored;
    private BaseMovement baseMovement;
    public KeyCode QBKey = KeyCode.LeftShift;
    public KeyCode downDash = KeyCode.LeftControl;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    [HideInInspector] public float lastQB;
    [SerializeField] private float speedLockTimer;
    [SerializeField] private float boostChainFallOff;
    [SerializeField] private PlayerCamera rotation;
    private float speedLockTimerStored;

    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    
    // 0 is none, 1 is forward, 2 is back, 3 is left, 4 is right, 5 is up, 6 is down. the numbers are inconsequental aside from allowing a cooldownskip by chainboosting. 

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        DashCountStored = dashCount;
        speedLockTimerStored = speedLockTimer;
        speedLockTimer = 0;
    }

    private void Update()
    {

        InputRegister();
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
            rb.linearDamping = 0;
        }

        speedLockTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        MoveDirection();    
    }
    private void InputRegister()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && dashCount > 0 && onGround == false && lastQB != 5)
        {
            Jump();
            dashCount -= 1;
            Debug.Log("UpBoost");
        }
        else if (Input.GetKey(jumpKey) && onGround == true && lastQB != 5)
        {
            Jump();
            Debug.Log("Jump");
        }

        if (Input.GetKey(downDash) && dashCount > 0 && onGround == false)
        {
            if (lastQB !=  6)
            {
                DashDown();
                dashCount -= 1;
                Invoke(nameof(ResetQB), QBCoolDown);
                Invoke(nameof(QBMemory), QBMemoryTime);
                lastQB = 6;
                Debug.Log("DownBoost");
            }
           
        }
        else if (Input.GetKey(downDash) && dashCount > 0 && onGround == true && lastQB != 6)
        {
            lastQB = 6;
            Debug.Log("YOU ARE DASHING DOWN WHILE PLANTED FIRMLY ON IT WHAT ARE YOU DOING");
        }

        if (Input.GetKey(QBKey) && dashCount > 0 && Input.GetKey(backwardKey))
        {
            if(lastQB != 2)
            {
                dashDirection = -1;
                ForwardDash();
            }
            
        }
        else if (Input.GetKey(QBKey) && dashCount > 0 && Input.GetKey(leftKey))
        {
            if (lastQB != 3)
            {
                dashDirection = -1;
                SideDash();
            }
        }
        else if (Input.GetKey(QBKey) && dashCount > 0 && Input.GetKey(rightKey))
        {
            if (lastQB != 4)
            {
                dashDirection = 1;
                SideDash();
            }
            
        }
        else if (Input.GetKey(QBKey) && dashCount > 0 && Input.GetKey(forwardKey))
        {
            if (lastQB != 1)
            {
                dashDirection = 1;
                ForwardDash();
            }

        }
    }

    private void DashDown()
    {
        rb.AddForce((transform.up * -1) * QBForce, ForceMode.Impulse);
    }

    private void SideDash()
    {
        rb.AddForce((transform.right * dashDirection) * (QBForce * QBForceMult), ForceMode.Impulse);
        dashCount -= 1;

        if (dashDirection == -1)
            lastQB = 3;
        else if (dashDirection == 1)
            lastQB = 4;

        Invoke(nameof(ResetQB), QBCoolDown);
        Invoke(nameof(QBMemory), QBMemoryTime);
        Debug.Log("sideboost");
    }

    private void ForwardDash()
    {
        rb.AddForce((transform.forward * dashDirection) * (QBForce * QBForceMult), ForceMode.Impulse);
        dashCount -= 1;
        lastQB = 2;
        if (dashDirection == -1)
            lastQB = 2;
        else if (dashDirection == 1)
            lastQB = 1;
        Invoke(nameof(ResetQB), QBCoolDown);
        Invoke(nameof(QBMemory), QBMemoryTime);
        Debug.Log("forward boost");
    }

    private void MoveDirection()
    {
        MovementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (onGround)
        {
            rb.AddForce(MovementDirection.normalized * moveSpeed * 10, ForceMode.Force);
            dashCount = DashCountStored;
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


    private void Jump()
    {
        if(speedLockTimer > 0)
        {
            rb.AddForce(transform.up * (QBForce / boostChainFallOff), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * QBForce, ForceMode.Impulse);
        }
            lastQB = 5;
        speedLockTimer = speedLockTimerStored;
        Invoke(nameof(ResetQB), QBCoolDown);
        Invoke(nameof(QBMemory), QBMemoryTime);
        lastQB = 5;

    }
    public void QBMemory()
    {
        lastQB = 0;
    }
    public void ResetQB()
    {
        dashCount += 1;
    }
}
