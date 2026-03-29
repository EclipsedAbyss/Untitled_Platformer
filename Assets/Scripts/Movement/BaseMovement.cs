
using UnityEngine;

public class BaseMovement : MonoBehaviour//for context, qb means quickboost. its used to refer to both vertical and horizontal dashes whenr eferred to as one group.
{//start of base movement scripts

    [Header("movement")]//these are all for basic movement (walking around on the floor)bhn
    public float moveSpeed;
    public float overSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    private float overSpeedThreshhold;
    [HideInInspector] public float grounded;
    Vector3 MovementDirection;
    public float groundDrag;
    public float iceDrag;



    [Header("QuickBoost")] // all of these fields are for the multi-directional dash (includes jumping and downdashing) this was an absolute headache to get working initially.
    Rigidbody rb;
    public float QBForce;
    public float horizontalDashForceMult;
    public float airmultiplier;
    public float chargeCount;
    public float downDashBounceForce;
    public KeyCode forwardKey = KeyCode.W;// inputs stored for detecting direction dash is being performed in.
    public KeyCode leftKey = KeyCode.A;//same as above
    public KeyCode backwardKey = KeyCode.S;//same as above
    public KeyCode rightKey = KeyCode.D;//same as above.
    [HideInInspector] public float downDashBounce;
    [HideInInspector] public float dashDirection;
    [HideInInspector] public float dashCountStored;
    [HideInInspector] public float lastQB; // 0 is none, 1 is forward, 2 is back, 3 is left, 4 is right. the numbers are inconsequental aside from allowing a cooldown bypass by chainboosting(this is intended). 
    [HideInInspector] public float cachedQB; //used to cache another prior Qb, as a means to avoid issues caused by 2 buttons being held at once. as a consquence, chaining is now slightly harder to pull off.
    [HideInInspector] private bool canVerticalDash;// allows cancelling a circumstance where the player holds space and ctrl simoultaniously draining all of their charges.
    [HideInInspector] public float downDashPrepKick;//used to store the bonus force for the boost
    [SerializeField] private float groundedBonusForce;//used to set the bonus force gained from a grounded downdash
    private bool canGroundJump;
    private float dashCountLenienceStored;


    [Header("QuickBoost Delay Values")]// these were split off the prior header for legibilities sake. these are all in relation to cooldowns and delays.
    public float horizontalDashCoolDown;
    public float QBRecharge;
    public float QBRechargeDelayChecker;
    public float verticalDashCoolDown;
    public float QBMemoryTime;
    public float dashAccumulationThreshhold;
    public float coyoteTimer;
    public bool amDashing;
    public float downDashBounceTime;
    [SerializeField] private float QBDuration;
    [SerializeField] private float speedLockTimer;
    [SerializeField] private float boostChainFallOff;
    [SerializeField] private float downDashKickDecayTime;
    [SerializeField] private float chargeRechargeGroundedBonus;//names a bit messy, couldnt figure out a good name. this is just how much the recharge delay gets multiplied while grounded
    [SerializeField] private float chargeExpendedLeniencyDelay;
    [HideInInspector] public float dashRechargeInterval;
    private float speedLockTimerStored;
    private float downDashBounceTimeStored;
    private bool lenienceStackBlock;





    [Header("keybinds")]// actual input bindings. the priorly listed wasd are not really inputs but more just detection.
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode QBKey = KeyCode.Mouse1;
    public KeyCode downDash = KeyCode.LeftControl;

