using UnityEngine;

public class CameraPosition : MonoBehaviour
{//start of camera position script
    [SerializeField] GameObject cameraLocation;//gets the position of the object where the camera is supposed to be located at
    void Update()//start of update
    {
        this.transform.position = cameraLocation.transform.position;//moves the camera to the position it should be at, on the player.
    }//end of update
}//end of camera position script
    // sets the camera to the players position.
    //it not being placed on the player is due to the camera freaking out in response to the rigidbody.
