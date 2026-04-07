using UnityEngine;

public class FPSLock : MonoBehaviour//start of the FPS Locker script
{
    public int defaultTarget;//the defaulttarget fps
    private StoredSettings storedFPS;//the manually set FPS target by the player
    public float timetoLagCheck = 0.5f;
    private void Awake()//start of awake
    {
        storedFPS = FindFirstObjectByType<StoredSettings>();//chamges the stored fps to the fps value set by the user
        QualitySettings.vSyncCount = 0;//disables vsync
        Application.targetFrameRate = defaultTarget;//sets the default target framerate to the actual framerate target
        LagChecker();
    }//end of awake

    private void LagChecker()//start of update
    {
        if (Application.targetFrameRate != storedFPS.storedFPS && !storedFPS.FPSUncap)//checks if the active framerate target alligns with the set target, alongside if the fps is capped
        {
            Application.targetFrameRate = (int)storedFPS.storedFPS;//if it is not, change it to align
        }
        else if (storedFPS.FPSUncap)//or if your computer is beefy enough
        {
            Application.targetFrameRate = -1;//uncap the framerate
        }
        Invoke(nameof(LagChecker),timetoLagCheck);
    }//end of update
    private void OnEnable()//start of onenable
    {
        LevelCompletionTracker[] duplicates = FindObjectsByType<LevelCompletionTracker>(FindObjectsSortMode.None);//used to check if a levelcompletiontracker already exists
        if (duplicates.Length > 1)//if another level completion tracker already exists
        {
            Destroy(gameObject);//delete this one
        }
    }//end of onenable
    private void Start()//start of start
    {
        DontDestroyOnLoad(gameObject);//marks the object to not be destroyed on load. ran seperately from the duplicate check to avoid any possible issues, includes added information scripts.

    }//end of start
}//end of the FPS Locker script
