using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenController : MonoBehaviour
{
    [SerializeField] GameObject VictoryHud;
    [SerializeField] PauseMenuController pauseMenuButtons;

    private void Start()
    {
        VictoryHud.SetActive(false);
    }

    public void LevelEnd()
    {
        Time.timeScale = 0;
        VictoryHud.SetActive(true);
    }

    public void Next()
    {
       

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

}
