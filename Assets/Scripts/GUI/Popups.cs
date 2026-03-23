using System.Data;
using UnityEngine;

public class Popups : MonoBehaviour//allows UI level popups to show up. meant for use in the tutorial only as a means to avid being disruptive.
{
    [SerializeField] private GameObject Popup;
    [SerializeField] private bool startOrEnd; //on is start, off is end.
    private void OnTriggerEnter(Collider other)
    {
        if (startOrEnd)
        {
            Popup.SetActive(true);
        }
        else
        {
            Popup.SetActive(false);
        }
        Destroy(gameObject);
    }
}
