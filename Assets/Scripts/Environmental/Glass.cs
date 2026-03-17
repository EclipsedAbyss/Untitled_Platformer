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
                Destroy(glass);
            }
            else
            {
                playerMovement = null;
            }
        }
    }
}
