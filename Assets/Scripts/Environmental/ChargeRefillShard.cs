using UnityEngine;

public class ChargeRefillShard : MonoBehaviour
{//beginning of shard script
    private BaseMovement player;//gets the players movement script
    [SerializeField] private float strength;//tyhe manually set number of charges the shard will restore
    [SerializeField] private float chargeTime;//the manually set time it takes for the shard to recharge
    [SerializeField] private Material activeMaterial;//the material used when the charge is active
    [SerializeField] private Material inactiveMaterial;//the material used when the shard is inactive
    [SerializeField] private GameObject particles;//stores a prefab for the particles that allow visual feedback for collecting a charge
    [SerializeField] private GameObject overlayParticles;//stores a prefab for the particles that display as a screen effect when collecting a charge
    private Transform overlay;//hte position of the camera used for screen effects
    private bool charged = true;//used to tell the script if the shard is charged or not.

    private void Awake()//start of awake
    {
        overlay = FindFirstObjectByType<OverlayPos>().transform;//gets the position of the screen overlay camera
    }//end of awake
    private void OnTriggerEnter(Collider other)//start of ontriggerenter
    {
        if (charged)//if the shard is charged
        {
            if (other.GetComponent<BaseMovement>() != null)//if the colliding object is the player
            {
                player = other.GetComponent<BaseMovement>();//sets the player field to the player
                player.chargeCount += strength;//adds the number used for strength to the dash count
                charged = false;//sets the charged state to false
                gameObject.GetComponent<MeshRenderer>().material = inactiveMaterial;//sets the material of the shard to the inactive material
                Invoke(nameof(Recharge), chargeTime);//starts the recharge delay
                Instantiate(particles, this.transform);//spawns the world particles on the shard
                Instantiate(overlayParticles, overlay);//spawns the screen effect particles
                player = null;//reset player field to avoid inconsistencies with consecutive uses
            }
        }
    }//end of ontriggerenter

    private void Recharge()//start of recharge function
    {
        charged = true;//sets the charged bool to true
        gameObject.GetComponent<MeshRenderer>().material = activeMaterial;//swaps the material back to the active shard
    }//end of recharge function
}//end of shard script
