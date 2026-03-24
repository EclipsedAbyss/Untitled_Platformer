using UnityEngine;

public class Teleport_Plane : MonoBehaviour

{
    [SerializeField] private Transform target;
    private Rigidbody velocityReset;
    private BaseMovement boostRefill;
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = target.position;
        velocityReset = collision.gameObject.GetComponent<Rigidbody>();
        boostRefill = collision.gameObject.GetComponent<BaseMovement>();
        boostRefill.dashCount = boostRefill.dashCountStored;//refills the players dashes.
        velocityReset.linearVelocity = Vector3.zero;
        velocityReset = null;
        boostRefill = null;
    }
}
