using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timer;
    public TextMeshProUGUI timerResult;
    public TextMeshProUGUI timerResultText;
    public TextMeshProUGUI prevBestTime;
    public TextMeshProUGUI differenceInTime;
    public TextMeshProUGUI differenceInTimeText;
    public TextMeshProUGUI prevBestTimeText;
    private LevelCompletionTracker LCT;// about 75% of these fields are NOT used in this script, but rather are stored here to allow the Level COmpletion Tracker to access them all without running more then one findObject.

    public float timerValue;
    public float MinuteTicker;

    private void Start()
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        if (LCT != null )
        {
            if (LCT.isLevelCompleted == false)
            {
                timer.enabled = false;//disables the timer if this is the players first time in the level.
            }
        }
    }
    private void Update()
    {
        timerValue += Time.deltaTime;
        MinuteTicker += Time.deltaTime;
        timer.text = timerValue.ToString("0:00.00");//simply increments the timer.
        if (MinuteTicker >= 60)//this prevents Minutes from containing 100 seconds, which last I checked, is very much not how time works.
        {
            MinuteTicker = 0;
            timerValue += 40;
        }
    }
}
