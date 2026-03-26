using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private MovingPlatform platformInfo;
    public GameObject player;
    private Vector3 offset;
    private bool onPlatform;
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private float platformOffsetX;
    private float platformOffsetY;
    private float platformOffsetZ;
    private Vector3 platformOffset;
    private bool inCollider;
    private float timeToBoot;

    private void OnEnable()
    {
        lastPosition = gameObject.transform.position;
    }
    private void Update()
    {
        currentPosition = gameObject.transform.position;

        platformOffsetX = (Mathf.Abs(lastPosition.x - currentPosition.x));
        platformOffset.y = (Mathf.Abs(lastPosition.y - currentPosition.y));
        platformOffsetZ = (Mathf.Abs(lastPosition.z - currentPosition.z));
        platformOffset  = new Vector3(platformOffsetX, platformOffsetY, platformOffsetZ);
        if (onPlatform &&  inCollider)
        {
            player.transform.position = platform.transform.position + offset + (platformOffset * platformInfo.Direction) ;
        }
        lastPosition = currentPosition;

        //if (!inCollider && onPlatform)
        //{
        //    timeToBoot += Time.deltaTime;
        //    if (timeToBoot > 0.5)
        //    {
        //        platformInfo.Disembark();
        //        onPlatform = false;
        //        Debug.Log("bad connection");
        //    }
        //}

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BaseMovement>() != null)
        {
            player = other.gameObject.GetComponent<BaseMovement>().gameObject;
            onPlatform = true;
            if (other.gameObject.GetComponent<BaseMovement>() != null)
            {
                player = other.gameObject;
                player.transform.parent = platform.transform;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (player != null)
        {
            offset = player.transform.position - platform.transform.position;
        }
        inCollider = true;
        Debug.Log("player is inside the collider");
    }

    private void OnTriggerExit(Collider other)
    {
        Disembark();
        onPlatform = false;
        Debug.Log("aura exited");
    }
    private void LateUpdate()
    {
        inCollider = false;
        
    }
    public void Disembark()
    {
        if (player != null)
        {
            player.transform.parent = null;
            gameObject.SetActive(false);
        }
      
        //boundingTrigger.SetActive(false);
    }

}
