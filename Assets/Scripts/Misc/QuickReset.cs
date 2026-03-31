using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickReset : MonoBehaviour//used to quickly restart a level. less debug more quality of life
{//start of quick reset script
    void Update()//start of update
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.timeScale != 0)//if the player presses R and the game is not paused
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);//simply reloads the active scene. 
        }
    }//end of update
}//end of quick reset script
