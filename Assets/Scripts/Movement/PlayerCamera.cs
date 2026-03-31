using UnityEngine;

public class PlayerCamera : MonoBehaviour//this controls the players camera
{//start of player camera script
    [SerializeField]float sensitivityX;//the manually set X sensitivity
    [SerializeField]float sensitivityY;//the manually set Y sensitivity
    [SerializeField] float dashFOVEffect;//the force of the FOV effects
    private float storedDashFOVEffect;
    private BaseMovement playerMovement;//the player movement script
    private GameObject player;//the player itself
    public Transform orientation;//the orientation of the camera 
    private Camera mainCam;//the camera this is on
    private Rigidbody playerRB;//the players rigidbody
    private float storedCamFOV;//the stored camera FOV to allow it to reset to the default value
    private float averageLinearSpeed;//the players average speed
    //this allows for camera movement. this script has been particularly troublesome due to it sometimes forgetting what the player is if the movement script is changed too much at once. i have no idea what causes it.
    float xRotation;//the cameras active x rotation
    float yRotation;//the cameras active y rotation
    public float maxFovValue;//the max fov that can be induced from the fov effects
    public float FOVEffectDelayTime;//the time that the fov effects will be delayed
    private bool falloffAllower;//allows the fov gain falloff to actually trigger without being reset every frame.
    private void Start()//start of start
    {
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor
        Cursor.visible = false;//hides the cursor
        playerMovement = FindFirstObjectByType<BaseMovement>();//finds and stoes the player movement script
        player = playerMovement.gameObject;//stores the player object using base movement as reference
        orientation = player.transform;//gets the orientation of the camera based on the orientation of the players gameobject
        playerRB = player.GetComponent<Rigidbody>();//stores the players rigidbody for velocity calculations
        mainCam = this.GetComponent<Camera>();//gets the camera script from the camera
        storedCamFOV = mainCam.fieldOfView;//stores the cameras base FOV
        maxFovValue += storedCamFOV;//adds the max camera FOV to the base fov for the actual maximum, allowing FOV changes
    }//end of start

    private void Update()//start of update
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;//gets the mouses X position and averages its speed, before multiplying that by its sensitivity
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;//gets the mouse Y position and averages its speed, before multiplying that by its sensitivity 

        yRotation += mouseX;//adds the cameras y rotation to the mouses X position

        xRotation -= mouseY;//subtracts the cameras x rotation from the mouses Y position

        xRotation = Mathf.Clamp(xRotation, -90, 90);//caps the players vertical look angles
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);//sets the cameras rotation to one relative to the mouses movement 
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);//rotates the player to match the camera
        // basic camera movement, not attatched to the player as the camera jittered severely when it was.

        averageLinearSpeed = playerRB.linearVelocity.magnitude / 3;//gets the players average speed from the rigidbody 
        if (!playerMovement.amDashing)//checks if the player is not actively dashing
        {
            mainCam.fieldOfView = storedCamFOV + averageLinearSpeed; //applies the average linear speed as an fov effect
        }
        else//if the [player is actively dashing
        {
            if (!falloffAllower)//used for dashing to avoid it stacking
            {
                mainCam.fieldOfView += dashFOVEffect/3;//applies thedash FOV effect divided by 3
                Invoke(nameof(FOVEffectDelay), FOVEffectDelayTime);//invokes the fov effect delay so that the fov effect is more gradual
                falloffAllower = true;//enables the falloff allower
            }

        }
        if (falloffAllower && mainCam.fieldOfView > storedCamFOV)//chekcs if the falloff allower is enabled
        {
            mainCam.fieldOfView -= (Time.deltaTime);//if it is, slowly reduce FOV via deltatime
        }

        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, storedCamFOV, maxFovValue);//caps the cameras FOV to the miniumu and Maximum

        if (mainCam.fieldOfView == storedCamFOV)//cchekcs if the camera is at the base FOV
        {
            mainCam.fieldOfView = storedCamFOV;//if it is, reset it
            falloffAllower = false;//and stop the falloff
        }
        if (mainCam.fieldOfView < storedDashFOVEffect -2)//checks for drastic fov loss
        {
            mainCam.fieldOfView = storedDashFOVEffect;//attempts to make it less jarring
        }
    }//end of update
    private void FOVEffectDelay()//prevents the FOV change from being immediate and overall jarring.
    {
        mainCam.fieldOfView += dashFOVEffect / 2;//divides the fov effect force by 2 to soften the results
    }//end of the fov effect delay script

    private void LateUpdate()//start of lateupdate
    {
        storedDashFOVEffect = mainCam.fieldOfView;//this is used to prevent the fov from snapping still
    }//end of lateupdate
}//end of the player camera script
