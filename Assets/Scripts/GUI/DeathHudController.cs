using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathHudController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [HideInInspector] public bool playerDie;
    private void Start()
    {
        gameOverScreen.SetActive(false);
    }
   private void Update()
   {
        if (playerDie)
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
   }
}
