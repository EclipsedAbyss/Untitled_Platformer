using UnityEngine;

public class Glass : MonoBehaviour//needed context. this script is on a trigger surrounding the glass object itself to avoid it breaking flow of movement.
{//beginning of the glass script
    private BaseMovement playerMovement;//used to get the players state on if it is dashing or not
    private Grapple grappling;
    [SerializeField] private GameObject glass;//the glass object itself
    private void OnTriggerEnter(Collider other)//start of trigger functions
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object actually is the player (this is mainly precautionary)
        {
            playerMovement = other.GetComponent<BaseMovement>();//if it is the player, we get the base movement
            grappling = other.GetComponent<Grapple>();
            if (playerMovement.amDashing || grappling.grapplingNow)//and check if the player is dashing or grappling.
            {
                Destroy(glass);//destroys the glass if the player is in fact dashing
            }
        }
    }//end of trigger functions
}//end of the glass script
