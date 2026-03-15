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
    [HideInInspector] public float grounded;
    Vector3 MovementDirection;
    public float groundDrag;



    [Header("QuickBoost")] // all of these fields are for the multi-directional dash (includes jumping and downdashing) this was an absolute headache to get working initially.
    Rigidbody rb;
    public float QBForce;
    public float QBForceMult;
    public float airmultiplier;
    public float dashCount;
    public float downDashBounce;
    public float downDashBounceTime;
    public float downDashBounceForce;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;// inputs stored for detecting direction dash is being performed in.
    [HideInInspector] public float dashDirection;
    [HideInInspector] public float dashCountStored;
    [HideInInspector] public float lastQB; // 0 is none, 1 is forward, 2 is back, 3 is left, 4 is right. the numbers are inconsequental aside from allowing a cooldown bypass by chainboosting(this is intended). 
    [HideInInspector] private bool canVDash;// allows cancelling a circumstance where the player holds space and ctrl simoultaniously draining all of their charges.
    [HideInInspector] public float downDashPrepKick;//used to store the bonus force for the boost
    [SerializeField] private float groundedBonusForce;//used to set the bonus force gained from a grounded downdash


    [Header("QuickBoost Delay Values")]// these were split off the r\prior header for legibilities sake. these are all in relation to cooldowns and delays.
    public float QBCoolDown;
    public float QBRecharge;
    public float QBRechargeDelayChecker;
    public float verticalCoolDown;
    public float QBMemoryTime;
    public float dashAccumulationThreshhold;
    public bool amDashing;
    [SerializeField] private float dashDuration;
    [SerializeField] private float speedLockTimer;
    [SerializeField] private float boostChainFallOff;
    [SerializeField] private float downDashKickDecayTime;
    [HideInInspector] public float dashRechargeInterval;
    private float speedLockTimerStored;
    private float downDashBounceTimeStored;
    

    

    [Header("keybinds")]// actual input bindings. the priorly listed wasd are not really inputs but more just detection.
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode QBKey = KeyCode.LeftShift;
    public KeyCode downDash = KeyCode.LeftControl;

    [Header("Ground Check")]// checks for the player being on the ground.
    public float playerHeight;
    public LayerMask whatIsGround;
    bool onGround;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();// gets the players rigidbody.
        dashCountStored = dashCount;//stores the total amount of dashes manually set.
        speedLockTimerStored = speedLockTimer;// preserves information of the manually stated timer.
        downDashBounceTimeStored = downDashBounceTime;//same as above
        downDashBounceTime = 0;// clears the timer to avoid it firing prematurely
        speedLockTimer = 0;//same as above
        canVDash = true;
    }

    private void Update()
    {

        InputRegister();// used to detect inputs
        SpeedControl();// used for drag on ground
        ResetQB();// used to recharge the players useable charge count.


        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);// this allows the player to know what ground is
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
        

        if (downDashBounceTime > 0 && onGround)// this allows the player to bounce when downdashing into the floor. not extremely useful but its kinda fun and could definately have use cases
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MoveDirection();    //allows movement and prevents movement from being sped up based on framerate.
    }
    private void InputRegister()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(jumpKey) && dashCount > 0 && !onGround && canVDash)// allows for midair jumping (up dash)
        {
            Jump();
            dashCount -= 1;
            Debug.Log("UpBoost");
        }
        else if (Input.GetKey(jumpKey) && onGround && canVDash)// allows grunded jumps (does not consume a charge)
        {
            Jump();
            Debug.Log("Jump");
        }

        if (Input.GetKey(downDash) && dashCount > 0 && !onGround && canVDash)// allows to down dash
        {
            DashDown();
        }
        else if (Input.GetKey(downDash) && dashCount > 0 && onGround && canVDash)//allows to down dash immediately into a horizontal dash while grounded for a larger result, at the cost of an extra charge.
        {
            canVDash = false;
            dashCount -= 1;
            downDashPrepKick = groundedBonusForce;
            Invoke(nameof(VerticalDashMemory), verticalCoolDown);
            Invoke(nameof(DownDashKickDecay), downDashKickDecayTime);
            Debug.Log("Prepped");
        }

        if(Input.GetKey(QBKey))// registers when the player quickboosts
        {
            if (dashCount > 0 && Input.GetKey(backwardKey))// placed at the top for highest priority, gets the player holding back and dashes in that direction
            {
                if (lastQB != 2)
                {
                    dashDirection = -1;
                    ForwardDash();
                }

            }
            else if (dashCount > 0 && Input.GetKey(leftKey))//these 2 (left and right) were essentially a coin toss in priority since realistically noone should try to do both at once
            {
                if (lastQB != 3)
                {
                    dashDirection = -1;// to allow for less obtuse coode, both side dashes run the same function but just reverse the actual directionit is sent. the same applies to forward and back.
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
            else if (dashCount > 0)// if the player is going forward or is simply still, the default is a forward dash. lowest priority to assist in control useability
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
        rb.AddForce((transform.up * -1) * QBForce, ForceMode.Impulse);// applies a downward force to the player
        downDashBounceTime = downDashBounceTimeStored;
        dashCount -= 1;
        canVDash = false;// prevents theplayer from holding ctrl and space simoultaniously, draining all their charge instantly
        amDashing = true;//used for camera calculations and slam pads.
        Invoke(nameof(VerticalDashMemory), verticalCoolDown);
        Invoke(nameof(dashDurationEnd), dashDuration);
        lastQB = 6;
        Debug.Log("DownBoost");
    }

    private void SideDash()//code to induce the sideways dash
    {
       rb.AddForce((transform.right * dashDirection) * (QBForce * QBForceMult * downDashPrepKick), ForceMode.Impulse);
        dashCount -= 1;
        if (dashDirection == -1)
            lastQB = 3;
        else if (dashDirection == 1)
            lastQB = 4;
        amDashing = true;
        Invoke(nameof(QBMemory), QBCoolDown);
        Invoke(nameof(dashDurationEnd), dashDuration);
        Debug.Log("sideboost");
    }

    private void ForwardDash()// code to induce the forward/backward dash
    {
        rb.AddForce((transform.forward * dashDirection) * (QBForce * QBForceMult * downDashPrepKick), ForceMode.Impulse);
        dashCount -= 1;
        lastQB = 2;
        if (dashDirection == -1)
            lastQB = 2;
        else if (dashDirection == 1)
            lastQB = 1;
        amDashing = true;
        Invoke(nameof(QBMemory), QBCoolDown);
        Invoke(nameof(dashDurationEnd), dashDuration);
        Debug.Log("forward boost");
    }

    private void MoveDirection()//allows to move the player
    {
        MovementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (onGround)
        {
            rb.AddForce(10 * moveSpeed * MovementDirection.normalized, ForceMode.Force);

            if (QBRechargeDelayChecker < dashCount)
            {
                Invoke(nameof(ResetQB), QBRecharge);
                QBRechargeDelayChecker = dashCount;
            }
            else if (QBRechargeDelayChecker > dashCount)
            {
                QBRechargeDelayChecker = dashCount;
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
    private void Jump()// allows to jump
    {
        if(speedLockTimer > 0)
        {
            rb.AddForce(transform.up * (QBForce / boostChainFallOff), ForceMode.Impulse);
            canVDash = false;
        }
        else if (downDashBounceTime > 0)
        {
            rb.AddForce((downDashBounceTime) * (QBForce * downDashBounceForce) * transform.up, ForceMode.Impulse);
            downDashBounceTime = 0;
        }
        else
        {
            rb.AddForce(transform.up * QBForce, ForceMode.Impulse);
            canVDash = false;
        }

        lastQB = 5;
        amDashing = true;
        speedLockTimer = speedLockTimerStored;
        Invoke(nameof(VerticalDashMemory), verticalCoolDown);
        Invoke(nameof(dashDurationEnd), dashDuration);


    }
    public void QBMemory()// remembers the last quickboost performed to allow chainboosting. chainboosting is an idea taken from an animation cancel exploit found in armoured core for answer.
    {
        lastQB = 0;
    }
    public void VerticalDashMemory()//allows to do the same as above but for dashing up and down
    {
        canVDash = true;
    }

    public void ResetQB()// slowly increases stored charges.
    {
        if (dashCount != dashCountStored)//prevents it from trying to recharge when charges are full
        {
            dashRechargeInterval += Time.deltaTime;

            if (dashRechargeInterval > dashAccumulationThreshhold)// this is used as opposed to a raw time.deltatime to allow charges to take more time to fill.
            {
                dashCount += 1;

                dashRechargeInterval = 0;
            }
        }
        else if (dashCount > dashCountStored)
        {
            dashCount -= 1;
        }
        else
        {
            dashRechargeInterval = 0;
        }
    }
    public void DownDashKickDecay()
    {
        downDashPrepKick = 1;
    }
    public void dashDurationEnd()
    {
        amDashing = false;
    }
}
