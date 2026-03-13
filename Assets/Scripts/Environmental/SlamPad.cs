using UnityEngine;

public class SlamPad : MonoBehaviour
{

    private BaseMovement playerMovement;
    private void OnTriggerEnter(Collider other)
    {
    if(other.GetComponent<BaseMovement>() != null)
       {
            playerMovement = other.GetComponent<BaseMovement>();
            if (playerMovement.amDashing)
            {
                playerMovement.dashCount = playerMovement.dashCountStored;
                playerMovement = null;
            }
            else
            {
                playerMovement = null;
            }
           
       }
    }
}
