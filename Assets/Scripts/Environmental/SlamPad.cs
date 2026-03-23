using UnityEngine;

public class SlamPad : MonoBehaviour
{
    [SerializeField] private bool singleUse;
    [SerializeField] private float coolDown;
    private float coolDownStored;
    private BaseMovement playerMovement;
    private HitStopEffects hitstopfirer;

    private void Start()
    {
        hitstopfirer = FindFirstObjectByType<HitStopEffects>();
        coolDownStored = coolDown;
    }
    private void OnTriggerEnter(Collider other)
    {
     if (coolDown < 0)
        {
            if (other.GetComponent<BaseMovement>() != null)
            {
                playerMovement = other.GetComponent<BaseMovement>();
                if (playerMovement.amDashing)
                {
                    playerMovement.dashCount = playerMovement.dashCountStored;//refills the players dashes.
                    hitstopfirer.OnHitStopStart();//fires the hit stop FX script.
                    playerMovement = null;
                    coolDown = coolDownStored;
                    if (singleUse)
                    {
                        Destroy(this);
                    }
                }
                else
                {
                    playerMovement = null;
                }

            }
        }
    }
    private void Update()
    {
        
            coolDown -= Time.deltaTime;//reduces the cooldown if the pad isnt a one time use (this should be used primarily in the tutorial to avoid frustration, however a visual cue would be nice.

    }
}
