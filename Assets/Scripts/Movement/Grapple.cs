using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private BaseMovement playerMovementScript;
    [SerializeField] private float raycastRange;
    public KeyCode grapple = KeyCode.Mouse2;
    public float coolDown;
    private RaycastHit hit;
    [HideInInspector] public float coolDownStored;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(grapple))
        {
           
        }
    }

    private void WallGrapple()
    {

    }

    private void EnemyGrapple()
    {

    }
}
