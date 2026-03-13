using UnityEngine;

public class BouncePad : MonoBehaviour
{


    private GameObject player;
    private Rigidbody playerRB;
    public float bouncePadForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMovement>() != null)
        {
            player = other.GetComponent<GameObject>();
            playerRB = other.GetComponent<Rigidbody>();
            playerRB.AddForce(this.transform.up * (bouncePadForce), ForceMode.Impulse);


        }
    }
}
