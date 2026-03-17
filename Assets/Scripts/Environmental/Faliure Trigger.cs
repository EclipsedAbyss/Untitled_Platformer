using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class FaliureTrigger : MonoBehaviour
{
    private DeathHudController getDeathHud;
    private void Start()
    {
        getDeathHud = FindFirstObjectByType<DeathHudController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        getDeathHud.playerDie = true;
    }
}
