using UnityEngine;

public class EmergencyGlass : MonoBehaviour
{//beginning of the glass script
    private BaseMovement playerMovement;//used to get the players state on if it is dashing or not
    private GameObject glass;//the glass object itself
    private void OnEnable()
    {
        glass = this.gameObject;
    }
    private void OnTriggerEnter(Collider other)//start of trigger functions
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object actually is the player (this is mainly precautionary)
        {
            playerMovement = other.GetComponent<BaseMovement>();//if it is the player, we get the base movement
            if (playerMovement.amDashing)//and check if the player is dashing.
            {
                Destroy(glass);//destroys the glass if the player is in fact dashing
            }
        }
    }//end of trigger functions
}//end of the glass script

