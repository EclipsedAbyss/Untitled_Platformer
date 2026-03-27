using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    public float bouncePadForce;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            objectRigidbody = other.GetComponent<Rigidbody>();
            objectRigidbody.linearVelocity = Vector3.zero;
            objectRigidbody.AddForce(this.transform.up * (bouncePadForce), ForceMode.Impulse);
            objectRigidbody = null;
        }
    }
}
