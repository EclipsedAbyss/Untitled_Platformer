using UnityEngine;

public class FaliureTrigger : MonoBehaviour
{//start of faliure trigger script
    private DeathUIController getDeathUI;//the death screen UI
    [SerializeField] private Vector3 relocatePosition;//the position the object will send loose rigidbodies to to avoid players standing on destroyed objects
    private void Start()//start of start
    {
        getDeathUI = FindFirstObjectByType<DeathUIController>();//gets the death hud. due to the level components needing to be able to be dropped in willy nilly, the field is not set before runtime to make the process easier.
    }//end of start
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (other.GetComponent<BaseMovement>() != null)//checks if the colliding object is the player
        {
            getDeathUI.playerDie = true;//if it is, "kills" the player and pulls up the corresponding UI.
        }
        else if (other.GetComponent<Rigidbody>() && !other.GetComponent<BaseMovement>())//checks if the colliding object is a non player rigidbody
        {
            other.transform.position = relocatePosition;//teleports it elsewhere
        }
        other = null;
    }//end of ontriggerenter
}//end of faliure trigger script
