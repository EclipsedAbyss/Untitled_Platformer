using UnityEngine;

public class FaliureTrigger : MonoBehaviour
{//start of faliure trigger script
    private DeathHudController getDeathHud;
    private void Start()//start of start
    {
        getDeathHud = FindFirstObjectByType<DeathHudController>();//gets the death hud. due to the level components needing to be able to be dropped in willy nilly, the field is not set before runtime to make the process easier.
    }//end of start
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object is the player
        {
            getDeathHud.playerDie = true;//if it is, "kills" the player and pulls up the corresponding UI.
        }
    }//end of ontriggerenter
}//end of faliure trigger script
