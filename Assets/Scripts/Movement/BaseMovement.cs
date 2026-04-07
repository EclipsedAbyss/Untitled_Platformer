using System.ComponentModel;
using UnityEngine;

public class BaseMovement : MonoBehaviour//for context, QB means quickboost. its used to refer to both vertical and horizontal dashes in instances where both are affected by one field
{//start of the base movement script

    [Header("movement")]//these are all for basic movement (walking around on the floor)bhn
    public float moveSpeed;//manually set value for the default player speed
    public float overSpeed;//manually se tvalue for the post-dash player speed
    public Transform orientation;//the players orientation
    float horizontalInput;//the horizontal input axis(a and d prefferably)
    float verticalInput;//the vertical input axis (w and s prefferably)
    private float overSpeedThreshhold;//the velocity required to exit the normal movement speed and go into the increased state
    [SerializeField] private float overSpeedThreshholdValue;//used to manually set how much speed over the cap the player needs to enter over speed. this was added after initial playtesting revealed issues with this
    Vector3 MovementDirection;//the direction the player is moving in
    public float groundDrag;//the drag the player recieves whie grounded
    public float iceDrag;//the drag the player recieves on ice


    [Header("QuickBoost")] // all of these fields are for the multi-directional dash (includes jumping and downdashing) this was an absolute headache to get working initially.
    Rigidbody rb;//the players rigidbody
    public float QBForce;//the force the player can QB with
    public float horizontalDashForceMult;//the multiplier added to the force of horizontal dashes
    public float airmultiplier;//the multiplier of player movement applied while midair. 
    public float chargeCount;//the amount of charges the player is holding
    public float downDashBounceForce;//the force applied when the player jumps after dashing into the ground
    public KeyCode forwardKey = KeyCode.W;// inputs stored for detecting direction dash is being performed in.
    public KeyCode leftKey = KeyCode.A;//same as above
    public KeyCode backwardKey = KeyCode.S;//same as above
    public KeyCode rightKey = KeyCode.D;//same as above.
    [HideInInspector] public float dashDirection;//the direction the player is dashing (swapped between -1 and 1 to allow simpler code for directions)
    [HideInInspector] public float chargeCountStored;//the stored amount of dashes the player can hold. used to cap amount
    [HideInInspector] public float lastQB; // 0 is none, 1 is forward, 2 is back, 3 is left, 4 is right, 5 is down, and 6 is up. the numbers are inconsequental aside from allowing a cooldown bypass by chainboosting(this is intended). 
    [HideInInspector] public float downDashPrepKick;//used to store the bonus force for the boost
    [SerializeField] private float groundedBonusForce;//used to set the bonus force gained from a grounded downdash
    private bool canGroundJump;//used to register when jumping will not consume a charge


    [Header("QuickBoost Delay Values")]// these were split off the prior header for legibilities sake. these are all in relation to cooldowns and delays.
    public float QBCoolDown;//the cooldown for horizontal dashes
    public float QBRecharge;//the time for a charge to refill
    public float QBMemoryTime;//the time that a QB will be stored in memory
    public float chargeAccumulationThreshhold;//the value charge recharge interval must reach before a charge is refilled
    public float coyoteTimer;//the length of the coyotetime for jumping
    public bool amDashing;//this is used to tell things collided with that the player is dashing
    public float downDashBounceTime;//the window of time where the player can jump out of the state entered when dashing into the ground
    [SerializeField] private float QBDuration;//time until the player is no longer marked as dashing
    [SerializeField] private float boostChainFallOff;//the
    [SerializeField] private float downDashKickDecayTime;//used to decay the extra force a down dash gives a normal dash
    [SerializeField] private float chargeRechargeGroundedBonus;//names a bit messy, couldnt figure out a good name. this is just how much the recharge delay gets multiplied while grounded
    [HideInInspector] public float chargeRechargeInterval;//runs a time.deltatime to slowly increment up. once it hits a certain threshhold it gets reset and fills a dash
    private float downDashBounceTimeStored;//the stored value of the down dash bounce timer. used to reset it

    [Header("keybinds")]// actual input bindings. the priorly listed wasd are not really inputs but more just detection.
    public KeyCode jumpKey = KeyCode.Space;//gets the spacebar for jumping
    public KeyCode QBKey = KeyCode.Mouse1;//gets leftmouse for dashing
    public KeyCode downDash = KeyCode.Mouse2;//gets rightmouse for down dashing

