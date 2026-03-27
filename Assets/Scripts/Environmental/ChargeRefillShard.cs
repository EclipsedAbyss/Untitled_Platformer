using UnityEngine;

public class ChargeRefillShard : MonoBehaviour
{//beginning of shard script
    private BaseMovement player;
    [SerializeField] private float strength;
    [SerializeField] private float chargeTime;
    [SerializeField] Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;
    private bool charged = true;
    private void OnTriggerEnter(Collider other)
    {
        if (charged)
        {
            if (other.GetComponent<BaseMovement>() != null)
            {
                player = other.GetComponent<BaseMovement>();
                player.dashCount += strength;
                charged = false;
                gameObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
                Invoke(nameof(Recharge), chargeTime);
            }
        }
    }

    private void Recharge()
    {
        charged = true;
        gameObject.GetComponent<MeshRenderer>().material = activeMaterial;
    }
}//end of shard script
