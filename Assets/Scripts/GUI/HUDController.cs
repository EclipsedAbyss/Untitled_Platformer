using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{//start of the HUD controller script
    //this script is entirely focused on the main heads up display while partaking in gameplay.
    public GameObject HUDCanvas;//used to store the canvas containing the heads up display
    [SerializeField] TextMeshProUGUI chargeCounterRaw;//used to log the charges remaining in raw text. primarily for debugging
    [SerializeField] BaseMovement playerMovementInfo;//used to store the player movement script
    [SerializeField] Slider chargeTimer;//used to display the time between recharges
    [SerializeField] Slider chargeDisplay;//used to display the chargesremaining

    void Update()//start of update
    {
        chargeDisplay.value = playerMovementInfo.chargeCount;//used to set the visual charge display accurately
        chargeCounterRaw.text = playerMovementInfo.chargeCount.ToString("Charges Remaining: 0");//used for the debug text display
        if (playerMovementInfo.chargeCount != 9)//used to check if the player has the maximum amount of charges
        {
            chargeTimer.value = playerMovementInfo.chargeRechargeInterval;//this allows the recharge time to display.
        }
        else//if they do have the max amount of stored charges
        {
            chargeTimer.value = playerMovementInfo.chargeAccumulationThreshhold;//fully fill the timer display instead of letting it fill up aimlessly forever
        }

    }//end of update
}//end of the HUD controller script