    [Header("Ground Check")]// checks for the player being on the ground.
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsIce;
    bool onGround;
    private bool canCoyoteTime;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();// gets the players rigidbody.
        dashCountStored = chargeCount;//stores the total amount of dashes manually set.
        speedLockTimerStored = speedLockTimer;// preserves information of the manually stated timer.
        downDashBounceTimeStored = downDashBounceTime;//same as above
        downDashBounceTime = 0;// clears the timer to avoid it firing prematurely
        speedLockTimer = 0;//same as above
        canVerticalDash = true;
        overSpeedThreshhold = moveSpeed + 1;
        canCoyoteTime = true;

    }

    private void Update()
    {

        InputRegister();// used to detect inputs
        SpeedControl();// used for drag on ground
        ResetQB();// used to recharge the players useable charge count.
        DashExpendLenience();//used to prevent dashes taking more charges then they should. hopefully.


        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);// this allows the player to know what ground is
        bool sliding = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsIce);//allows the player to have different movement on ice.
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            onGround = true;
            canGroundJump = true;
            canCoyoteTime = true;
        }
        else if(sliding)
        {
            rb.linearDamping = iceDrag;
            onGround = true;
            canGroundJump = true;
            canCoyoteTime = true;
        }
        else if (canCoyoteTime)
        {
            Invoke(nameof(CoyoteTime), coyoteTimer);
            onGround = false;
            rb.linearDamping = 0;
            canCoyoteTime = false;

        }
        speedLockTimer -= Time.deltaTime;
        downDashBounceTime -= Time.deltaTime;

      
    }

    private void LateUpdate()
    {
        if (chargeCount > dashCountStored)
        {
            chargeCount -= 1;
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
        if (Input.GetKeyDown(jumpKey) && canGroundJump && canVerticalDash)// allows grunded jumps (does not consume a charge)
        {
            Jump();
            Debug.Log("Jump");
        }
        else if (Input.GetKeyDown(jumpKey) && chargeCount > 0 && !canGroundJump && canVerticalDash)// allows for midair jumping (up dash)
        {
            Jump();
            amDashing = true;
            chargeCount -= 1;
            Debug.Log("UpBoost");
        }
      

        if (Input.GetKeyDown(downDash) && chargeCount > 0 && !onGround && canVerticalDash)// allows to down dash
        {
            DashDown();
        }
        else if (Input.GetKeyDown(downDash) && chargeCount > 0 && onGround && canVerticalDash)//allows to down dash immediately into a horizontal dash while grounded for a larger result, at the cost of an extra charge.
        {
            canVerticalDash = false;
            chargeCount -= 1;
            downDashPrepKick = groundedBonusForce;
            Invoke(nameof(VerticalDashMemory), verticalDashCoolDown);
            Invoke(nameof(DownDashKickDecay), downDashKickDecayTime);
            Debug.Log("Prepped");
        }

        if(Input.GetKeyDown(QBKey))// registers when the player quickboosts
        {
            if (chargeCount > 0 && Input.GetKey(backwardKey))// placed at the top for highest priority, gets the player holding back and dashes in that direction
            {
                if (lastQB != 2 && cachedQB != 2)
                {
                    dashDirection = -1;
                    ForwardDash();
                }

            }
            else if (chargeCount > 0 && Input.GetKey(leftKey))//these 2 (left and right) were essentially a coin toss in priority since realistically noone should try to do both at once
            {
                if (lastQB != 3 && cachedQB != 3)
                {
                    dashDirection = -1;// to allow for less obtuse coode, both side dashes run the same function but just reverse the actual directionit is sent. the same applies to forward and back.
                    SideDash();
                }
            }
            else if (chargeCount > 0 && Input.GetKey(rightKey))
            {
                if (lastQB != 4 && cachedQB != 4)
                {
                    dashDirection = 1;
                    SideDash();
                }

            }
            else if (chargeCount > 0)// if the player is going forward or is simply still, the default is a forward dash. lowest priority to assist in control useability
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
        rb.AddForce((transform.up * -1) * QBForce / 2, ForceMode.Impulse);// applies a downward force to the player
        downDashBounceTime = downDashBounceTimeStored;//allows a higher jump after dashing into the ground from midair. was originally done automatically but turned out EXTREMELY annoying in practice.
        chargeCount -= 1;
        cachedQB = lastQB;
        lastQB = 6;
        canVerticalDash = false;// prevents theplayer from holding ctrl and space simoultaniously, draining all their charge instantly
        amDashing = true;//used for camera calculations and slam pads.
        Invoke(nameof(VerticalDashMemory), verticalDashCoolDown /2);
        Invoke(nameof(QBDurationEnd), QBDuration);
        Debug.Log("DownBoost");
    }

    private void SideDash()//code to induce the sideways dash
    {
       rb.AddForce((transform.right * dashDirection) * (QBForce * horizontalDashForceMult * downDashPrepKick), ForceMode.Impulse);
        chargeCount -= 1;
        CacheChecker();
        if (dashDirection == -1)
            lastQB = 3;
        else if (dashDirection == 1)
            lastQB = 4;
        amDashing = true;
        Invoke(nameof(HorizontalDashMemory), horizontalDashCoolDown);
        Invoke(nameof(QBDurationEnd), QBDuration);
        Debug.Log("sideboost");
    }

    private void ForwardDash()// code to induce the forward/backward dash
    {
        rb.AddForce((transform.forward * dashDirection) * (QBForce * horizontalDashForceMult * downDashPrepKick), ForceMode.Impulse);
        chargeCount -= 1;
        CacheChecker();
        lastQB = 2;
        if (dashDirection == -1)
            lastQB = 2;
        else if (dashDirection == 1)
            lastQB = 1;
        amDashing = true;
        Invoke(nameof(HorizontalDashMemory), horizontalDashCoolDown);
        Invoke(nameof(QBDurationEnd), QBDuration);
        Debug.Log("forward boost");
    }

    private void MoveDirection()//allows to move the player
    {
        MovementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (onGround)
        {
            rb.AddForce(10 * moveSpeed * MovementDirection.normalized, ForceMode.Acceleration);

            if (QBRechargeDelayChecker < chargeCount)
            {
                Invoke(nameof(ResetQB), QBRecharge);
                QBRechargeDelayChecker = chargeCount;
            }
            else if (QBRechargeDelayChecker > chargeCount)
            {
                QBRechargeDelayChecker = chargeCount;
            }
        }
        else 
        {
            rb.AddForce(10 * airmultiplier * moveSpeed * MovementDirection.normalized, ForceMode.Force);//used for air movement so that it doesnt feel identical to grounded movement.
        }
    }
    private void SpeedControl()//this controls speed limits, and prevents attaining max speed by exclusively walking.
    {
        Vector3 flatVel = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);//gets the players flat velocity

        if (flatVel.magnitude > moveSpeed && flatVel.magnitude < overSpeedThreshhold )//caps the base plaer speed
        {
         Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > overSpeedThreshhold && flatVel.magnitude < overSpeed)//reduces speed ofver the default cap over time, to help incentivise quick movement
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x - Time.deltaTime / 2, rb.linearVelocity.y, rb.linearVelocity.z  - Time.deltaTime / 2);
        }
        else if(flatVel.magnitude > overSpeed)// caps the over speed.
        {
            Vector3 limitedOverVel = flatVel.normalized * overSpeed;
            rb.linearVelocity = new Vector3(limitedOverVel.x, rb.linearVelocity.y, limitedOverVel.z);
        }
    }
    private void Jump()// allows to jump
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);//resets players vertical velocity so that you can actually jump out of an extended fall.
        if (speedLockTimer > 0)
        {
            rb.AddForce(transform.up * (QBForce / boostChainFallOff), ForceMode.Impulse);
            canVerticalDash = false;//this is used for cooldowns of both the jump and the down dash.
        }
        else if (downDashBounceTime > 0 && onGround == true)
        {
            rb.AddForce((downDashBounceTime) * (QBForce * downDashBounceForce) * transform.up, ForceMode.Impulse);
            downDashBounceTime = 0;
        }
        else
        {
            rb.AddForce(transform.up * QBForce, ForceMode.Impulse);
            canVerticalDash = false;
        }

        lastQB = 5;
        speedLockTimer = speedLockTimerStored;
        Invoke(nameof(VerticalDashMemory), verticalDashCoolDown);
        Invoke(nameof(QBDurationEnd), QBDuration);


    }
    public void HorizontalDashMemory()// remembers the last quickboost performed to allow chainboosting. chainboosting is an idea taken from an animation cancel exploit found in armoured core for answer.
    {
        cachedQB = 0;
        lastQB = 0;
    }
    public void VerticalDashMemory()//allows to do the same as above but for dashing up and down
    {
        canVerticalDash = true;
    }

    public void ResetQB()// slowly increases stored charges.
    {
        if (chargeCount != dashCountStored)//prevents it from trying to recharge when charges are full
        {

            if (onGround)
            {
                dashRechargeInterval += Time.deltaTime * chargeRechargeGroundedBonus;
            }
            else
            {
                dashRechargeInterval += Time.deltaTime;//this if statement exists so that dashes recharge faster when grounded.
            }
            if (dashRechargeInterval > dashAccumulationThreshhold)// this is used as opposed to a raw time.deltatime to allow charges to take more time to fill.
            {
                chargeCount += 1;

                dashRechargeInterval = 0;
            }

        }
        else
        {
            dashRechargeInterval = 0;
        }
    }
    public void DownDashKickDecay()//decays the downdash boost increase
    {
        downDashPrepKick = 1;//sets the boost kick value to 1, as to avoid it multplying dash force by 0, cancelling them. 
    }
    public void QBDurationEnd()// ends the dash duration
    {
        amDashing = false;//used to communicate externally that the player is no longer dashing.
    }
    public void CoyoteTime()// ends the coyoteTIme grace period.
    {
        canGroundJump = false;//ends the coyote time window and activates the state where jumping requires charges.
    }
    private void DashExpendLenience()
    {
        if (chargeCount - 1 > 1)
        {
            if (dashCountLenienceStored -1 > (chargeCount))
            {
                chargeCount += 1;
            }
        }
        if (lenienceStackBlock)
        {
            
            lenienceStackBlock = false;
        }
        Invoke(nameof(DashExpendLenienceFire), chargeExpendedLeniencyDelay);
    }
    public void DashExpendLenienceFire()
    {
        dashCountLenienceStored = chargeCount;
        lenienceStackBlock = true;
    }
    private void CacheChecker()
    {
        if (lastQB != 1)
        {
            cachedQB = lastQB;//caches thelast qb to keep its cooldown consistent. if 3 dshes are performed the cche gets emptied, this is intentional
        }
        else
        {
            cachedQB = 0;//this allows you to cancel the forward dash cooldwon by dashing in a different direction, allowing far more frantic movement.
        }
    }

}
