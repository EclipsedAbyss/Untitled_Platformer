using UnityEngine;

public class LevelLoaded : MonoBehaviour
{
    private LevelCompletionTracker LCT;//this script simply allows for a function to be run prior to start in a script that is never destroyed.
    private void OnEnable()
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        if (LCT != null )
        {
            LCT.GetLevelIndex();
        }
        else
        {
            Debug.Log("LCT is Missing! ignore if actively testing a level.");
        }
            Destroy(gameObject);
    }
}
