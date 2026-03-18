using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private VictoryScreenController getVictoryController;
    private void Start()
    {
        getVictoryController = FindFirstObjectByType<VictoryScreenController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
