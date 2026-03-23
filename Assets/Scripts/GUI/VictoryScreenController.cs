using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{
    public bool FinalLevel;
    [SerializeField] GameObject VictoryHud;
    [SerializeField] PauseMenuController pauseMenuButtons;
    [HideInInspector] public bool levelEnded;



    private void Start()
    {
        VictoryHud.SetActive(false);
    }

    public void LevelEnd()
    {
        Time.timeScale = 0;
        VictoryHud.SetActive(true);
        levelEnded = true;
    }

    public void Next()
    {
        Time.timeScale = 1;
        if (FinalLevel)
        {
            SceneManager.LoadScene(0);
        }
       else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void RestartEndScreen()
    {
        pauseMenuButtons.Restart();
    }

    public void QuitToMenuEndScreen()
    {
        pauseMenuButtons.QuitToMenu();
    }

    public void QuitToDesktopEndScreen()
    {
        pauseMenuButtons.QuitToDesktop();
    }

    public void OnLevelEnd()
    {
        VictoryHud.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
