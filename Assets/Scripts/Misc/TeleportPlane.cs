using UnityEngine;

public class TeleportPlane : MonoBehaviour//used for the kill planes in the tutorial to avoid making it miserable
{//start of the teleport plane script
    [SerializeField] private Transform target;//stores the target teleport location for the object that collides
    private Rigidbody velocityReset;//used to reset the velocity of the colliding rigidbody
    private BaseMovement boostRefill;//used to refill the players boosts on "death"
    private void OnCollisionEnter(Collision collision)//start of oncollisionenter
    {
        collision.gameObject.transform.position = target.position;//teleports the object (hopefully the player) to the manually set position
        velocityReset = collision.gameObject.GetComponent<Rigidbody>();//gets the objects rigidbody
        boostRefill = collision.gameObject.GetComponent<BaseMovement>();//gets the players movement script
        boostRefill.chargeCount = boostRefill.dashCountStored;//refills the players dashes.
        velocityReset.linearVelocity = Vector3.zero;//resets the players velocity
        velocityReset = null;//resets the stored object rigidbody
        boostRefill = null;//resets the stored player movement.
    }//end of oncollissionenter
}//end of the teleport plane script
