using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float speed;
    private float step;
    public bool Direction;
    [SerializeField] private GameObject platform;
    private GameObject player;
    private Rigidbody playerRB;
    [SerializeField] private float platformEffectTime;
    private float platformEffect;
    private bool platformExit;
    [SerializeField]private Rigidbody velocity;

    private void OnEnable()
    {
        platform.transform.position = startPoint;
        Direction = true;
    }
    private void Update()
    {
        step = Time.deltaTime / 2 * speed;

        if (Direction)
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
            Direction = false;
        }
        else if (position == startPoint)
        {
            Direction = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BaseMovement>() != null)
        {
            player = collision.gameObject;
            playerRB = collision.gameObject.GetComponent<Rigidbody>();
            platformEffect = platformEffectTime;
            player.transform.parent = platform.transform;
        }
        platformExit = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        platformExit = true;

        Invoke(nameof(Disembark), platformEffectTime);
    }

    private void Disembark()
    {
        if (platformExit)
        {
            platform.transform.DetachChildren();
        }

    }
}



