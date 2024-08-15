using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonController : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
