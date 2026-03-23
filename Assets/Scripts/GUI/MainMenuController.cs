using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuLevelSelect;
    [SerializeField] TextMeshProUGUI level1TimeDisplay;
    [SerializeField] TextMeshProUGUI level2TimeDisplay;
    [SerializeField] TextMeshProUGUI level3TimeDisplay;
    [SerializeField] TextMeshProUGUI level4TimeDisplay;
    private LevelCompletionTracker LCT;
    private void Start()
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (LCT.level1_BestTime > -1)
        {
            level1TimeDisplay.text = LCT.level1_BestTime.ToString("0:00.00");
        }
       
        if (LCT.level2_BestTime > -1)
        {
            level2TimeDisplay.text = LCT.level2_BestTime.ToString("0:00.00");
        }

        if (LCT.level3_BestTime > -1)
        {
            level3TimeDisplay.text = LCT.level3_BestTime.ToString("0:00.00");
        }

        if (LCT.level4_BestTime > -1)
        {
            level4TimeDisplay.text = LCT.level4_BestTime.ToString("0:00.00");
        }//this chain of ifs are here to change the best time on the level select while allowing it to be blank if not beaten. the tutorial has no timer as it is not meant to be replayed constantly
     
        
        menuLevelSelect.SetActive(false);
        
    }
    public void StartPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitPress()
    {
        Application.Quit();
    }

    public void LevelSelectPress()
    {
        menuMain.SetActive(false);
        menuLevelSelect.SetActive(true);
    }

    public void ReturnToMenuPress()
    {
        menuMain.SetActive(true);
        menuLevelSelect.SetActive(false);
    }

    public void OnTutorialPress()
    {
        SceneManager.LoadScene(1);
    }

    public void OnLevel1Press()
    {
        SceneManager.LoadScene(2);
    }

    public void OnLevel2Press()
    {
        SceneManager.LoadScene(3);
    }
    public void OnLevel3Press()
    {
        SceneManager.LoadScene(4);
    }

    public void OnLevel4Press()
    {
        SceneManager.LoadScene(5);
    }//i feel all these level based inputs are very clear as to wat they do.
}
