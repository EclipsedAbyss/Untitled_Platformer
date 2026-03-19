using UnityEngine;

public class LevelLoaded : MonoBehaviour
{
    private LevelCompletionTracker LCT;
    private void OnEnable()
    {
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        LCT.GetLevelIndex();
        Destroy(gameObject);
    }
}
