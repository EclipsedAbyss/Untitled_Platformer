using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{//start of the death HUD controller script
    [SerializeField] private GameObject gameOverScreen;//used to store the canvas that contains the death HUD
    [HideInInspector] public bool playerDie;//used to inform the game that the player is dead
    private void Start()//start of start
    {
        gameOverScreen.SetActive(false);//disables the death hud, as the player shouldnt be dead at this point.
    }//end of start
   private void Update()//start of update
   {
        if (playerDie)//checks if the player is dead
        {
            Time.timeScale = 0;//stops time
            gameOverScreen.SetActive(true);//enables the death screen
            if (Input.anyKeyDown)//detects any button presses to restart
            {
                Time.timeScale = 1;//resumes time
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//reloads the active scene.
            }
        }
   }//end of update
}// end of the death HUD controller
