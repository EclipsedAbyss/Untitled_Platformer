using UnityEngine;

public class ChargeRefillShard : MonoBehaviour
{
    private BaseMovement player;
    [SerializeField] private float strength;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<BaseMovement>();
        player.dashCount += strength;
        Destroy(gameObject);
    }
}
