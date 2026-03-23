using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused;
    private bool OverFlowBlock;
    public bool resumePress;
    private DeathHudController playerDeathTracker;
    [SerializeField] GameObject pauseHUD;
    [SerializeField] VictoryScreenController VSC;

    private void Start()
    {
        playerDeathTracker = FindFirstObjectByType<DeathHudController>();
    }
    private void Update()
    {
        if (!VSC.levelEnded)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && playerDeathTracker.playerDie == false || resumePress == true)
            {
                if (!paused)
                {
                    Time.timeScale = 0;
                    paused = true;
                    Cursor.lockState = CursorLockMode.None;
                    pauseHUD.SetActive(true);
                    Cursor.visible = true;
                    resumePress = false;
                }
                else
                {
                    Time.timeScale = 1;
                    paused = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    pauseHUD.SetActive(false);
                    Cursor.visible = false;
                    resumePress = false;
                }
            }
        }
    }

    public void Resume()
    {
        paused = true;
        resumePress = true;
    }

    public void Restart()
    {
        paused = true;
        resumePress = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        paused = true;
        resumePress = true;
        SceneManager.LoadScene(0);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
