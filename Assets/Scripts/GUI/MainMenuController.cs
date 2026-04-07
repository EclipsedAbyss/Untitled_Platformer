using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour//start of the script for controlling the main menu
{
    [SerializeField] GameObject menuMain;//the canvas for the actual menu
    [SerializeField] GameObject menuLevelSelect;//the canvas for the level select screen
    [SerializeField] TextMeshProUGUI level1TimeDisplay;//the display for the best time on level 1
    [SerializeField] TextMeshProUGUI level1RankDisplay;
    [SerializeField] TextMeshProUGUI level2TimeDisplay;//the display for the best time on level 2
    [SerializeField] TextMeshProUGUI level2RankDisplay;
    [SerializeField] TextMeshProUGUI level3TimeDisplay;//the display for the best time on level 3
    [SerializeField] TextMeshProUGUI level3RankDisplay;
    [SerializeField] TextMeshProUGUI level4TimeDisplay;//the display for the best time on level 4
    [SerializeField] TextMeshProUGUI level4RankDisplay;
    private LevelCompletionTracker LCT;//the level completion tracker

    private string ranks = "SABCD";//stores the letters displayed for level rankLabels
    private void Start()//start of start
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();//gets the level completion tracker and stores it
        Cursor.lockState = CursorLockMode.None;//unlocks the cursor
        Cursor.visible = true;//makes the cursor visible
        if (LCT.level1_BestTime > -1)//checks if a best time exists for level 1
        {
            level1TimeDisplay.text = LCT.level1_BestTime.ToString("0:00.00");//if it does, sets the level select time display for level 1 to it
            level1RankDisplay.text = ranks[LCT.level1_Rank].ToString();
        }
    
       
        if (LCT.level2_BestTime > -1)//checks if a best time exists for level 2
        {
            level2TimeDisplay.text = LCT.level2_BestTime.ToString("0:00.00");//if it does, sets the level select time display for level 2 to it
            level2RankDisplay.text = ranks[LCT.level2_Rank].ToString();
        }

        if (LCT.level3_BestTime > -1)//checks if a best time exists for level 3
        {
            level3TimeDisplay.text = LCT.level3_BestTime.ToString("0:00.00");//if it does, sets the level select time display for level 3 to it
            level3RankDisplay.text = ranks[LCT.level3_Rank].ToString();
        }

        if (LCT.level4_BestTime > -1)//checks if a best time exists for level 4
        {
            level4TimeDisplay.text = LCT.level4_BestTime.ToString("0:00.00");//if it does, sets the level select time display for level 4 to it
            level4RankDisplay.text = ranks[LCT.level4_Rank].ToString();
        }//this chain of ifs are here to change the best time on the level select while allowing it to be blank if not beaten. the tutorial has no timer as it is not meant to be replayed constantly
     
        
        menuLevelSelect.SetActive(false);//disables the level select UI
        
    }//end of start
    public void QuitPress()//start of the function for quitting
    {
        Application.Quit();//closes the application 
    }//end of the function for quitting

    public void LevelSelectPress()//start of level select press (displayed as start)
    {
        menuMain.SetActive(false);//disables the base menu
        menuLevelSelect.SetActive(true);//enables the level select
    }//end of the level select function

    public void ReturnToMenuPress()//start of the return to menu button function
    {
        menuMain.SetActive(true);//enables the main menu
        menuLevelSelect.SetActive(false);//disables the level select
    }//end of the return to menu button function

    public void OnTutorialPress()//start of the function for the tutorial (level 0) selection
    {
        SceneManager.LoadScene(1);//loads the tutorial
    }//end of the function for selecting the tutorial

    public void OnLevel1Press()//start of the function for the level 1 selection
    {
        SceneManager.LoadScene(2);//loads level 1
    }//end of the function for selecting level 1
    public void OnLevel2Press()//start of the function for the level 2 selection
    {
        SceneManager.LoadScene(3);//loads level 2
    }//end of the function for selecting level 2

    public void OnLevel3Press()//start of the function for the level 3 selection
    {
        SceneManager.LoadScene(4);//loads level 3
    }//end of the function for selecting level 3

    public void OnLevel4Press()//start of the function for the level 4 selection
    {
        SceneManager.LoadScene(5);//loads level 4
    }//end of the function for selecting level 4
}
