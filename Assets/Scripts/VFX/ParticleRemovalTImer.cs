using UnityEngine;

public class ParticleRemovalTImer : MonoBehaviour//used for instantiated particles
{//start of script
    [SerializeField] private float timeToDestroy;//manually set value for the time the particle should last
    void Update()//start of update
    {
        timeToDestroy -= Time.deltaTime;//used to decrement the lifetime of the object
        if (timeToDestroy < 0)//once lifetime hits 0
        {
            Destroy(gameObject);//remove the object.
        }
    }//end of update
}//end of script
