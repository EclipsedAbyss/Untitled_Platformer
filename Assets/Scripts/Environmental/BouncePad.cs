using UnityEngine;

public class BouncePad : MonoBehaviour//used for the bounce pads
{//start of bounce pad script
    private Rigidbody objectRigidbody;//gets the rigidbody of the object collidimg with it
    public float bouncePadForce;//the manually set force the pad will apply
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (other.GetComponent<Rigidbody>() != null)//checks if colliding object has a rigidbody
        {
            objectRigidbody = other.GetComponent<Rigidbody>();//gets the rigidbodys object
            objectRigidbody.linearVelocity = Vector3.zero;//resets the rigidbodies velocity
            objectRigidbody.AddForce(this.transform.up * (bouncePadForce), ForceMode.Impulse);//applies the force to the rigidbody away from the actual pad
            objectRigidbody = null;//clears the stored rigidbody.
        }
    }//end of ontriggerenter
}//end of bounce pad script
