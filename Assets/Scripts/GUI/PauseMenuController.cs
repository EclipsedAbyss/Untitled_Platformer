using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private bool paused;
    private bool OverFlowBlock;
    private bool resumePress;
    private DeathHudController playerDeathTracker;
    [SerializeField] GameObject pauseHUD;

    private void Start()
    {
        playerDeathTracker = FindFirstObjectByType<DeathHudController>();
    }
    private void Update()
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

    public void Resume()
    {
        resumePress = true;
    }

    public void QuitToMenu()
    {
        //TBD
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
