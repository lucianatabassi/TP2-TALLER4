using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que "MainMenu" sea el nombre correcto de tu escena
    }
}
