using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public GameObject animacionAgrandar; // Asigna el GameObject con la animaci�n "Agrandar"
    public GameObject animacionIdle; // Asigna el GameObject con la animaci�n "Idle"
    private bool animacionActiva = false;

    void Start()
    {
        // Inicialmente, desactiva la animaci�n "Agrandar" y muestra la animaci�n "Idle"
        if (animacionAgrandar != null)
        {
            animacionAgrandar.SetActive(false);
        }
        if (animacionIdle != null)
        {
            animacionIdle.SetActive(true);
        }
    }

    // M�todo para activar o desactivar la animaci�n "Agrandar"
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
