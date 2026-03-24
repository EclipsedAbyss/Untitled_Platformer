using UnityEngine;

public class ParticleRemovalTImer : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy < 0)
        {
            Destroy(gameObject);
        }
    }
}
