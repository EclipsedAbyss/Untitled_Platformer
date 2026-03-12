using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI chargeCounter;
    [SerializeField] BaseMovement playerMovementInfo;
    void Start()
    {
        
    }

    void Update()
    {
        chargeCounter.text = playerMovementInfo.dashCount.ToString("Charges Remaining: 0");
    }
}
