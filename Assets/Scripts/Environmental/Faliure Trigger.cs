using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class FaliureTrigger : MonoBehaviour
{
    private DeathHudController getDeathHud;
    private void Start()
    {
        getDeathHud = FindFirstObjectByType<DeathHudController>();//gets the death hud. due to the level components needing to be able to be dropped in willy nilly, the field is not set before runtime to make the process easier.
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMovement>() != null)
        {
            getDeathHud.playerDie = true;//this marks the player as dead and pulls up the ui corresponding to that fact. if you are good enough at the game, this may never happen.
        }
    }
}
