using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    //this script is entirely focused on the main heads up display while partaking in gameplay.
    [SerializeField] TextMeshProUGUI chargeCounter;
    [SerializeField] BaseMovement playerMovementInfo;
    [SerializeField] Slider chargeTimer;
    void Start()
    {
        
    }

    void Update()
    {
        chargeCounter.text = playerMovementInfo.dashCount.ToString("Charges Remaining: 0");
        if (playerMovementInfo.dashCount != 9)
        {
            chargeTimer.value = playerMovementInfo.dashRechargeInterval;//this allows the recharge time to display.
        }
        else
        {
            chargeTimer.value = playerMovementInfo.dashAccumulationThreshhold;
        }

    }
}
