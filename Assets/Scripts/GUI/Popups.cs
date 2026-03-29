using UnityEngine;

public class Popups : MonoBehaviour//allows UI level popups to show up. meant for use in the tutorial only as a means to avid being disruptive.
{//start of popup script
    [SerializeField] private GameObject Popup;//the canvas used for this popup
    [SerializeField] private bool startOrEnd; //on is start, off is end.
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (startOrEnd)//chekcs which trigger this is
        {
            Popup.SetActive(true);//if its start, activates the popup
        }
        else//otherwise
        {
            Popup.SetActive(false);//if its end, disable the popup
        }
        Destroy(gameObject);//destroys the trigger after to avoid stacking popups or them flooding the screen.
    }//end of ontriggeerenter
}//end of popup script
