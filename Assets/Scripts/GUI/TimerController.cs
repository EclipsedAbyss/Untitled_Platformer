using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{//start of timer controller script

    [SerializeField] TextMeshProUGUI timer;//the text displayed on the HUD for the timer
    public TextMeshProUGUI timerResult;//the timer display for the current run within the end screen UI
    public TextMeshProUGUI timerResultText;//the text beneath the display to state what it is
    public TextMeshProUGUI prevBestTime;//the timer display for the current/previous best time on the current level
    public TextMeshProUGUI prevBestTimeText;//the text beneath the display for the prev best time to state what it is
    public TextMeshProUGUI differenceInTime;//the timer display that shows the difference between your best/previous best and your current time.
    public TextMeshProUGUI differenceInTimeText;//the text beneath the difference display that states what it is
    public TextMeshProUGUI rankDisplay;
    private LevelCompletionTracker LCT;// about 75% of these fields are NOT used in this script, but rather are stored here to allow the Level COmpletion Tracker to access them all without running more then one findObject.

    public float timerValue;//the current time passed
    public float MinuteTicker;//the internal timer to avoid second underflow

    private void Start()//start of start
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();//locates and stores the level completion tracker
        if (LCT != null )//if the level completion tracker does not exist
        {
            if (LCT.isLevelCompleted == false)//or if the current level has not been completed
            {
                timer.enabled = false;//disables the timer if this is the players first time in the level.
            }
        }
    }//end of start
    private void Update()//start of update
    {
        timerValue += Time.deltaTime;//increases the value of the internal timer
        MinuteTicker += Time.deltaTime;//increases the value of the seconds ticker
        timer.text = timerValue.ToString("0:00.00");//simply increments the visual timer.
        if (MinuteTicker >= 60)//this prevents Minutes from containing 100 seconds, which last I checked, is very much not how time works.
        {
            MinuteTicker = 0;//resets the miniute ticker
            timerValue += 40;//adds 40 to the timer to boost 60 seconds into display like its 1 miniute.
        }
    }//end of update
}//end of timer controller script
