using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]float sensitivityX;
    [SerializeField]float sensitivityY;
    private BaseMovement playerMovement;
    private GameObject player;
    public Transform orientation;
    //this allows for camera movement. this script has been particularly troublesome due to it sometimes forgetting what the player is if the movement script is changed too much at once. i have no idea what causes it.
    float xRotation;
    float yRotation;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = FindFirstObjectByType<BaseMovement>();
        player = playerMovement.gameObject;
        orientation = playerMovement.transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        // basic camera movement, not attatched to the player as the camera jittered severely when it was.
    } 
}
