using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickReset : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);//simply reloads the active scene. mainly for debug but is to be left in for QOL
        }
    }
}
