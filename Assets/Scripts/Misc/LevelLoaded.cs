using UnityEngine;

public class LevelLoaded : MonoBehaviour
{//start of the level loaded checker script
    private LevelCompletionTracker LCT;//this script simply allows for a function to be run prior to start in a script that is never destroyed.
    private void OnEnable()//start of onenable
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();//finds the level completion tracker
        if (LCT != null )//verifies that the level completion tracker exists
        {
            LCT.GetLevelIndex();//runs the get level index function within the level completion tracker. this is to get information about the scene that has just loaded
        }
        else//if the level completion tracker doesnt exist
        {
            Debug.Log("LCT is Missing! ignore if actively testing a level.");//logs the issue
        }
            Destroy(gameObject);//after this function has been run the script is no longer needed and is removed from memory.
    }//end of onenable
}//end of the level loaded checker script
