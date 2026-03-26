using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float speed;
    [SerializeField] private GameObject boundingTrigger;
    private float step;
    public float Direction;
    [SerializeField] private GameObject platform;
    private GameObject player;
    [SerializeField]private Rigidbody velocity;

    private void OnEnable()
    {
        platform.transform.position = startPoint;
        Direction = 1;
        //boundingTrigger.SetActive(false);
    }
    private void Update()
    {
        step = Time.deltaTime / 2 * speed;

        if (Direction == 1)
        {
            position = Vector3.MoveTowards(position, endPoint, step);
        }
        else
        {
            position = Vector3.MoveTowards(position, startPoint, step);
        }

        platform.transform.position = position;

        if (position == endPoint)
        {
            Direction = -1;
        }
        else if (position == startPoint)
        {
            Direction = 1;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        boundingTrigger.SetActive(true);
        if (collision.gameObject.GetComponent<BaseMovement>() != null)
        {
            boundingTrigger.SetActive(true);
        }
    }



}



