using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{//start of the pause menu controller script
    private bool paused;//bool to allow the pause menu to toggle
    public bool resumePress;//bool to register when resume is pressed
    private DeathHudController playerDeathTracker;//used to avoid pausing on the death screen
    private VictoryScreenController playerVictoryTracker;//used to track if the player has ended the level or not
    [SerializeField] GameObject pauseUI;//used to store the HUD for the pause menu


    private void Start()//start of start
    {
        playerDeathTracker = FindFirstObjectByType<DeathHudController>();//finds the death hud controller so it can figure out if the player is dead or not
        playerVictoryTracker = FindFirstObjectByType<VictoryScreenController>();//finds the victory hud controller so the game can verify if the level has ended or not
    }//end of start
    private void Update()//start of update
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerDeathTracker.playerDie && !playerVictoryTracker.levelEnded || resumePress == true)//checks if: the player has not died, the level is not over, and the pause button has been pressed.
        {//or, if the resume bool has been set to true
            if (!paused)//checks if the game is not paused
            {
             Time.timeScale = 0;//freezes time
             paused = true;//sets the paused bool to true
             Cursor.lockState = CursorLockMode.None;//unlocks the cursor
             pauseUI.SetActive(true);//enables the pause UI
             Cursor.visible = true;//makes the cursor visible
             resumePress = false;//turns off the resume press bool
            }
            else//if the game is paused
            {
             Time.timeScale = 1;//resume time
                paused = false;//set paused toggle to false
             Cursor.lockState = CursorLockMode.Locked;//lock the cursor
             pauseUI.SetActive(false);//disable the pause UI
             Cursor.visible = false;//hides the cursor
             resumePress = false;//turns off the resumepress bool
            }
        }
    }//end of update

    public void Resume()//start of the resume function
    {
        paused = true;//sets paused to true, mostly as a failsafe
        resumePress = true;//runs resume, properly unpausing the game
    }//end of the resume function

    public void Restart()//start of the restart function
    {
        paused = true;//sets paused to true as a failsafe
        resumePress = true;//sets resume press to true to disable all the pause functions
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//resets the loaded scene.
    }//end of the restart function

    public void QuitToMenu()//start of the quit to menu function
    {
        paused = true;//marks paused as true as a failsafe
        resumePress = true;//sets resume press to true to disable the pause functions
        SceneManager.LoadScene(0);//loads scene 0 (the main menu)
    }

    public void QuitToDesktop()//start of the quit to desktop function
    {
        Application.Quit();//quits the application
    }//end of the quit to desktop function
}//end of the pause menu controller script
