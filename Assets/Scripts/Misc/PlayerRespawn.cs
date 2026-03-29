using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{//start of player respawn script
    //this just prevents the player from falling infinitely in the worst case scenario of them cipping out of bounds
    private BaseMovement playerMovement;//gets theplayers movement in order to get the object
    private GameObject player;//the actual player
    public Vector3 respawnPosition;//the manually set respawn point (preferably set this inside the map.)
    public float respawnPlane;//the point where the player is deemed :out of bounds"
    void Start()//start of start
    {
        playerMovement = FindFirstObjectByType<BaseMovement>();//first we get the player's movement script
        player = playerMovement.gameObject;//then we get the associated gameobject
    }//end of start


    void LateUpdate()//start of lateupdate
    {
        if (player.transform.position.y < respawnPlane)//checks if the player has fallen below the set y position
        {
            player.transform.position = respawnPosition;//sets the player to the predefined respawn position.
        }
    }//end of lateupdate
}//end of player respawn script
