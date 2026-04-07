using UnityEngine;

public class EmergencyGlass : MonoBehaviour
{//beginning of the glass script
    private BaseMovement playerMovement;//used to get the players state on if it is dashing or not
    private GameObject glass;//the glass object itself
    private void OnEnable()
    {
        glass = this.gameObject;//used to get the gameobject f the glass. this is here to make glass easier to break
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseMovement>() != null)//checks if the colliding object actually is the player (this is mainly precautionary)
        {
            playerMovement = collision.gameObject.GetComponent<BaseMovement>();//if it is the player, we get the base movement
            if (playerMovement.amDashing)//and check if the player is dashing.
            {
                Destroy(glass);//destroys the glass if the player is in fact dashing
            }
        }
    }//end of trigger functions


}//end of the glass script

