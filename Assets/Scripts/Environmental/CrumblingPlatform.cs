using UnityEngine;

public class CrumblingPlatform : MonoBehaviour
{//start of crumbling platform script
    [SerializeField] GameObject platform;//stores the platform itself
    [SerializeField] float timeToCollapse;//the manually set time the platform lasts after contact
    [SerializeField] float timeToRegenerate;//the manually set time for the platform to come back
    [SerializeField] Material remainsMat;//the material that displays where the platform was
    private bool collapsed;//used to prevent contact with the broken platform causing it to crumble again prematurely
    private GameObject remains;//used to keep track of the object to show where the platform will return to
    private void OnCollisionEnter(Collision collision)// checks for contact
    {
        if (!collapsed)//makes sure the platform hasn't already collapsed
        {
            Invoke(nameof(Collapse), timeToCollapse);//triggers the collapse function
            collapsed = true;//marks the platform as collapsed
        }
    }//end of contact check

    private void Collapse()//start of collapse function
    {
        remains = Instantiate(platform);//creates the remains object (this shows where the platform will be placed after it resets)
        Destroy(remains.GetComponent<Collider>());//removes the collider from the remains object as it should not have it
        remains.GetComponent<MeshRenderer>().material = remainsMat;//sets the material of the remains object to avoid confusion with the active state
        platform.AddComponent<Rigidbody>();//allows the actual platform to collapse
        Invoke(nameof(Reappear), timeToRegenerate);//runs the platform reset function
    }//end of collapse function

    private void Reappear()//start of platform reset function
    {
        Destroy(platform.GetComponent<Rigidbody>());//removes the rigidbody from the platform
        platform.transform.position = remains.transform.position;//resets the platforms position
        platform.transform.rotation = remains.transform.rotation;//resets the platforms rotation
        Destroy(remains);//destroys the remains object
        collapsed = false;//marks collapsed as false
    }//end of platform reset function
}//end of crumbling platform script
