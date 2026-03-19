using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    //this script is entirely focused on the main heads up display while partaking in gameplay.
    public GameObject HUDCanvas;
    [SerializeField] TextMeshProUGUI chargeCounterRaw;
    [SerializeField] BaseMovement playerMovementInfo;
    [SerializeField] Slider chargeTimer;
    [SerializeField] Slider chargeDisplay;

    void Update()
    {
        chargeDisplay.value = playerMovementInfo.dashCount;
        chargeCounterRaw.text = playerMovementInfo.dashCount.ToString("Charges Remaining: 0");
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
