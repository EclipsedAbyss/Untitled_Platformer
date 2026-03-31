using UnityEngine;

public class VictoryTrigger : MonoBehaviour//this is used to detect the player reaching the end of the level.
{//start of the victory trigger script
    private VictoryScreenController getVictoryController;//used to store the victory ui controller
    private LevelCompletionTracker LCT;//used to store the level completion tracker
    private HUDController HUD;//used to store the HUD controller
    private void Start()//start of start
    {
        getVictoryController = FindFirstObjectByType<VictoryScreenController>();//gets the victory screen UI controller
        LCT = FindFirstObjectByType<LevelCompletionTracker>();//stores the level completion tracker
        HUD = FindFirstObjectByType<HUDController>();//stores the player HUD controller

    }//end of start
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object is the player
        {
            HUD.HUDCanvas.SetActive(false);//disables the players HUD
            getVictoryController.OnLevelEnd();//runs the victory screen controllers onlevelend function
            LCT.OnLevelCompletion();//runs the level completion trackers level completion function
        }//end of ontriggerenter
    }
}//end of the victory trigger script
