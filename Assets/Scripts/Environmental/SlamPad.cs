using UnityEngine;

public class SlamPad : MonoBehaviour//this is used for slam pads, which refill your charges when dashed into
{//start of slam pad script
    [SerializeField] private bool singleUse;//manually set bool that dictates whether the pad will recharge or not
    [SerializeField] private float coolDown;//manually set value for the cooldown if it is rechargeable
    private float coolDownStored;//used to store the cooldown for reseting the cooldown timer
    private BaseMovement playerMovement;//used to store the players movement
    private HitStopEffects hitstopfirer;//used to call the hitstop effect scipt (see the hitstop effect script for context)

    private void Start()//start of start
    {
        hitstopfirer = FindFirstObjectByType<HitStopEffects>();//gets the controller for the hitstop effect
        coolDownStored = coolDown;//stores the cooldown
        coolDown = 0;//clears the cooldown so it can be used
    }//end of start
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (coolDown <= 0)//checks if the cooldown is or is below zero
        {
            if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object is a player
            {
                playerMovement = other.GetComponent<BaseMovement>();//stores the players movement script
                if (playerMovement.amDashing)//checks if the player is dashing
                {
                    playerMovement.chargeCount = playerMovement.chargeCountStored;//refills the players dashes.
                    hitstopfirer.OnHitStopStart();//fires the hit stop FX script.
                    playerMovement = null;//clears the stored movement script
                    coolDown = coolDownStored;//resets the cooldown
                    if (singleUse)//checks if it is defined as single use
                    {
                        Destroy(this);//if it is, destroy the object
                    }
                }
                else//if the player is not dashing
                {
                    playerMovement = null;//clears the stored movement script
                }

            }
        }
    }//end of ontriggerenter
    private void Update()//start of update
    {
        
            coolDown -= Time.deltaTime;//reduces the cooldown if the pad isnt a one time use (this should be used primarily in the tutorial to avoid frustration, however a visual cue would be nice.

    }//end of update
}//end of slam pad script
