using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelCompletionTracker : MonoBehaviour
{
    public float currentLevel;
    [HideInInspector]public float level1_BestTime = -1;
    [HideInInspector] public float level2_BestTime = -1;
    [HideInInspector] public float level3_BestTime = -1;
    [HideInInspector] public float level4_BestTime = -1;
    public bool isLevelCompleted;
    public float currentLevelBestTime;
    private TimerController timer;


    private void OnEnable()
    {
        LevelCompletionTracker[] duplicates = FindObjectsByType<LevelCompletionTracker>(FindObjectsSortMode.None);
        if (duplicates.Length > 1)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
      
    }

    public void GetLevelIndex()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        timer = FindFirstObjectByType<TimerController>();

        switch (currentLevel)
        {
            case 1:
                Tutorial();
            break;

            case 2:
                LevelOne();
            break;

            case 3:
                LevelTwo();
            break;

            case 4:
                LevelThree();
            break;

            case 5:
                LevelFour();
            break;

            default:
                //do nothing.
            break;
        }
    }

    public void Tutorial()
    {
        //do nothing.
    }
    public void LevelOne()
    {
        if (level1_BestTime != -1)
        {
            isLevelCompleted = true;
        }
        currentLevelBestTime = level1_BestTime;
    }
    public void LevelTwo()
    {
        if (level1_BestTime != -1)
        {
            isLevelCompleted = true;
        }
        currentLevelBestTime = level2_BestTime;
    }
    public void LevelThree()
    {
        if (level1_BestTime != -1)
        {
            isLevelCompleted = true;
        }
        else

            currentLevelBestTime = level3_BestTime;
    }
    public void LevelFour()
    {
        if (level1_BestTime != -1)
        {
            isLevelCompleted = true;
        }
        currentLevelBestTime = level4_BestTime;
    }

    public void OnLevelCompletion()
    {
        if (currentLevelBestTime != -1)
        {
            timer.prevBestTime.text = currentLevelBestTime.ToString("0:00.00");
            timer.differenceInTime.text = Mathf.Abs(currentLevelBestTime - timer.timerValue).ToString("0:00.00");
        }
        else
        {
            timer.prevBestTime.text = ("0:00.00");
        }

            
        if (timer.timerValue != -1 && isLevelCompleted == false || timer.timerValue < currentLevelBestTime )
        {
            switch (currentLevel)// detects the current level and uses its specific stored time values.
            {
                case 2:
                    level1_BestTime = timer.timerValue;
                break;

                case 3:
                    level2_BestTime = timer.timerValue;
                break;

                case 4:
                    level3_BestTime = timer.timerValue;
                break;

                case 5:
                    level4_BestTime = timer.timerValue;
                break;

                default:
                    //do nothing.
                break;

            }
            if (isLevelCompleted)
            {
                timer.differenceInTime.color = Color.green;
                timer.differenceInTimeText.color = Color.green;
                timer.prevBestTimeText.text = ("Previous Best Time");
                timer.timerResultText.text = ("New Best Time");
            }
        }
        else
        {
            timer.differenceInTime.color = Color.red;
            timer.differenceInTimeText.color = Color.red;
            
        }
            timer.timerResult.text = timer.timerValue.ToString("0:00.00");
    }
}
