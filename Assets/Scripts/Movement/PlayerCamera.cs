using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]float sensitivityX;
    [SerializeField]float sensitivityY;
    [SerializeField] float dashFOVEffect;
    private BaseMovement playerMovement;
    private GameObject player;
    public Transform orientation;
    private Camera mainCam;
    private Rigidbody rb;
    private float storedCamFOV;
    private float averageLinearSpeed;
    //this allows for camera movement. this script has been particularly troublesome due to it sometimes forgetting what the player is if the movement script is changed too much at once. i have no idea what causes it.
    float xRotation;
    float yRotation;
    public float maxFovValue;
    private bool fallOffAllower;//allows the fov gain falloff to actually trigger without being reset every frame.
    private bool overExtensionBlocker; //prevents the above bool from being touched every frame.
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = FindFirstObjectByType<BaseMovement>();
        player = playerMovement.gameObject;
        orientation = playerMovement.transform;
        rb = player.GetComponent<Rigidbody>();
        mainCam = this.GetComponent<Camera>();
        storedCamFOV = mainCam.fieldOfView;
        maxFovValue += storedCamFOV;
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

        averageLinearSpeed = rb.linearVelocity.magnitude;
        mainCam.fieldOfView = mainCam.fieldOfView;
        if (!playerMovement.amDashing)
        {
            mainCam.fieldOfView = storedCamFOV + averageLinearSpeed;
        }
        else
        {
            if (!fallOffAllower)
            {
                mainCam.fieldOfView += dashFOVEffect;
                fallOffAllower = true;
            }

        }
        if (fallOffAllower && mainCam.fieldOfView > storedCamFOV)
        {
            mainCam.fieldOfView -= (Time.deltaTime * 25);
        }

        if (mainCam.fieldOfView > maxFovValue)//just to make sure we dont under or overflow the players fov.
        {
            mainCam.fieldOfView = maxFovValue;
        }
        else if (mainCam.fieldOfView < storedCamFOV)
        {
            mainCam.fieldOfView = storedCamFOV;
            fallOffAllower = false;
        }
    } 
}
