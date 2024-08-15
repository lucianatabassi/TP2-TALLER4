using UnityEngine;

public class ControladorRombo : MonoBehaviour
{
    public GameObject animacionAgrandar; // Asigna el GameObject con la animaci�n "Agrandar"
    public GameObject animacionIdle; // Asigna el GameObject con la animaci�n "Idle"
    public Transform estrella; // Asigna el Transform del GameObject estrella
    public float anguloRotacionLeve = 10f; // �ngulo de rotaci�n leve para ajustar la orientaci�n

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

    void Update()
    {
        // Si la animaci�n est� activa, rota el rombo hacia la estrella
        if (animacionActiva && estrella != null)
        {
            RotarHaciaEstrella();
        }
    }

    // M�todo para activar o desactivar la animaci�n "Agrandar"
    public void ActivarAnimacionAgrandar(bool activar)
    {
        if (animacionAgrandar != null && animacionIdle != null)
        {
            animacionAgrandar.SetActive(activar);
            animacionIdle.SetActive(!activar);
            Debug.Log($"{gameObject.name} - Animaci�n Agrandar: {activar}");
        }
        animacionActiva = activar;
    }

    // M�todo para rotar el rombo hacia la estrella
    private void RotarHaciaEstrella()
    {
        Vector3 direccion = (estrella.position - transform.position).normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        Debug.Log($"Rombo: {gameObject.name}, �ngulo: {angulo}, Direcci�n: {direccion}");
        transform.rotation = Quaternion.Euler(0, 0, angulo + anguloRotacionLeve);
    }
}
