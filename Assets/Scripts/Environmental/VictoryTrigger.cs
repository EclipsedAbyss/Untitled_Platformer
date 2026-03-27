using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private VictoryScreenController getVictoryController;
    private LevelCompletionTracker LCT;
    private HUDController HUD;
    private void Start()
    {
        getVictoryController = FindFirstObjectByType<VictoryScreenController>();
        LCT = FindFirstObjectByType<LevelCompletionTracker>();
        HUD = FindFirstObjectByType<HUDController>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMovement>() != null)
        {
            HUD.HUDCanvas.SetActive(false);
            getVictoryController.OnLevelEnd();
            LCT.OnLevelCompletion();
        }
    }
}
