using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    //this just prevents the player from falling infinitely in the worst case scenario of them cipping out of bounds
    private BaseMovement playerMovement;
    private GameObject player;
    public Vector3 respawnPosition;
    public float respawnPlane;
    void Start()
    {
        playerMovement = FindFirstObjectByType<BaseMovement>();
        player = playerMovement.gameObject;
    }


    void Update()
    {
        if (player.transform.position.y < respawnPlane)
        {
            player.transform.position = respawnPosition;
        }
    }
}
