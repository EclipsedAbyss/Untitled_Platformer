using UnityEngine;

public class MovingPlatform : MonoBehaviour//this is used for the movement of the platform, and has no relation to the actual effects it has on the player;
{
    [SerializeField] private Vector3 startPoint;//stores the platforms starting position
    [SerializeField] private Vector3 position;//stores the platforms current position
    [SerializeField] private Vector3 endPoint;//stores the platforms ending position
    [SerializeField] private float speed;//stores the platforms speed
    [SerializeField] private GameObject boundingTrigger;//stores the trigger that interacts with player velocity
    private float step;//used for stepping the platform
    public float Direction;//tells the platform if its going to the end point or the start point (1 is to end, -1 is to start)
    [SerializeField] private GameObject platform;//stores the platform as a gameobject

    private void OnEnable()
    {
        platform.transform.position = startPoint;//makes sure the platform is on its starting position
        Direction = 1;//sets the platform to move towards 
    }
    private void FixedUpdate()//fixedupdate is used to avoid the platform movement being framerate dependant
    {
        step = Time.deltaTime / 2 * speed;//sets step to deltatime for smooth movement and multiplies it by the manually set speed value

        if (Direction == 1)//checks the movement direction
        {
            position = Vector3.MoveTowards(position, endPoint, step);//moves the platform towards the end point.
        }
        else//if direction isnt 1. it should never be anything other then 2 specific values so no if is needed
        {
            position = Vector3.MoveTowards(position, startPoint, step);//moves the platform back towards the start point
        }

        platform.transform.position = position;//saves the current position

        if (position == endPoint)//checks if the platform has reached the end point
        {
            Direction = -1;//inverts its movement direction
        }
        else if (position == startPoint)//checks if platform has reached its start point
        {
            Direction = 1;//sets the movement back to normal
        }

    }

    private void OnCollisionEnter(Collision collision)//start of the collision entry function
    {
        if (collision.gameObject.GetComponent<BaseMovement>() != null)//checks if the colliding object is a player
        {
            boundingTrigger.SetActive(true);//enables the movement trigger;
        }
    }//end of the collision entry function



}



