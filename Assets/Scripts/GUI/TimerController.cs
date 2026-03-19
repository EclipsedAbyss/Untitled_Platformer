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
    private LevelCompletionTracker LCT;

    public float timerValue;

    private void Start()
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        if (LCT.isLevelCompleted == false)
        {
            timer.enabled = false;
        }
    }
    private void Update()
    {
        timerValue += Time.deltaTime;
        timer.text = timerValue.ToString("0:00.00");
    }
}
