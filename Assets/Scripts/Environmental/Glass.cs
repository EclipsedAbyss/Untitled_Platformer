using Unity.VisualScripting;
using UnityEngine;

public class Glass : MonoBehaviour//needed context. this script is on a trigger surrounding the glass object itself to avoid it breaking flow of movement.
{//beginning of the glass script
    private BaseMovement playerMovement;//used to get the players state on if it is dashing or not
    [SerializeField] private GameObject glass;//the glass object itself
    private void OnTriggerEnter(Collider other)//start of trigger functions
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object actually is the player (this is mainly precautionary)
        {
            playerMovement = other.GetComponent<BaseMovement>();//if it is the player, we get the base movement
            if (playerMovement.amDashing)//and check if the player is dashing.
            {
                Destroy(glass);//destroys the glass if the player is in fact dashing
            }
            else//otherwise
            {
                playerMovement = null; //clears the player movement to prevent any possible issues retaining it may induce, and does not break the glass.
            }
        }
    }//end of trigger functions
}
