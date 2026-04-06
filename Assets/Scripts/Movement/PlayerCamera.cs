using UnityEngine;

public class PlayerCamera : MonoBehaviour//this controls the players camera
{//start of player camera script
    public float sensitivity;//the camera sensitivityInput
    [SerializeField] float dashFOVEffect;//the force of the FOVInput effects
    private float storedDashFOVEffect;
    private BaseMovement playerMovement;//the player movement script
    private GameObject player;//the player itself
    public Transform orientation;//the orientation of the camera 
    private Camera mainCam;//the camera this is on
    private StoredSettings camSettings;
    private Rigidbody playerRB;//the players rigidbody
    [HideInInspector] public float storedCamFOV;//the stored camera FOVInput to allow it to reset to the default value
    private float averageLinearSpeed;//the players average speed
    //this allows for camera movement. this script has been particularly troublesome due to it sometimes forgetting what the player is if the movement script is changed too much at once. i have no idea what causes it.
    float xRotation;//the cameras active x rotation
    float yRotation;//the cameras active y rotation
    public float maxFovValue;//the max FOVInput that can be induced from the FOVInput effects
    public float FOVEffectDelayTime;//the time that the FOVInput effects will be delayed
    private bool falloffAllower;//allows the FOVInput gain falloff to actually trigger without being reset every frame.
    private void OnEnable()//start of OnEnable
    {
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor
        Cursor.visible = false;//hides the cursor
        playerMovement = FindFirstObjectByType<BaseMovement>();//finds and stoes the player movement script
        camSettings = FindFirstObjectByType<StoredSettings>();
        player = playerMovement.gameObject;//stores the player object using base movement as reference
        orientation = player.transform;//gets the orientation of the camera based on the orientation of the players gameobject
        playerRB = player.GetComponent<Rigidbody>();//stores the players rigidbody for velocity calculations
        mainCam = this.GetComponent<Camera>();//gets the camera script from the camera
        storedCamFOV = mainCam.fieldOfView;//stores the cameras base FOVInput
        maxFovValue += storedCamFOV;//adds the max camera FOVInput to the base FOVInput for the actual maximum, allowing FOVInput changes
    }//end of OnEnable

    
    private void Update()//start of update
    {

        if (camSettings == null)
        {
            Debug.Log("panic");
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (sensitivity * 10);//gets the mouses X position and averages its speed, before multiplying that by the sensitivityInput
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (sensitivity * 10);//gets the mouse Y position and averages its speed, before multiplying that by the sensitivityInput 

        yRotation += mouseX;//adds the cameras y rotation to the mouses X position

        xRotation -= mouseY;//subtracts the cameras x rotation from the mouses Y position

        xRotation = Mathf.Clamp(xRotation, -90, 90);//caps the players vertical look angles
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);//sets the cameras rotation to one relative to the mouses movement 
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);//rotates the player to match the camera
        // basic camera movement, not attatched to the player as the camera jittered severely when it was.

        averageLinearSpeed = playerRB.linearVelocity.magnitude / 3;//gets the players average speed from the rigidbody 
        if (!playerMovement.amDashing)//checks if the player is not actively dashing
        {
            mainCam.fieldOfView = storedCamFOV + averageLinearSpeed; //applies the average linear speed as an FOVInput effect
        }
        else//if the [player is actively dashing
        {
            if (!falloffAllower)//used for dashing to avoid it stacking
            {
                mainCam.fieldOfView += dashFOVEffect/3;//applies thedash FOVInput effect divided by 3
                Invoke(nameof(FOVEffectDelay), FOVEffectDelayTime);//invokes the FOVInput effect delay so that the FOVInput effect is more gradual
                falloffAllower = true;//enables the falloff allower
            }

        }
        if (falloffAllower && mainCam.fieldOfView > storedCamFOV)//chekcs if the falloff allower is enabled
        {
            mainCam.fieldOfView -= (Time.deltaTime);//if it is, slowly reduce FOVInput via deltatime
        }

        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, storedCamFOV, maxFovValue);//caps the cameras FOVInput to the miniumu and Maximum

        if (mainCam.fieldOfView == storedCamFOV)//cchekcs if the camera is at the base FOVInput
        {
            mainCam.fieldOfView = storedCamFOV;//if it is, reset it
            falloffAllower = false;//and stop the falloff
        }
        if (mainCam.fieldOfView < storedDashFOVEffect -2)//checks for drastic FOVInput loss
        {
            mainCam.fieldOfView = storedDashFOVEffect;//attempts to make it less jarring
        }
        if (mainCam.fieldOfView > maxFovValue)//used to make absolutely CERTAIN that the FOVInput does not overflow.
        {
            mainCam.fieldOfView = maxFovValue - 10;//resets the FOVInput to avoid full overflow
        }

        if (storedCamFOV != camSettings.storedFOV || sensitivity != camSettings.storedSens && camSettings.storedSens != 0)//used to check if any of the camera settings have been changed
        {
            
            maxFovValue -= storedCamFOV;//
            storedCamFOV = camSettings.storedFOV;
            maxFovValue += storedCamFOV;
            mainCam.fieldOfView = storedCamFOV;
            sensitivity = camSettings.storedSens;
        }
        else if (camSettings.storedSens == 0)//checks if sensitivityInput is 0, if it is
        {
            camSettings.storedSens = sensitivity;//set it to the playyers set sensitivityInput. this mostly exists as a emergency run because the player should not at any point have 0 sensitivityInput
        }


    }//end of update
    private void FOVEffectDelay()//prevents the FOVInput change from being immediate and overall jarring.
    {
        mainCam.fieldOfView += dashFOVEffect / 2;//divides the FOVInput effect force by 2 to soften the results
    }//end of the FOVInput effect delay script

    private void LateUpdate()//start of lateupdate
    {
        storedDashFOVEffect = mainCam.fieldOfView;//this is used to prevent the FOVInput from snapping still
    }//end of lateupdate
}//end of the player camera script
