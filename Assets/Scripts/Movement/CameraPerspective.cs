using UnityEngine;

public class CameraPerspective : MonoBehaviour
{
    [SerializeField]private Transform cameraPositionFP;
    [SerializeField]private Transform cameraPositionTP;
    public bool cameraFP;

    // Update is called once per frame
    void Update()
    {
        if (cameraFP == true)
        {
            transform.position = cameraPositionFP.position;
        }
        else if (cameraFP == false)
        {
            transform.position = cameraPositionTP.position;
        }
    }
}
