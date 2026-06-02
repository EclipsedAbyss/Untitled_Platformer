using UnityEngine;

public class PlatformTrigger : MonoBehaviour//this controls the player movement effects of the moving platform. this script is NOT used for the platform itselfs movement, as that is held elsewhere,
{//start of the platform trigger script
    [SerializeField] private GameObject platform;//stores the platform
    [SerializeField] private MovingPlatform platformInfo;//stores the information about the platform
    public GameObject player;//used for storing the player
    private bool onPlatform;//used to initiate the players status of being on the platform
    private Vector3 lastPosition;//the platfoerms last stored position
    private Vector3 currentPosition;//the platforms current stored position
    private float platformOffsetX;//the player position offset on the X axis. this will be combined into a vector3
    private float platformOffsetY;//the player offset position on the Y axis. this will be combined into a vector3
    private float platformOffsetZ;//the player offset position on the Z axis. this will be combined into a vector3
    private Vector3 platformOffset;//the overall stored player position offset
    private bool inCollider;//bool for making certain the player is in the collider
    private void OnEnable()//start of onenable
    {
        lastPosition = gameObject.transform.position;//sets the position of the trigger into memory
    }//end of onenable
    private void FixedUpdate()//start of fixxedupdate. it is used here so it does not desync with the platform
    {
        currentPosition = gameObject.transform.position;//gets the current position of the object

        platformOffsetX = (Mathf.Abs(lastPosition.x - currentPosition.x));//stores the offset on the X axis
        platformOffsetY = (Mathf.Abs(lastPosition.y - currentPosition.y));//stores the offset on the Y axis
        platformOffsetZ = (Mathf.Abs(lastPosition.z - currentPosition.z));//stores the offset on the Z axis
        platformOffset  = new Vector3(platformOffsetX, platformOffsetY, platformOffsetZ);//combines the offsets into a vector3
        if (onPlatform &&  inCollider)//checks if the player is on the platform
        {
            player.transform.position = platform.transform.position + (platformOffset) ;//applies both the offset and the platform offset to the player
        }
        lastPosition = currentPosition;//stores the current (now prior) position of the platform
    }//end of fixedupdate
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (other.gameObject.GetComponent<BaseMovement>() != null)//checks if the objectin contact is the player
        {
            player = other.gameObject.GetComponent<BaseMovement>().gameObject;//stores the player
            onPlatform = true;//verifies the entry of the collider
            player.transform.parent = platform.transform;//parents the player to the platform
            
        }
    }//end of ontriggerenter
    private void OnTriggerStay(Collider other)//start of ontriggerstay
    {
        inCollider = true;//turns on incollider. this is used to make sure the player doesnt enter a state where they are attatched to the platform but not on it
    }//end of ontriggerstay
    private void OnTriggerExit(Collider other)//start of ontriggerexit
    {
        Disembark();//runs the disembark function
        onPlatform = false;//disables the onplatform bool 
    }//end of ontriggerexit
    private void LateUpdate()//start of lateupdate 
    {
        inCollider = false;//turns off the incollider bool. if the player is not in the collider this willm stay off to avoid any issues.
    }//end of lateupdate
    public void Disembark()// used for the player getting off the platform
    {
        if (player != null)//makes sure that the player is present
        {
            player.transform.parent = null;//unparents the player from the platform
            gameObject.SetActive(false);//disables the trigger
        }
    }// end of disembark function
}//end of the platform trigger script
