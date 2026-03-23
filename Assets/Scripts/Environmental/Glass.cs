using Unity.VisualScripting;
using UnityEngine;

public class Glass : MonoBehaviour
{
    private Rigidbody playerRB;
    private BaseMovement playerMovement;
    [SerializeField] private GameObject glass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMovement>() != null)
        {
            playerMovement = other.GetComponent<BaseMovement>();
            if (playerMovement.amDashing)
            {
                Destroy(glass);//destroys the glass
            }
            else
            {
                playerMovement = null; //clears the player movement to prevent any possible issues retaining it may induce.
            }
        }
    }
}
