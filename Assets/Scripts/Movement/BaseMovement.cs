using System;
using TMPro;
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



    [Header("QuickBoost")] // all of these fields are for the multi-directional dash (includes jumping and downdashing) this was an absolute headache to get working initially.
    public float QBForce;
    public float QBForceMult;
    public float airmultiplier;
    public float dashCount;
    public float downDashBounce;
    public float downDashBounceTime;
    public float downDashBounceForce;
    public float downDashPrepKick;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;// inputs stored fr detecting direction dash is being performed in.
    [HideInInspector] public float dashDirection;
    [HideInInspector] public float DashCountStored;
    [HideInInspector] public float lastQB; // 0 is none, 1 is forward, 2 is back, 3 is left, 4 is right. the numbers are inconsequental aside from allowing a cooldown bypass by chainboosting(this is intended). 
    [HideInInspector] public float lastVerticalDash;// 0 is none, 1 is up, 2 is down. same use as above but for vertical movement to avoid penalizing players due to bad code execution.
    [HideInInspector] public bool downDashGrounded;


    [Header("QuickBoost Delay Values")]
    public float QBCoolDown;
    public float QBRecharge;
    public float QBRechargeDelay;
    public float verticalCoolDown;
    public float QBMemoryTime;
    [SerializeField] private float speedLockTimer;
    [SerializeField] private float boostChainFallOff;
    private float speedLockTimerStored;
    private float downDashBounceTimeStored;
    private float QBRechargeDelayStored;
    

    [Header("keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    Rigidbody rb;
    public KeyCode QBKey = KeyCode.LeftShift;
    public KeyCode downDash = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        DashCountStored = dashCount;
        speedLockTimerStored = speedLockTimer;
        downDashBounceTimeStored = downDashBounceTime;
        downDashBounceTime = 0;
        speedLockTimer = 0;
        QBRechargeDelayStored = QBRecharge;
        QBRechargeDelayStored = QBRechargeDelay;
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
        downDashBounceTime -= Time.deltaTime;
        QBRechargeDelay -= Time.deltaTime; 

        if (downDashBounceTime > 0 && onGround)// this allows the player to do smaller hops from the jump via dashing into the ground;
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MoveDirection();    
    }
    private void InputRegister()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && dashCount > 0 && !onGround && lastVerticalDash != 1)
        {
            Jump();
            dashCount -= 1;
            Debug.Log("UpBoost");
        }
        else if (Input.GetKey(jumpKey) && onGround && lastVerticalDash != 1)
        {
            Jump();
            Debug.Log("Jump");
        }

        if (Input.GetKey(downDash) && dashCount > 0 && !onGround && lastVerticalDash != 2)
        {
            DashDown();
        }
        else if (Input.GetKey(downDash) && dashCount > 0 && onGround && lastVerticalDash != 2)
        {
            lastVerticalDash = 2;
            dashCount -= 1;
            downDashGrounded = true;
            Debug.Log("Perepped");
        }

        if(Input.GetKey(QBKey))
        {
            if (dashCount > 0 && Input.GetKey(backwardKey))
            {
                if (lastQB != 2)
                {
                    dashDirection = -1;
                    ForwardDash();
                }

            }
            else if (dashCount > 0 && Input.GetKey(leftKey))
            {
                if (lastQB != 3)
                {
                    dashDirection = -1;
                    SideDash();
                }
            }
            else if (dashCount > 0 && Input.GetKey(rightKey))
            {
                if (lastQB != 4)
                {
                    dashDirection = 1;
                    SideDash();
                }

            }
            else if (dashCount > 0 && Input.GetKey(forwardKey))
            {
                if (lastQB != 1)
                {
                    dashDirection = 1;
                    ForwardDash();
                }

            }
        }
    }

    private void DashDown()
    {
        rb.AddForce((transform.up * -1) * QBForce, ForceMode.Impulse);
        downDashBounceTime = downDashBounceTimeStored;
        dashCount -= 1;
        lastVerticalDash = 2;
        Invoke(nameof(ResetQB), QBCoolDown);
        Invoke(nameof(VerticalDashMemory), verticalCoolDown);
        lastQB = 6;
        Debug.Log("DownBoost");
    }

    private void SideDash()
    {
        if (downDashGrounded == true)
        {
            rb.AddForce((transform.right * dashDirection) * (QBForce * QBForceMult * downDashPrepKick), ForceMode.Impulse);
        }
        else
        {
            rb.AddForce((transform.right * dashDirection) * (QBForce * QBForceMult), ForceMode.Impulse);
        }

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
            rb.AddForce(10 * moveSpeed * MovementDirection.normalized, ForceMode.Force);

            if (QBRechargeDelay < 0)
            {
                Invoke(nameof(ResetQB), QBRecharge);
                QBRechargeDelay = QBRechargeDelayStored;
            }
        }
        else 
        {
            rb.AddForce(10 * airmultiplier * moveSpeed * MovementDirection.normalized, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);

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
            lastVerticalDash = 1;
        }
        else if (downDashBounceTime > 0)
        {
            rb.AddForce((downDashBounceTime) * (QBForce * downDashBounceForce) * transform.up, ForceMode.Impulse);
            downDashBounceTime = 0;
        }
        else
        {
            rb.AddForce(transform.up * QBForce, ForceMode.Impulse);
            lastVerticalDash = 1;
        }

        lastQB = 5;
        speedLockTimer = speedLockTimerStored;
        Invoke(nameof(ResetQB), QBCoolDown);
        Invoke(nameof(VerticalDashMemory), verticalCoolDown);
        lastQB = 5;

    }
    public void QBMemory()
    {
        lastQB = 0;
    }
    public void VerticalDashMemory()
    {
        lastVerticalDash = 0;
    }

    public void ResetQB()
    {
       if (dashCount != DashCountStored)
        {
            dashCount += 1;
        }
    }
}
