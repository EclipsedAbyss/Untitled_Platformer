using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{//start of the victory screen controller script
    public bool FinalLevel;//manually set bool to tell the script it is the final level so it doesnt try to load a scene that doesnt exist.
    [SerializeField] GameObject VictoryUI;//the canvas for the VIctory UI
    [SerializeField] PauseMenuController pauseMenuButtons;//used to access the pause buttons to avoid having 2 near identical scripts for UI intractions.
    [HideInInspector] public bool levelEnded;//used to tell other scripts that the level is over



    private void Start()//start of start
    {
        VictoryUI.SetActive(false);//makes certain the UI for the victory screen is disabled
    }//end of start

    public void LevelEnd()//start of the levelend function
    {
        Time.timeScale = 0;
        VictoryUI.SetActive(true);
        levelEnded = true;
    }//end of the lvelend function

    public void Next()//start of the next function. this loads the next scene
    {
        Time.timeScale = 1;//unfreezes time
        if (FinalLevel)//if this is marked as the final level
        {
            SceneManager.LoadScene(0);//load the menu
        }
       else//if this is not the final level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//load the next scene
        }

    }//end of the next function

    public void RestartEndScreen()//on restart press in the end screen
    {
        pauseMenuButtons.Restart();// run the pause menu restart script, as itd have the same effect
    }//end of restart button for end screen

    public void QuitToMenuEndScreen()//on quit press in the end screen
    {
        pauseMenuButtons.QuitToMenu();//run the pause menu quit script, as itd have the same effect
    }//end of quite button for end screen

    public void QuitToDesktopEndScreen()//on quit to desktop press in the end screen
    {
        pauseMenuButtons.QuitToDesktop();//run the quit to desktop function from the pause menu as it would have the same effect
    }//end of quit to desktop button for end screen

    public void OnLevelEnd()//called by the level end point trigger
    {
        VictoryUI.SetActive(true);//enables the UI that displays the end of the level
        Time.timeScale = 0;//freezes time
        Cursor.lockState = CursorLockMode.None;//unlocks the cursor
        Cursor.visible = true;//makes the cursor visible

    }
}//end of the victory screen controller script
