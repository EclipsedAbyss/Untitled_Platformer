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
    public float FOVEffectDelayTime;
    private bool fallOffAllower;//allows the fov gain falloff to actually trigger without being reset every frame.
    private bool overExtensionBlocker; //prevents the above bool from being touched every frame.
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = FindFirstObjectByType<BaseMovement>();
        player = playerMovement.gameObject;
        orientation = player.transform;
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

        averageLinearSpeed = rb.linearVelocity.magnitude / 3;
        mainCam.fieldOfView = mainCam.fieldOfView;
        if (!playerMovement.amDashing)
        {
            mainCam.fieldOfView = storedCamFOV + averageLinearSpeed; //for non boosting FOV effects
        }
        else
        {
            if (!fallOffAllower)//used for dashing to avoid it stacking
            {
                mainCam.fieldOfView += dashFOVEffect/3;
                Invoke(nameof(FOVEffectDelay), FOVEffectDelayTime);
                fallOffAllower = true;
            }

        }
        if (fallOffAllower && mainCam.fieldOfView > storedCamFOV)//reduces FOV shift
        {
            mainCam.fieldOfView -= (Time.deltaTime);
        }

        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, storedCamFOV, maxFovValue);

        if (mainCam.fieldOfView == storedCamFOV)
        {
            mainCam.fieldOfView = storedCamFOV;
            fallOffAllower = false;
        }
    }
    private void FOVEffectDelay()//prevents the FOV change from being immediate and overall jarring.
    {
        mainCam.fieldOfView += dashFOVEffect / 2;
    }
}
