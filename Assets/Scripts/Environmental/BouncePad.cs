using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Rigidbody playerRB;
    public float bouncePadForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMovement>() != null)
        {
            playerRB = other.GetComponent<Rigidbody>();
            playerRB.linearVelocity = Vector3.zero;
            playerRB.AddForce(this.transform.up * (bouncePadForce), ForceMode.Impulse);
            playerRB = null;
        }
    }
}
