using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public GameObject animacionAgrandar; // Asigna el GameObject con la animación "Agrandar"
    public GameObject animacionIdle; // Asigna el GameObject con la animación "Idle"
    private bool animacionActiva = false;

    void Start()
    {
        // Inicialmente, desactiva la animación "Agrandar" y muestra la animación "Idle"
        if (animacionAgrandar != null)
        {
            animacionAgrandar.SetActive(false);
        }
        if (animacionIdle != null)
        {
            animacionIdle.SetActive(true);
        }
    }

    // Método para activar o desactivar la animación "Agrandar"
    public void ActivarAnimacionAgrandar(bool activar)
    {
        if (animacionAgrandar != null && animacionIdle != null)
        {
            animacionAgrandar.SetActive(activar);
            animacionIdle.SetActive(!activar);
        }
        animacionActiva = activar;
    }
}
