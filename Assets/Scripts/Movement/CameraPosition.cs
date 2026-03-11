using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] GameObject cameraLocation;
    void Update()
    {
        this.transform.position = cameraLocation.transform.position;
    }
}
