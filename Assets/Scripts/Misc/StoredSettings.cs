using UnityEngine;

public class StoredSettings : MonoBehaviour
{
    public float storedFOV;//the players set FOVInput Value
    public float storedSens;//the players set camera sensitivityInput
    public float storedFPS;//the players set FPS target
    public bool VSYNC;//used so players can toggle vsync
    public bool FPSUncap;//used so technologically well off players can uncap the framerate


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

}
