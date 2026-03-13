using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] GameObject cameraLocation;
    void Update()
    {
        this.transform.position = cameraLocation.transform.position;
    }
}
    // sets the camera to the players position.
    //it not being placed on the player is due to the camera freaking out in response to the rigidbody.