    [Header("Ground Check")]// checks for the player being on the ground.
    public float playerHeight;//stores the height of the player
    public LayerMask whatIsGround;//stores the layermask for ground
    public LayerMask whatIsIce;//stores the layermask for ice
    bool onGround;//confirms that the player is grounded. 
    private bool canCoyoteTime;//used to store the coyotetime state

    private void Start()//start of start
    {
        rb = this.GetComponent<Rigidbody>();// gets the players rigidbody.
        chargeCountStored = chargeCount;//stores the total amount of dashes manually set.
        downDashBounceTimeStored = downDashBounceTime;//same as above
        downDashBounceTime = 0;// clears the timer to avoid it firing prematurely
        overSpeedThreshhold = moveSpeed + overSpeedThreshholdValue;//sets the move speed threshhold to its intended value
        canCoyoteTime = true;//informs that the player can enter coyotetime

    }//end of start

    private void Update()//start of update
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);// this allows the player to know what ground is
        bool sliding = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsIce);//allows the player to have different movement on ice.
        if (grounded)//checks if the player is in the grounded state
        {
            rb.linearDamping = groundDrag;//if it is, apply ground drag to player movement
            onGround = true;//set on ground to true
            canGroundJump = true;//mark the player as able to jump without consuming a charge
            canCoyoteTime = true;//and able to enter coyote time.
        }
        else if(sliding)//if the player is on ice
        {
            rb.linearDamping = iceDrag;//apply the ice drag to player movement
            onGround = true;//set on ground to true
            canGroundJump = true;//mark the player as able to jump without consuming a charge
            canCoyoteTime = true;//and able to enter coyote time
        }
        else if (canCoyoteTime)//if the player is not on the ground but can enter coyote time
        {
            Invoke(nameof(CoyoteTime), coyoteTimer);//count the coyote time period down
            onGround = false;//mark on ground as false
            rb.linearDamping = 0;//remove drag from player movement
            canCoyoteTime = false;//mark the player as no longer being able to enter oyote time as they are already in it
        }
        else//if the player is not on the ground
        {
            onGround = false;//mark on ground as false
            rb.linearDamping = 0;//remove drag from player movement
        }
        downDashBounceTime -= Time.deltaTime;//constantly decrement down dash bounce timer

      
    }//end of update

    private void LateUpdate()//start of lateupdate
    {
        if (chargeCount > chargeCountStored)//checks if the player is holding more then the maximum amount of charges
        {
            chargeCount = chargeCountStored;//if they are, change the amount to max
        }

    }//end of lateupdate

    private void FixedUpdate()//start of fixedupdate
    {
        InputRegister();// used to detect inputs
        SpeedControl();// used for drag on ground
        ResetQB();// used to recharge the players useable charge count.
        MoveDirection();    //allows movement and prevents movement from being sped up based on framerate.
    }//end of fixedupdate
    private void InputRegister()//start of the input register function. this is used for registering the players inputs
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");//gets the key inputs for left and right
        verticalInput = Input.GetAxisRaw("Vertical");//gets thekey inputs for forward and backward
        if (Input.GetKeyDown(jumpKey) && canGroundJump && lastQB != 6)// allows grunded jumps (does not consume a charge)
        {
            Jump();//fires the jump function
        }
        else if (Input.GetKeyDown(jumpKey) && chargeCount > 0 && !canGroundJump && lastQB != 6)// allows for midair jumping (up dash)
        {
            Jump();//fires the jump function
            amDashing = true;//marks the player as dashing
            chargeCount -= 1;//reduces the charges held by 1
        }
      
        if (Input.GetKeyDown(downDash) && chargeCount > 0 && !onGround && lastQB != 5)// allows to down dash
        {
            DashDown();//fires the dash down function
        }
        else if (Input.GetKeyDown(downDash) && chargeCount > 0 && onGround && lastQB != 5)//allows to down dash immediately into a horizontal dash while grounded for a larger result, at the cost of an extra charge.
        {
            chargeCount -= 1;//redduces charge count by 1
            downDashPrepKick = groundedBonusForce;//sets up the force for the extra dash impact
            Invoke(nameof(DownDashKickDecay), downDashKickDecayTime);//invokes the decay period for the kick
        }

        if(Input.GetKeyDown(QBKey))// registers when the player quickboosts
        {
            if (chargeCount > 0 && Input.GetKey(backwardKey))// placed at the top for highest priority, gets the player holding back and dashes in that direction
            {
                if (lastQB != 2)//checks to make sure the players most recent dash withint the cooldown was not backwards
                {
                    dashDirection = -1;//sets the dash direction  to -1, thus applying a backwards force
                    ForwardDash();//firest the forward dash function
                }
            }
            else if (chargeCount > 0 && Input.GetKey(leftKey))//these 2 (left and right) were essentially a coin toss in priority since realistically noone should try to do both at once
            {
                if (lastQB != 3)//checks to make sure the players most recent dash withint the cooldown was not left
                {
                    dashDirection = -1;////sets the dash direction to -1, applying a force to the left
                    SideDash();//fires the side dash function
                }
            }
            else if (chargeCount > 0 && Input.GetKey(rightKey))
            {
                if (lastQB != 4)//checks to make sure the players most recent dash withint the cooldown was not right
                {
                    dashDirection = 1;//sets the dash direction to 1, applying a force to the right
                    SideDash();//fires the side dash function
                }

            }
            else if (chargeCount > 0)// if the player is going forward or is simply still, the default is a forward dash. lowest priority to assist in control useability
            {
                if (lastQB != 1)//checks to make sure the player hasn't dashed forward within the cooldown
                {
                    dashDirection = 1;//sets the dash direction to 1, applying a forward force
                    ForwardDash();//fires the forward dash function
                }
            }
        }
    }//end of the input register function

    private void DashDown()//start of the downdash function. this is used to allow the player to perform the down dash
    {
        if (rb.linearVelocity.y > 0)//verifies the player isnt already falling
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);//resets players vertical velocity so that you can actually downdash out of a jump
        }
        rb.AddForce((transform.up * -1) * QBForce / 2, ForceMode.Impulse);// applies a downward force to the player
        downDashBounceTime = downDashBounceTimeStored;//allows a higher jump after dashing into the ground from midair. was originally done automatically but turned out EXTREMELY annoying in practice.
        chargeCount -= 1;//reduces the held charges by 1
        amDashing = true;//marks the player as dashing
        lastQB = 5;//marks the last qb as 5
        Invoke(nameof(QBMemory), QBCoolDown);//starts the QB cooldown
        Invoke(nameof(QBDurationEnd), QBDuration);//starts the timer for the players dashing state
    }//end of the down dash function

    private void SideDash()//start of the left/right dash function
    {
       rb.AddForce((transform.right * dashDirection) * (QBForce * horizontalDashForceMult * downDashPrepKick), ForceMode.Impulse);//applies a force in the direcction selected by the player on the left/right axis
        chargeCount -= 1;//reduces the held charges by 1
        if (dashDirection == -1)//checks the dash direction
            lastQB = 3;//if the last dash was left, it stores 3
        else//otherwise
            lastQB = 4;//if the last dash was right, it stores 4
        amDashing = true;//marks the player as dashing
        Invoke(nameof(QBMemory), QBCoolDown);//starts the QB cooldown
        Invoke(nameof(QBDurationEnd), QBDuration);//starts the timer for the players dashing state
    }//end of the left/right dash function

    private void ForwardDash()// start of the forward/backward dash function
    {
        rb.AddForce((transform.forward * dashDirection) * (QBForce * horizontalDashForceMult * downDashPrepKick), ForceMode.Impulse);//applies a force in the direcction selected by the player on the forward/back axis
        chargeCount -= 1;//reduces the held charges by 1
        if (dashDirection == -1)//checks the dash direction
            lastQB = 2;//if the last dash was backwards, it stores 2
        else//otherwise
            lastQB = 1;//if the last dash was forwards, it stores 1
        amDashing = true;//marks the player as dashing
        Invoke(nameof(QBMemory), QBCoolDown);//starts the QB cooldown
        Invoke(nameof(QBDurationEnd), QBDuration);//starts the timer for the players dashing state
    }//end of the forward/backward dash function

    private void MoveDirection()//allows to move the player
    {
        MovementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;//marks the movement direction as the orientation of the players front

        if (onGround)//checks if the player is on the ground
        {
            rb.AddForce(10 * moveSpeed * MovementDirection.normalized, ForceMode.Acceleration);//if so, applies ground drag and movement
        }
        else //if the player is not grounded
        {
            rb.AddForce(10 * airmultiplier * moveSpeed * MovementDirection.normalized, ForceMode.Force);//applies air movement to the player
        }
    }//end of the move direction unction
    private void SpeedControl()//this controls speed limits, and prevents attaining max speed by exclusively walking.
    {
        Vector3 flatVel = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);//gets the players flat velocity

        if (flatVel.magnitude > moveSpeed && flatVel.magnitude < overSpeedThreshhold )//caps the base plaer speed
        {
         Vector3 limitedVel = flatVel.normalized * moveSpeed;//locks and averages the players speed
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);//vector3 is made so that it does not count the upward velocity
        }
        else if (flatVel.magnitude > overSpeedThreshhold && flatVel.magnitude < overSpeed)//reduces speed over the default cap over time, to help incentivise quick movement
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x - Time.deltaTime / 2, rb.linearVelocity.y, rb.linearVelocity.z  - Time.deltaTime / 2);//decrements over speed by time.deltatime
        }
        else if(flatVel.magnitude > overSpeed)// caps the over speed.
        {
            Vector3 limitedOverVel = flatVel.normalized * overSpeed;//averages and locks the overspeed
            rb.linearVelocity = new Vector3(limitedOverVel.x, rb.linearVelocity.y, limitedOverVel.z);//vector3 is made so that it does not count the upward velocity
        }
    }//end of the speed control function
    private void Jump()// allows to jump
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);//resets players vertical velocity so that you can actually jump out of an extended fall.
        }

        if (downDashBounceTime > 0.9 && onGround == true)//checks if the player has slammed into the ground with the down dash
        {
            rb.AddForce((downDashBounceTime) * (QBForce * downDashBounceForce) * transform.up, ForceMode.Impulse);//applies the bounce force to the players jump immediately after
            downDashBounceTime = 0;//clears the bounce time
        }
        else//if neither of the above cases are in effect
        {
            rb.AddForce(transform.up * QBForce, ForceMode.Impulse);//calculate jump force normally
        }
        lastQB = 6;//marks the last QB as 6
        Invoke(nameof(QBMemory), QBCoolDown);//starts the QB cooldown
        Invoke(nameof(QBDurationEnd), QBDuration);//marks the player as dashing


    }//end of the jump function
    public void QBMemory()// remembers the last quickboost performed to allow chainboosting. chainboosting is an idea taken from an animation cancel exploit found in armoured core for answer.
    {
        lastQB = 0;//clears the last QB
    }//end of QB memory function

    public void ResetQB()// slowly increases stored charges.
    {
        if (chargeCount != chargeCountStored)//prevents it from trying to recharge when charges are full
        {
            if (onGround)//checks if the player is grounded
            {
                chargeRechargeInterval += Time.deltaTime * chargeRechargeGroundedBonus;//if the player is grounded, apply a multiplier to the recharge speed
            }
            else//if the player is midair
            {
                chargeRechargeInterval += Time.deltaTime;//recharge the charges at a normal pace
            }
            if (chargeRechargeInterval > chargeAccumulationThreshhold)//tracks the recharge intervals value. when it reaches the set value a dash is added back and the interval is reset
            {
                chargeCount += 1;//increases charge count by 1

                chargeRechargeInterval = 0;//resets the recharge interval
            }
        }
        else
        {
            chargeRechargeInterval = 0;//if the charges are full, just reset
        }
    }//end of reset QB function
    public void DownDashKickDecay()//decays the downdash boost increase
    {
        downDashPrepKick = 1;//sets the boost kick value to 1, as to avoid it multplying dash force by 0, cancelling them. 
    }//end of the down dash decay function
    public void QBDurationEnd()// ends the QB duration
    {
        amDashing = false;//used to communicate externally that the player is no longer dashing.
    }//end of the QB duration function
    public void CoyoteTime()// ends the coyoteTIme grace period.
    {
        canGroundJump = false;//ends the coyote time window and activates the state where jumping requires charges.
    }//end of the coyote time function

}//end of the base movement script
