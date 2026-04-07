using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionTracker : MonoBehaviour//this script is used to store best times, level completion states, and the like. the game CAN run without it but a lot of things would stop working.
{//start of the level completion tracker script
    public float currentLevel;//the current loaded scene 
    [HideInInspector]public float level1_BestTime = -1;//the stored best time for level 1. -1 is used by default because it is very easy to differentiate from an actual plausable time.
    [HideInInspector] public int level1_Rank;
    [HideInInspector] public float level2_BestTime = -1;//the stored best time for level 2
    [HideInInspector] public int level2_Rank;
    [HideInInspector] public float level3_BestTime = -1;//the stored best time for level 3
    [HideInInspector] public int level3_Rank;
    [HideInInspector] public float level4_BestTime = -1;//the stored best time for level 4
    [HideInInspector] public int level4_Rank;
    [HideInInspector] public int rank;
    [HideInInspector] public float timeForS;
    [HideInInspector] public float timeForA;
    [HideInInspector] public float timeForB;
    [HideInInspector] public float timeForC;
    public bool isLevelCompleted;//used to check if the level is complete. if it is not the timer stays disabled
    public float currentLevelBestTime;//used to store the best time of the current loaded scene
    private TimerController timer;//used to get the times used
    private LevelTimeReqs rankRequirements;

    private string rankLabels = "SABCD";


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

    public void GetLevelIndex()//start of the level checker function. this is used to figure out the current active level. it is fired from the level load checker which fires this function and then is removed from the scene
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;//gets the current levels build index
        timer = FindFirstObjectByType<TimerController>();//finds the timer in this scene
        rankRequirements = FindFirstObjectByType<LevelTimeReqs>();
        timeForS = rankRequirements.timeForS;
        timeForA = rankRequirements.timeForA;
        timeForB = rankRequirements.timeForB;
        timeForC = rankRequirements.timeForC;

        switch (currentLevel)//used to get the information relative to the current scene
        {
            case 1://a sceneindex of 1 is the tutorial
                Tutorial();//so run the functions needed for the tutorial and get its stored info
                break;//else

            case 2://a scene index of 2 is level 1
                LevelOne();//so run the functions needed for level 1 and get its stored info
                break;//else

            case 3://a scene index of 3 is level 2
                LevelTwo();//so run the functions needed for level 2 and get its stored info
                break;//else

            case 4://a scene index of 4 is level 3
                LevelThree();//so run the functions needed for level 3 and get its stored info
                break;//else

            case 5://a scene index of 5 is level 4.
                LevelFour();//so run the functions needed for level 4 and get its stored info
                break;//else

                    default://if the scene index has no corresponding level we
                //do nothing.
            break;//end

        }
    }

    public void Tutorial()//function used for storing info related to the tutorial
    {
        //do nothing, as the tutorial should not have a timer or be focused on speedy clears.
    }//end of the tutorial info function
    public void LevelOne()//function used for storing info related to level 1
    {
        if (level1_BestTime != -1)//checks if the level has a best time stored
        {
            isLevelCompleted = true;//if it does, mark the level as completed, enable timer
        }
        currentLevelBestTime = level1_BestTime;//caches the current levels best time
    }//end of the level 1 info function
    public void LevelTwo()//function used for storing info related to level 2
    {
        if (level1_BestTime != -1)//checks if the level has a best time stored
        {
            isLevelCompleted = true;//if it does, mark the level as completed, enable timer
        }
        currentLevelBestTime = level2_BestTime;//caches the current levels best time
    }//end of the level 2 info function
    public void LevelThree()//function used for storing info related to level 4
    {
        if (level1_BestTime != -1)//checks if the level has a best time stored
        {
            isLevelCompleted = true;//if it does, mark the level as completed, enable timer
        }
        else
        currentLevelBestTime = level3_BestTime;//caches the current levels best time
    }//end of the level 3 info function
    public void LevelFour()//function used for storing info related to level 4
    {
        if (level1_BestTime != -1)//checks if the level has a best time stored
        {
            isLevelCompleted = true;//if it does, mark the level as completed, enable timer
        }
        currentLevelBestTime = level4_BestTime;//caches the current levels best time
    }//end of the level 4 info function

    public void OnLevelCompletion()//this runs when the lvel itself is completed.
    {
        if (currentLevelBestTime != -1)//checks that the current level has a best time
        {
            timer.prevBestTime.text = currentLevelBestTime.ToString("0:00.00");//sets the end screens best time text tothat time
            timer.differenceInTime.text = Mathf.Abs(currentLevelBestTime - timer.timerValue).ToString("0:00.00");//sets the difference on the end screen to the difference between the current run and the prior run
        }
        else//if ther eis no best time
        {
            timer.prevBestTime.text = ("0:00.00");//marked best time is 0 as it didnt exist prior
        }

        if (timer.timerValue < timeForS)
        {
            rank = 0;
        }
        else if (timer.timerValue < timeForA)
        {
            rank = 1;
        }
        else if (timer.timerValue < timeForB )
        {
            rank = 2;
        }
        else if (timer.timerValue < timeForC )
        {
            rank = 3;
        }
        else
        {
            rank = 4;
        }

        if (timer.timerValue != -1 && isLevelCompleted == false || timer.timerValue < currentLevelBestTime)//checks if this is the first run or if this run was faster then the prior one
        {
            switch (currentLevel)// detects the current level and uses its specific stored time values.
            {
                case 2://if the loaded scene index is 2
                    level1_BestTime = timer.timerValue;//update the best time for level 1 to the current run
                    level1_Rank = rank;
                    break;//else

                case 3://if the loaded scene index is 3
                    level2_BestTime = timer.timerValue;//update the best time for level 2 to the current run
                    level2_Rank = rank;
                    break;//else

                case 4://if the loaded scene index is 4
                    level3_BestTime = timer.timerValue;//update the best time for level 3 to the current run
                    level3_Rank = rank;
                    break;//else

                case 5://if the loaded scene index is 5
                    level4_BestTime = timer.timerValue;//update the best time for level 4 to the current run
                    level4_Rank = rank;
                    break;//else

                default://if the loaded scene is not registered, it won't have a timer and can be ignored
                    //do nothing.
                    break;//end

            }



            if (isLevelCompleted)//checks if the level has been completed, if so and the best time is better, give positive feedback
            {
                timer.differenceInTime.color = Color.green;//mark the difference display as green to show its a good difference
                timer.differenceInTimeText.color = Color.green;//alongside its accompanied text
                timer.prevBestTimeText.text = ("Previous Best Time");//changes best time to state that the prior best time is no longer the best time
                timer.timerResultText.text = ("New Best Time");//marks this run as your current best time in the text

            }
        }
        else//if the timer is worse and this isnt your first run
        {
            timer.differenceInTime.color = Color.red;//mark the difference in red to show it is not an improvement
            timer.differenceInTimeText.color = Color.red;//alogside its accompanied text

        }
            timer.timerResult.text = timer.timerValue.ToString("0:00.00");//simply sets the text on the end screen for the current run to the proper display
        timer.rankDisplay.text = rankLabels[rank].ToString();
    }
}//end of the level completion tracker script
